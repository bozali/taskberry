import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { GroupsComponent } from './groups/groups.component';
import { LoginComponent } from './login/login.component';
import { AuthGuardGuard } from './auth-guard.guard';
import { NbDialogRef } from '@nebular/theme';
import { BlankComponent } from './blank/blank.component';
import { DashboardMyBoardComponent } from './dashboard-my-board/dashboard-my-board.component';


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
    path: 'my-board',
    component: DashboardMyBoardComponent,
    canActivate: [AuthGuardGuard]
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'blank',
    component: BlankComponent
  },
  {
    path: '**',
    component: BlankComponent
  },

];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
