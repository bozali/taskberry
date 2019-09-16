import { Component, NgModule } from '@angular/core';
import { NbTooltipModule, NbIconModule, NbButtonModule, NbCardModule, NbInputModule, NbToastrService, NbDialogRef } from '@nebular/theme';
import { HeaderBarComponent } from '../header-bar/header-bar.component';
import { AuthService } from '../auth.service';

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
  ],
})

export class LoginComponent {
  Username: string;
  PasswordInputWrong = 0;
  constructor(protected dialogRef: NbDialogRef<HeaderBarComponent>, public authService: AuthService,
              public toastrService: NbToastrService) { }

  close() {
    this.dialogRef.close(false);
  }


public Login() {
  // Validate Username
  if (this.authService.login(this.Username)) {
    // Send proper status message
    this.toastrService.show('Du hast dich erfolgreich am System angemeldet!', 'Eingeloggt');

    // close dialog and route to other component?
    this.dialogRef.close(true);
  }
   else {
      this.toastrService.show('Du bist bereits eingeloggt!', 'Status');
   }

}
}
