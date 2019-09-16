import { Component, OnInit, NgModule, TemplateRef } from '@angular/core';
import { GroupsViewModel, GroupViewModel } from '../models/GroupsViewModel';
import { NbTreeGridModule, NbIconModule, NbButtonModule, NbTooltipModule, NbDialogService, NbSortDirection, NbSortRequest, NbTreeGridDataSourceBuilder, NbTreeGridDataSource, NbToastrService, NbToastRef } from '@nebular/theme';
import { faTrash, faUserPlus, faUserTie, faUserAltSlash, faUsers } from '@fortawesome/free-solid-svg-icons';
import { GroupsAddUserComponent } from '../groups-add-user/groups-add-user.component';
import { GroupsAddComponent } from '../groups-add/groups-add.component';
import { GroupsService, Group } from '../api';
import { group } from '@angular/animations';

interface TreeNode<T> {
  data: T;
  children?: TreeNode<T>[];
  expanded?: boolean;
}

interface FSEntry {
  id?: string;
  name: string;
  description: string;
  type: number;
  groupId?: string;
}


@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.scss']
})

@NgModule ({
  imports: [
    NbTreeGridModule,
    NbIconModule,
    NbButtonModule,
    NbTooltipModule,
    NbToastrService,
    NbDialogService
  ]
})
export class GroupsComponent implements OnInit {
  groupsViewModel: GroupsViewModel;
  loaded = false;

  // Group Table
  sortColumn: string;
  sortDirection: NbSortDirection = NbSortDirection.NONE;
  dataSource: NbTreeGridDataSource<FSEntry>;

  // Modals
  addUserToGroupModal;

  // Font Awesome Icons
  trashIcon = faTrash;
  groupIcon = faUsers;
  userIcon = faUserTie;
  addUserIcon = faUserPlus;
  deleteUserIcon = faUserAltSlash;
  // Tabelle
  customColumn = 'Gruppen / Mitglieder Name';
  defaultColumns = [ 'Beschreibung', 'Aktionen'];

  allColumns = [ this.customColumn, ...this.defaultColumns];


  data: TreeNode<FSEntry>[] = [
    {
      data: { id: '1', groupId: '-1', name: 'Gruppe 1', description: 'Planungsgruppe', type: 1},
      children: [
        { data: { id: '2', groupId: '1', name: 'Max Musterman', description: '', type: 2} },
        { data: { id: '3', groupId: '1', name: 'Fisch von dem Fisch', description: '', type: 2} }
      ]
    },
    {
    data: { id: '2', groupId: '-1', name: 'Gruppe 2', description: 'Durchführungsgruppe', type: 1},
    children: [
      { data: { id: '4', groupId: '2', name: 'Mitglied 1', description: '', type: 2} },
      { data: { id: '5', groupId: '2', name: 'Mitglied 2', description: '', type: 2} },
      { data: { id: '6', groupId: '2', name: 'Mitglied 3',  description: '', type: 2} }
    ]
  }];

  constructor(private dialogService: NbDialogService, private dataSourceBuilder: NbTreeGridDataSourceBuilder<FSEntry>,
              private toastrService: NbToastrService, private groupsService: GroupsService) {
    this.dataSource = this.dataSourceBuilder.create(this.data);
  }

  ngOnInit() {
    this.groupsViewModel = new GroupsViewModel();
    let oneGroup: GroupViewModel = new GroupViewModel();
    oneGroup.id = 1;
    oneGroup.name = 'Test Name';
    oneGroup.description = 'Beschreibung';
    this.groupsViewModel.GroupViewModel.fill(oneGroup);
    this.loaded = true;

  }

  updateSort(sortRequest: NbSortRequest): void {
    this.sortColumn = sortRequest.column;
    this.sortDirection = sortRequest.direction;
  }

  getSortDirection(column: string): NbSortDirection {
    if (this.sortColumn === column) {
      return this.sortDirection;
    }
    return NbSortDirection.NONE;
  }


  getShowOn(index: number) {
    const minWithForMultipleColumns = 400;
    const nextColumnStep = 100;
    return minWithForMultipleColumns + (nextColumnStep * index);
  }

  public DeleteUserFromGroup(groupId: string, userId: number) {
    // Add HttpDelete

    // Remove from Grid
    const groupIndex = this.data.findIndex(w => w.data.id === groupId && w.data.groupId === '-1' && w.data.type === 1);
    if(groupIndex !== -1) {
    const userIndex = this.data[groupIndex].children.findIndex(w => w.data.id === userId.toString() && w.data.type === 2);
    if (userIndex !== -1) {
        this.data[groupIndex].
          children.splice(userIndex, 1);

        // Update Grid (reinstantiate)
        this.dataSource = this.dataSourceBuilder.create(this.data);

        // Return proper status message
        this.toastrService.show('Du hast erfolgreich den Schüler aus der Gruppe entfernt.',
        'Mitglied erfolgreich entfernt.');
      }
    }
  }

  public DeleteGroup(groupId: string) {
    // Add HttpDelete

    //Remove From Grid
    const groupIndex = this.data.findIndex(w => w.data.id === groupId && w.data.type === 1);
    let groupName: string = "";

    if(groupIndex !== -1) {
      groupName = this.data[groupIndex].data.name;
      this.data.splice(groupIndex, 1);

      // Update Grid (reinstantiate)
      this.dataSource = this.dataSourceBuilder.create(this.data);

      // Return proper status message
      this.toastrService.show('Du hast erfolgreich die Gruppe ' + groupName + ' entfernt.',
      'Gruppe erfolgreich entfernt.');
    }
  }

  public ShowAddGroupWindow() {
    let modal = this.dialogService.open(GroupsAddComponent,
      { hasBackdrop: true, closeOnBackdropClick: true  })
        .onClose.subscribe(result =>  result && this.AddNewGroup(result.groupName, result.groupDescription));
  }

  public ShowAddUserWindow(dialog: TemplateRef<any>) {
    this.dialogService.open(GroupsAddUserComponent, { hasBackdrop: true, closeOnBackdropClick: true  })
    .onClose.subscribe(s=> s && s.forEach((user: number) => {
      this.AddUserToGroup('1', user, 'Peter'); // replace with group Id (get everything from userId)
        // Update Grid (reinstantiate)
      this.dataSource = this.dataSourceBuilder.create(this.data);
    }));


  }

  public AddUserToGroup(groupIdt: string, userId: number, userName: string) {
    let usersToAddToGroup: TreeNode<FSEntry> =
      {
        data: { id: userId.toString(), groupId: groupIdt, name: userName, description: '', type: 2},
        children: [ ]
      };
      // Add HTTP POST

    // Add received Group to Grid
    const groupIndex = this.data.findIndex(w => w.data.id === groupIdt && w.data.groupId === '-1' && w.data.type === 1);
    if(groupIndex !== -1) {
      const userIndex = this.data[groupIndex].children.findIndex(w => w.data.id === userId.toString() && w.data.type === 2);
      if (userIndex !== -1) {
        // User ist bereits in dieser Gruppe
      } else {
        this.data[groupIndex].children.push(usersToAddToGroup);
     }
    }
  }

  public async AddNewGroup(groupName: string, groupDescription: string) {
    // Add HTTP POST
    let group: Group = {
      name: groupName,
      description:groupDescription
    };

    const newGroup = await this.groupsService.createGroup(group).toPromise();

    // Add received Group to Grid data
    let groupToAddToViewTable: TreeNode<FSEntry> =
    {
      data: { id: newGroup.id, groupId: '-1', name: newGroup.name, description: newGroup.description, type: 1},
      children: [ ]
    };

    this.data.push(groupToAddToViewTable);

    // Update Grid (reinstantiate)
    this.dataSource = this.dataSourceBuilder.create(this.data);
  }
}