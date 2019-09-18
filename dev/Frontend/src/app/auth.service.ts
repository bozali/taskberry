import { Injectable, NgModule } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { AuthenticationService, User, GroupsService, UsersService } from './api';
import { LoginComponent } from './login/login.component';
import { NbDialogRef } from '@nebular/theme';
import { HeaderBarComponent } from './header-bar/header-bar.component';
import { AppComponent } from './app.component';

@Injectable({
  providedIn: 'root'
})

@NgModule ({
  imports: [
    JwtHelperService
  ]
})

export class AuthService {

  public isLoggedIn: boolean;
  constructor(public authenticationService: AuthenticationService, public groupsService: GroupsService,
              public router: Router, public jwtHelper: JwtHelperService) {}

  public isAuthenticated(): boolean {
    const token = localStorage.getItem('jwt');

    if (token === undefined || token === null || token === '' || token === 'token') {
      //console.log('No Valid Token Present.');
      return false;
    }

  // Check whether the token is expired
    return !this.jwtHelper.isTokenExpired(token, 600);
  }

    public logout() {
      if (this.isAuthenticated()) {
        localStorage.removeItem('userId');
        localStorage.removeItem('userFirstName');
        localStorage.removeItem('userLastName');
        localStorage.removeItem('email');
        localStorage.removeItem('jwt');
        this.authenticationService.logout().subscribe(response => {
            this.isLoggedIn = false;
          },
           err => {
            console.log('Couldnt log out! ' + err.toString());
           });

         } else {
           this.router.navigate(['/']);
           return false;
      }
    }
}
