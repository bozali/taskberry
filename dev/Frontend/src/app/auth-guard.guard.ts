import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { NbToastrService } from '@nebular/theme';

@Injectable({
  providedIn: 'root'
})


export class AuthGuardGuard implements CanActivate {

  constructor(public toastrService: NbToastrService, public auth: AuthService, private router: Router) {}

  canActivate(): boolean {
    return this.auth.isAuthenticated();
  }
}
