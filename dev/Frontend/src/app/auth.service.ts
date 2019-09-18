import { Injectable, NgModule } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { AuthenticationService, User, GroupsService, UsersService } from './api';

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
      console.log('No Valid Token Present.');
      return false;
    }

  // Check whether the token is expired
    return !this.jwtHelper.isTokenExpired(token, 600);
  }

  public async login(userName: string): Promise<boolean> {
    if (userName == null || userName === undefined) {
          console.log(userName + ' falscher benutzername wurde eingegeben.');
          return false;
      }

    if (this.isAuthenticated()) {
          console.log('already Authentificated');
          return true;
      }

      // Increase if authentification failed -> PasswordInputWrong++

    await this.authenticationService.login(userName).
       subscribe(response => {
         const token = ( response as any).token;
         const id = ( response as any).id;
         const firstName = ( response as any).firstName;
         const lastName = ( response as any).lastName;
         const email = ( response as any).email;

        // Define some model e.g. AuthentificationModel
         localStorage.setItem('jwt', token);
         localStorage.setItem('userId', id);
         localStorage.setItem('userFirstName', firstName);
         localStorage.setItem('userLastName', lastName);
         localStorage.setItem('email', email);
        // this.invalidLogin = false;

         this.authenticationService.configuration.apiKeys = {'Authorization': token};
         this.groupsService.configuration.apiKeys = {'Authorization': token};
         //this.userService.configuration.apiKeys = {'Authorization': token}; add to app.module falls required

          //this.authenticationService.configuration.apiKeys['Authorization'] = token;
         this.router.navigate(['/dashboard']);
         return true;
       }, err => {
         console.log(err);
         return false;
        // this.PasswordInputWrong++;
        // this.invalidLogin = true;
       });
    }

    public async logout() {
      if (this.isAuthenticated()) {
          await this.authenticationService.logout().subscribe(response => {
            this.isLoggedIn = false;
            localStorage.removeItem('jwt');
            localStorage.removeItem('userId');
            localStorage.removeItem('userFirstName');
            localStorage.removeItem('userLastName');
            localStorage.removeItem('email');
            return true;
          },
           err => {
            console.log('Couldnt log out! ' + err);
            return false;
           });
    
         } else {
           return false;
      }
    }
}
