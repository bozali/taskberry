import { Component, OnInit, TemplateRef, NgModule, Input } from '@angular/core';
import { NbCardModule, NbButtonModule, NbIconModule, NbTooltipModule, NbToastRef, NbToastrService, NbDialogRef, NbDialogService } from '@nebular/theme';
import { faChevronLeft } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';
import { GroupsComponent } from '../groups/groups.component';
import { UsersService, User, TasksService } from '../api';
import { DashboardGroupBoardComponent } from '../dashboard-group-board/dashboard-group-board.component';
@Component({
  selector: 'app-dashboard-group-change-assignee',
  templateUrl: './dashboard-group-change-assignee.component.html',
  styleUrls: ['./dashboard-group-change-assignee.component.scss']
})

@NgModule({
  imports: [
    NbCardModule,
    NbButtonModule,
    NbIconModule,
    NbTooltipModule
  ]
})

export class DashboardGroupChangeAssigneeComponent implements OnInit {
  public allUser: User[];
  public selectedUser;
  public loaded = false;
  public taskId;
  public groupId;

  // Font Awesome Icons
backButtonIcon = faChevronLeft;

// tslint:disable-next-line: max-line-length
constructor(private taskService: TasksService, protected dialogRef: NbDialogRef<DashboardGroupBoardComponent>, private router: Router, private userService: UsersService, private toastrService: NbToastrService) { }

ngOnInit() {
}

public AssignSelectedUserToTask() {
  const toastRef: NbToastRef = this.toastrService.show('Du hast erfolgreich die Schüler zur Gruppe hinzugefügt.', 'Benutzer hinzugefügt');


  const userIndex = this.allUser.findIndex(w => w.id.toString() === this.selectedUser);
  if (userIndex !== -1) {
    this.loaded = false;
  }

  // tslint:disable-next-line: no-unused-expression
  this.taskService.assignTaskToUser(this.taskId, this.selectedUser).subscribe( result => { result &&
    this.toastrService.success('Aufgabe wurde erfolgreich dem Benutzer zugewiesen.',
                              'Aufgaben Zuweisung') && this.dialogRef.close
                                  ({assigneeId: this.selectedUser});
    this.loaded = true;
  }, err => {
    this.toastrService.danger('Aufgabe konnte nicht zugewiesen werden.', 'Fehler beim zuweisen.');
  });
}
close() {
  this.dialogRef.close();
}

}
