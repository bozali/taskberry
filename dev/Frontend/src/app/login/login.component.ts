import { Component, NgModule } from '@angular/core';
import { NbTooltipModule, NbIconModule, NbButtonModule, NbCardModule, NbInputModule, NbToastrService, NbDialogRef } from '@nebular/theme';
import { HeaderBarComponent } from '../header-bar/header-bar.component';
import { AuthService } from '../auth.service';
import { isNullOrUndefined } from 'util';
import { AuthenticationService, GroupsService, UsersService } from '../api';
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
  IsRegistered = false;
  registerOrLoginText: string;
  userExistInMoodle = false;
  userDidInput = false;
  disabled = false;
  // tslint:disable-next-line: max-line-length
  constructor(private userService: UsersService, private groupsService: GroupsService, private router: Router, public dialogRef: NbDialogRef<HeaderBarComponent>, public authService: AuthService,
              public toastrService: NbToastrService) { }

  close() {
    this.dialogRef.close(this.Username);
  }

  public async Login() {
  if (this.PasswordFieldVisible) {
      // close dialog and route to other component?
      this.dialogRef.close({registered: this.IsRegistered, username: this.Username, password: this.Password});
  } else if (isNullOrUndefined(this.Password) && this.PasswordFieldVisible) {
      this.toastrService.show('Die eingegebenen Einlog Daten waren nicht korrekt!', 'Fehler beim Einloggen');
  } else if (this.PasswordFieldVisible === false) {
      await this.userService.isUserRegistered(this.Username).subscribe(isRegistered => {
        if (isRegistered) {
          this.IsRegistered = true;
          this.PasswordFieldVisible = true;
          this.registerOrLoginText = 'Benutzer gefunden, gib dein Passwort ein.';
          this.disabled = true;
          this.userDidInput = false;
        } else {
          this.IsRegistered = false;
          this.userService.userExists(this.Username).subscribe(userExist => {
            if (userExist) {
              this.registerOrLoginText = 'Unregistriert, gib dein zukünfitges Passwort nun ein!';
              this.userExistInMoodle = true;
              this.PasswordFieldVisible = true;
              this.disabled = true;
              this.userDidInput = false;
            } else {
              this.registerOrLoginText = 'Benutzer nicht gefunden!';
              this.userExistInMoodle = false;
              this.userDidInput = false;
            }
          });
        }
      },
      err => {
        this.registerOrLoginText = 'Benutzer nicht gefunden!';
        this.userExistInMoodle = false;
        this.userDidInput = true;
      });
      } else {
        this.toastrService.show('Fehler beim einloggen!', 'Status');
     }

  }

}
