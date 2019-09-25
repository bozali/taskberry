import { Component, OnInit, NgModule } from '@angular/core';
import { GroupsComponent } from '../groups/groups.component';
import { NbDialogRef, NbToastrService, NbToastRef, NbButtonModule, NbCardModule, NbIconModule, NbTooltipModule, NbDialogService } from '@nebular/theme';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { GroupsService } from '../api';

export class NewGroup {
  public groupName: string;
  public groupDescription?: string;

  constructor(groupName: string, groupDescription: string) {
    this.groupName = groupName;
    this.groupDescription = groupDescription;
  }
}

@Component({
  selector: 'app-groups-add',
  templateUrl: './groups-add.component.html',
  styleUrls: ['./groups-add.component.scss']
})

@NgModule({
  imports: [
    NbCardModule,
    NbButtonModule,
    NbIconModule,
    NbTooltipModule
  ],
})

export class GroupsAddComponent {
  public groupName: string;
  public groupDescription: string;
  public submitted = false;

  constructor(private dialogService: NbDialogService, private router: Router,
              private toastrService: NbToastrService, protected dialogRef: NbDialogRef<GroupsComponent>) { }

  public AddNewGroup() {
    const toastRef: NbToastRef = this.toastrService.show('Du hast erfolgreich eine neue Gruppe hinzugefügt.', 'Gruppe hinzugefügt');
    this.submitted = true;

    const newGroup: any = {};
    newGroup.groupName = this.groupName;
    newGroup.groupDescription = this.groupDescription;

    this.dialogRef.close(newGroup);
  }


  close() {
    this.dialogRef.close();
  }

}
