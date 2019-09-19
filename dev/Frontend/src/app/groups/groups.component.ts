import { Component, OnInit, NgModule, TemplateRef } from '@angular/core';
import { GroupsViewModel, GroupViewModel } from '../models/GroupsViewModel';
import { NbTreeGridModule, NbIconModule, NbButtonModule, NbTooltipModule, NbDialogService,
   NbSortDirection, NbSortRequest, NbTreeGridDataSourceBuilder, NbTreeGridDataSource, NbToastrService } from '@nebular/theme';
import { faTrash, faUserPlus, faUserTie, faUserAltSlash, faUsers, faEdit, faUserEdit } from '@fortawesome/free-solid-svg-icons';
import { GroupsAddUserComponent } from '../groups-add-user/groups-add-user.component';
import { GroupsAddComponent } from '../groups-add/groups-add.component';
import { Group, GroupsService, User, UsersService } from '../api';
import { group } from '@angular/animations';
import { GroupsEditComponent } from '../groups-edit/groups-edit.component';

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
  selectedGroupRow = '-1';

  // Modals
  addUserToGroupModal;

  // Font Awesome Icons
  trashIcon = faTrash;
  groupIcon = faUsers;
  userIcon = faUserTie;
  addUserIcon = faUserPlus;
  deleteUserIcon = faUserAltSlash;
  editGroupIcon = faEdit;

  // Tabelle
  customColumn = 'Gruppen / Mitglieder Name';
  defaultColumns = [ 'Beschreibung', 'Aktionen'];
  selectedGroupId = '';

  allColumns = [ this.customColumn, ...this.defaultColumns];

  data: TreeNode <FSEntry>[] = [
    /*{
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
  }*/];

  constructor(private usersService: UsersService, private dialogService: NbDialogService,
              private dataSourceBuilder: NbTreeGridDataSourceBuilder<FSEntry>,
              private toastrService: NbToastrService, private groupsService: GroupsService) {
    this.dataSource = this.dataSourceBuilder.create(this.data);
  }

  ngOnInit() {
    this.InitializeGroupsTable();
  }

  private async InitializeGroupsTable() {
    const allGroups = await this.groupsService.getGroups().toPromise();

    if (allGroups !== undefined) {
      allGroups.forEach(grp => {
          this.AddGroupToTable(grp);
      });
      this.loaded = true;
      }
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
    if (groupIndex !== -1) {
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

  public async DeleteGroup(groupId: string) {
    // Add HttpDelete
    const resultText = await this.groupsService.deleteGroup(groupId).toPromise();

    if (resultText != undefined || resultText != null) {

      // Remove From Grid
      const groupIndex = this.data.findIndex(w => w.data.id === groupId && w.data.type === 1);
      let groupName = '';

      if (groupIndex !== -1) {
        groupName = this.data[groupIndex].data.name;
        this.data.splice(groupIndex, 1);

        // Update Grid (reinstantiate)
        this.dataSource = this.dataSourceBuilder.create(this.data);

        // Return proper status message

        // this.toastrService.show('Du hast erfolgreich die Gruppe ' + groupName + ' entfernt.',
        // 'Gruppe erfolgreich entfernt.');
      }
    }

    this.toastrService.show(resultText);
  }

  public ShowAddGroupWindow() {
    const modal = this.dialogService.open(GroupsAddComponent,
      { hasBackdrop: true, closeOnBackdropClick: true  })
        .onClose.subscribe(result =>  result && this.AddNewGroup(result.groupName, result.groupDescription));
  }


  public ShowEditGroupWindow(groupId: string, groupName: string, groupDescription: string) {
    const modal = this.dialogService.open(GroupsEditComponent,
      { hasBackdrop: true, closeOnBackdropClick: true  });

    const editComponent = modal.componentRef.instance;
    editComponent.id = groupId;
    editComponent.name = groupName;
    editComponent.description = groupDescription;

    modal.onClose.subscribe(result =>  result &&
          this.UpdateGroupChanges(editComponent.id, editComponent.name, editComponent.description)
      );
  }

  public async UpdateGroupChanges(groupId: string, groupName: string, groupDescription: string) {
    const group: Group = {
      id: groupId,
      name: groupName,
      description: groupDescription
    };

    const updatedGroup = await this.groupsService.editGroup(group).toPromise();

    if (updatedGroup !== undefined) {
    // Update Grid properly

    // Add received Group to Grid data
      // this.AddGroupToTable(newGroup);
    }
  }

  public ShowAddUserWindow(groupId: string) {
    this.selectedGroupId = groupId;
    this.dialogService.open(GroupsAddUserComponent, { hasBackdrop: true, closeOnBackdropClick: true  })
    .onClose.subscribe(users => this.layer(this.selectedGroupId, users));


  }

  public layer(groupIdt: string, users: User[]) {
    if (this.selectedGroupId !== '') {
      this.AddUsersToGroup(this.selectedGroupId, users);

    }
  }

  public async AddUsersToGroup(groupIdt: string, users: User[]) {

      // Add HTTP POST

       const userIds = new Array();

       users.forEach(us => {
        userIds.push(us.id);
      });

       const groupe = await this.groupsService.assignUsersToGroup(userIds, groupIdt).toPromise();

       if (groupe == null || groupe === undefined) {
        return;
    }

       users.forEach(user => {

      const usersToAddToGroup: TreeNode<FSEntry> = {
        data: { id: user.id.toString(), groupId: groupIdt, name: user.lastName + ' ' + user.firstName, description: '', type: 2},
        children: [ ]
      };

      // Add received Group to Grid
      const groupIndex = this.data.findIndex(w => w.data.id === groupIdt && w.data.groupId === '-1' && w.data.type === 1);
      if (groupIndex !== -1) {
      const userIndex = this.data[groupIndex].children.findIndex(w => w.data.id === user.id.toString() && w.data.type === 2);
      if (userIndex !== -1) {
        // User ist bereits in dieser Gruppe
      } else {
        this.data[groupIndex].children.push(usersToAddToGroup);
     }
    }
    });
       this.dataSource = this.dataSourceBuilder.create(this.data);
  }

  public async AddNewGroup(groupName: string, groupDescription: string) {
    // Add HTTP POST
    const group: Group = {
      name: groupName,
      description: groupDescription
    };

    const newGroup = await this.groupsService.createGroup(group).toPromise();

    if (newGroup !== undefined) {
    // Add received Group to Grid data
      this.AddGroupToTable(newGroup);
    }
  }

  public async AddGroupToTable(group: Group) {
        // Add received Group to Grid data
        const groupToAddToViewTable: TreeNode<FSEntry> = {
          data: { id: group.id, groupId: '-1', name: group.name, description: group.description, type: 1},
          children: [ ]
        };

        const usersInGroup = await this.usersService.getUsersByGroupId(group.id).toPromise();

        if (usersInGroup != null) {
        usersInGroup.forEach(member => {
          // Prevent Dublicates in Table (not in database)
          const userIndex = groupToAddToViewTable.children.findIndex(w => w.data.type === 2 && w.data.id === member.id.toString());
          if (userIndex === -1) {
            const userToAddToGroup: TreeNode<FSEntry> = {
              data: { id: member.id.toString(), groupId: group.id,
                 name: member.lastName + ' ' + member.firstName, description: '', type: 2},
              children: [ ]
            };
            groupToAddToViewTable.children.push(userToAddToGroup);
        }
        });
        }
        this.data.push(groupToAddToViewTable);
        // Update Grid (reinstantiate)
        this.dataSource = this.dataSourceBuilder.create(this.data);
  }
}
