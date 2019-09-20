import { Component, NgModule } from '@angular/core';
import { NbTooltipModule, NbIconModule, NbButtonModule, NbCardModule, NbInputModule, NbToastrService, NbDialogRef } from '@nebular/theme';
import { HeaderBarComponent } from '../header-bar/header-bar.component';
import { AuthService } from '../auth.service';
import { isNullOrUndefined } from 'util';
import { AuthenticationService, GroupsService } from '../api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

@NgModule({
  imports: [
    NbCardModule,
    NbButtonModule,
    NbIconModule,
    NbTooltipModule,
    NbInputModule
  ]
})

export class LoginComponent {
  Username: string;
  PasswordInputWrong = 0;
  Password: string;
  PasswordFieldVisible = false;
  // tslint:disable-next-line: max-line-length
  constructor(private authenticationService: AuthenticationService, private groupsService: GroupsService, private router: Router, public dialogRef: NbDialogRef<HeaderBarComponent>, public authService: AuthService,
              public toastrService: NbToastrService) { }

  close() {
    this.dialogRef.close(this.Username);
  }

  public Login() {

    // Validate Username
    // if email exists in moodle database
    //    Show Password Field
    // else
    //    Proper User Notification

  if (this.PasswordFieldVisible) {
      // close dialog and route to other component?
      this.dialogRef.close({username: this.Username, password: this.Password});
  } else if(isNullOrUndefined(this.Password) && this.PasswordFieldVisible) {
      this.toastrService.show('Die eingegebenen Einlog Daten waren nicht korrekt!', 'Fehler beim Einloggen');
  } else if (this.PasswordFieldVisible === false) {
      this.PasswordFieldVisible = true;
      } else {
        this.toastrService.show('Fehler beim einloggen!', 'Status');
     }

  }

}
