import { Component, OnInit, NgModule } from '@angular/core';
import { Router } from '@angular/router'; // we also need angular router for Nebular to function properly
import { NbThemeModule, NbLayoutModule, NbSidebarModule, NbButtonModule, NbTabsetModule, NbToastRef, NbToastrService, NbDialogService, NbDialogRef } from '@nebular/theme';
import { LoginComponent } from '../login/login.component';
import { AuthService } from '../auth.service';

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
  NbDialogService
  ]
})
export class HeaderBarComponent implements OnInit {

  public dashboardVisible = false;
  public groupsVisible = false;
  public logoutVisible = false;
  public loginVisibile = true;

  constructor(private authService: AuthService, private router: Router,
    private toastrService: NbToastrService, public dialogService: NbDialogService) { }

  ngOnInit() {
    if (!this.authService.isAuthenticated()) {
      this.defaultUserLoggedOutView();
    } else {
      this.defaultUserLoggedInView();
    }
  }

public defaultUserLoggedInView() {
  this.dashboardVisible = true;
  this.groupsVisible = true; // Add extr authentification for teachers
  this.logoutVisible = true;
  this.loginVisibile = false;
  this.router.navigate(['/dashboard']);
}

public defaultUserLoggedOutView() {
  this.dashboardVisible = false;
  this.groupsVisible = false;
  this.logoutVisible = false;
  this.loginVisibile = true;

  this.router.navigate(['/']);
}

public OpenLogin() {
  const dialogRef = this.dialogService.open(LoginComponent, { hasBackdrop: true, closeOnBackdropClick: false  })
  .onClose.subscribe(loggedIn => loggedIn && this.defaultUserLoggedInView());
}

  /**
   * Changed Tab
   */
  public SelectedTabChanged(fisch) {
    switch (fisch) {
      case 'Dashboard':
          this.router.navigate(['/dashboard']);
          break;
      case 'Gruppen':
          this.router.navigate(['/groups']);
          break;
          case 'Ausloggen':
              if (this.authService.logout()) {
              this.defaultUserLoggedOutView();
              this.toastrService.show('Du hast dich erfolgreich vom System abgemeldet!', 'Ausgeloggt');
              }
              break;
          case 'Einloggen':
            this.OpenLogin();
            break;
      default:
          this.router.navigate(['/login']);
          break;
    }
  }
}
