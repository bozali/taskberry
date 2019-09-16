import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { GroupsComponent } from './groups/groups.component';
import { LoginComponent } from './login/login.component';
import { AuthGuardGuard } from './auth-guard.guard';
import { NbDialogRef } from '@nebular/theme';


const routes: Routes = [
    {
    path: 'dashboard',
    children: [
      {
        path: '',
        component: DashboardComponent,
        canActivate: [AuthGuardGuard]
      },
   ]},
   {
    path: 'groups',
    component: GroupsComponent,
    canActivate: [AuthGuardGuard] // Add Teacher Guard later
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '**',
    redirectTo: ''
  },

];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }