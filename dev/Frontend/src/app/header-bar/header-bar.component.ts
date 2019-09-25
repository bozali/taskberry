import { Component, OnInit, NgModule } from '@angular/core';
import { Router } from '@angular/router'; // we also need angular router for Nebular to function properly
import { NbThemeModule, NbLayoutModule, NbSidebarModule, NbButtonModule, NbTabsetModule, NbToastRef, NbToastrService, NbDialogService, NbDialogRef } from '@nebular/theme';
import { LoginComponent } from '../login/login.component';
import { AuthService } from '../auth.service';
import { faColumns } from '@fortawesome/free-solid-svg-icons';
import { AuthenticationService, GroupsService, UsersService, TasksService } from '../api';

@Component({
  selector: 'app-header-bar',
  templateUrl: './header-bar.component.html',
  styleUrls: ['./header-bar.component.scss']
})
@NgModule({
  imports: [
  NbLayoutModule,
  NbThemeModule,
  NbSidebarModule,
  NbButtonModule,
  NbTabsetModule,
  NbDialogService,
  NbDialogRef
  ]
})
export class HeaderBarComponent implements OnInit {

  // Font Awesome Icons
  dashboardIcon = faColumns;

  public dashboardVisible = false;
  public groupsVisible = false;
  public logoutVisible = false;
  public loginVisibile = true;

  // Dialogs
  private dialog: NbDialogRef<LoginComponent>;


  // tslint:disable-next-line: max-line-length
  constructor(private taskService: TasksService, private authenticationService: AuthenticationService, private groupsService: GroupsService, private authService: AuthService, private router: Router,
              private toastrService: NbToastrService, public dialogService: NbDialogService, private usersService: UsersService) { }

  toggle() {
    this.groupsVisible = !this.groupsVisible;
  }

  ngOnInit() {
    if (!this.authService.isAuthenticated()) {
      this.defaultUserLoggedOutView();
    } else {
      this.defaultUserLoggedInView();
    }
  }

  public IsTeacher() {
    const isAdmin = localStorage.getItem('isTeacher');
    if (isAdmin === 'true') {
      return true;
    }
    return false;
  }
public defaultUserLoggedInView() {
  this.dashboardVisible = true;
  if (this.IsTeacher()) {
    this.groupsVisible = true; // Add extr authentification for teachers
  }
  this.logoutVisible = true;
  this.loginVisibile = false;
  // this.router.navigate(['/dashboard']);
}

public defaultUserLoggedOutView() {
  this.dashboardVisible = false;
  this.groupsVisible = false;
  this.logoutVisible = false;
  this.loginVisibile = true;
}

public OpenLogin() {
  // tslint:disable-next-line: max-line-length
  this.dialogService.open(LoginComponent, { hasBackdrop: true, closeOnBackdropClick: false }).onClose.subscribe(
    (result) => {
    if (result.registered) {
        this.login(result.username, result.password);
    } else {
        this.authenticationService.register(result.username, result.password).subscribe(message => {
          this.login(result.username, result.password);
        }, err => {
          this.toastrService.danger('Bei der Registrierung ist wohl etwas schiefgelaufen...', 'Problem');
        });
    }
  });
}

public DetermineLoginOrregister() {

}

  /**
   * Changed Tab
   */
  public SelectedTabChanged(fisch) {
    switch (fisch) {
      case 'Boards':
          this.router.navigate(['/dashboard']);
          break;
      case 'Gruppen':
          this.router.navigate(['/groups']);
          break;
          case 'Ausloggen':
              this.authService.logout();
              this.defaultUserLoggedOutView();
              this.toastrService.show('Du hast dich erfolgreich vom System abgemeldet!', 'Ausgeloggt');
              this.router.navigate(['/blank']);
              break;
          case 'Einloggen':
            this.OpenLogin();
            break;
            case 'Mein Board':
            this.router.navigate(['/my-board']);
            break;
      default:
        this.router.navigate(['/']);
        this.defaultUserLoggedOutView();
        break;
    }
  }




  public login(userName: string, password: string) {
    if (userName == null || userName === undefined) {
          console.log(userName + ' falscher benutzername wurde eingegeben.');
          return false;
      }

    if (this.authService.isAuthenticated()) {
          console.log('already Authentificated');
          this.defaultUserLoggedInView();
          this.dialog.close(false);
          this.router.navigate(['/dashboard']);
          return true;
      }

      // Increase if authentification failed -> PasswordInputWrong++

    this.authenticationService.login(userName, password).
      subscribe(response => {
        const token = (response as any).token;
        const id = (response as any).id;
        const firstName = (response as any).firstName;
        const lastName = (response as any).lastName;
        const email = (response as any).email;
        const isAdmin = (response as any).isTeacher;
        // Define some model e.g. AuthentificationModel
        localStorage.setItem('jwt', token);
        localStorage.setItem('userId', id);
        localStorage.setItem('userFirstName', firstName);
        localStorage.setItem('userLastName', lastName);
        localStorage.setItem('email', email);
        localStorage.setItem('isTeacher', isAdmin);
        // this.invalidLogin = false;
        this.authenticationService.configuration.apiKeys = { Authorization: token };
        this.groupsService.configuration.apiKeys = { Authorization: token };
        this.usersService.configuration.apiKeys = { Authorization: token };
        this.taskService.configuration.apiKeys = { Authorization: token };

        this.defaultUserLoggedInView();
        this.toastrService.primary('Du hast dich erfolgreich angemeldet!', 'Anmeldung');
        //this.dialog.close(false);
        this.router.navigate(['/dashboard']);
      }, err => {
        this.toastrService.danger('Das falsche Passwort wurde eingegeben.', 'Falsches Passwort');
        console.log('login failed' + err);
        // this.router.navigate(['/']);
        // this.PasswordInputWrong++;
        // this.invalidLogin = true;
      });
    }
}
