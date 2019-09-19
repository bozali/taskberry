import { Component, OnInit, NgModule } from '@angular/core';
import { NbCardModule, NbButtonModule, NbIconModule, NbTooltipModule, NbDialogRef, NbToastrService, NbDialogService } from '@nebular/theme';
import { GroupsComponent } from '../groups/groups.component';
import { Router } from '@angular/router';
import { Group } from '../api';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'app-groups-edit',
  templateUrl: './groups-edit.component.html',
  styleUrls: ['./groups-edit.component.scss']
})

@NgModule({
  imports: [
    NbCardModule,
    NbButtonModule,
    NbIconModule,
    NbTooltipModule
  ],
})

export class GroupsEditComponent {
  public id: string;
  public name: string;
  public description?: string;

  loaded = false;

  constructor(private dialogService: NbDialogService, private router: Router,
              private toastrService: NbToastrService, protected dialogRef: NbDialogRef<GroupsComponent> ) { }


  public EditGroup() {
    if (!isNullOrUndefined(this.id) && !isNullOrUndefined(this.name))
    {
      this.dialogRef.close({name: this.name, description: this.description});
    }
  }

  close() {
    this.dialogRef.close();
  }

}
