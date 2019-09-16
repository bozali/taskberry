import { Component, OnInit, TemplateRef, NgModule } from '@angular/core';
import { NbCardModule, NbButtonModule, NbIconModule, NbTooltipModule, NbToastRef, NbToastrService, NbDialogRef, NbDialogService } from '@nebular/theme';
import { faChevronLeft } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';
import { GroupsComponent } from '../groups/groups.component';
@Component({
  selector: 'app-groups-add-user',
  templateUrl: './groups-add-user.component.html'
})

@NgModule({
  imports: [
    // ...
    NbCardModule,
    NbButtonModule,
    NbIconModule,
    NbTooltipModule
  ],
})
export class GroupsAddUserComponent {
public selectedClass = '';
public selectedUser = [];
public classSelected = false;

constructor(private router: Router, private toastrService: NbToastrService, protected dialogRef: NbDialogRef<GroupsComponent>) { }

// Font Awesome Icons
backButtonIcon = faChevronLeft;

//

public SearchForClass() {
  this.classSelected = true;
}

public ResetClassSelected()
{
  this.classSelected = false;
}

public AddSelectedUserToGroup() {
  const toastRef: NbToastRef = this.toastrService.show('Du hast erfolgreich die Schüler zur Gruppe hinzugefügt.', 'Benutzer hinzugefügt');

  const userForGroup: any = {};
  userForGroup.users = this.selectedUser;

  this.dialogRef.close(this.selectedUser);
}

close() {
  this.dialogRef.close();
}
}
