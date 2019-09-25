import { Component, OnInit, TemplateRef, NgModule } from '@angular/core';
import { NbCardModule, NbButtonModule, NbIconModule, NbTooltipModule, NbToastRef, NbToastrService, NbDialogRef, NbDialogService } from '@nebular/theme';
import { faChevronLeft } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';
import { GroupsComponent } from '../groups/groups.component';
import { UsersService } from '../api';

@Component({
  selector: 'app-groups-add-user',
  templateUrl: './groups-add-user.component.html'
})

@NgModule({
  imports: [
    NbCardModule,
    NbButtonModule,
    NbIconModule,
    NbTooltipModule
  ]
})
export class GroupsAddUserComponent implements OnInit {

public allUser = [];
public selectedClass = '';
public selectedUser = [];
public classSelected = false;
public allClasses = [];

// tslint:disable-next-line: max-line-length
constructor(private router: Router, private userService: UsersService, private toastrService: NbToastrService, protected dialogRef: NbDialogRef<GroupsComponent>) { }

// Font Awesome Icons
backButtonIcon = faChevronLeft;

//

ngOnInit() {
  this.LoadAvailableClasses();
}

private async LoadAvailableClasses() {
  this.allClasses = await this.userService.getClasses().toPromise();

}

public async SearchForClass() {
  this.allUser = await this.userService.getUsersInClass(this.selectedClass).toPromise();
  if (this.allUser === undefined || this.allUser == null || this.allUser[0] === null) {
    this.toastrService.danger('In dieser Klasse befinden sich keine Schüler.', 'Klasse leer');
  } else {
    this.classSelected = true;
  }

}

public ResetClassSelected() {
  this.classSelected = false;
}

public AddSelectedUserToGroup() {
  const toastRef: NbToastRef = this.toastrService.show('Du hast erfolgreich die Schüler zur Gruppe hinzugefügt.', 'Benutzer hinzugefügt');

  const usersToAdd = new Array();

  this.selectedUser.forEach(usra => {
    let userIndex = this.allUser.findIndex(w => w.id.toString() === usra);
    if (userIndex !== -1) {
        usersToAdd.push(this.allUser[userIndex]);
    }
  });

  this.dialogRef.close(usersToAdd);
}

close() {
  this.dialogRef.close();
}
}
