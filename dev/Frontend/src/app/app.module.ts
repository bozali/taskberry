import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NbThemeModule, NbLayoutModule, NbSidebarModule, NbButtonModule, NbTabsetModule, NbToastrModule, NbTreeGridModule, NbTooltipModule, NbWindowModule, NbDialogModule, NbCardModule, NbSelectModule, NbInputModule, NbAccordionModule, NbIconModule, NbUserModule } from '@nebular/theme';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import { HeaderBarComponent } from './header-bar/header-bar.component';
import { FooterBarComponent } from './footer-bar/footer-bar.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { GroupsComponent } from './groups/groups.component';
import { HttpClientModule } from '@angular/common/http';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { GroupsAddUserComponent } from './groups-add-user/groups-add-user.component';
import { LoginComponent } from './login/login.component';
import { GroupsAddComponent } from './groups-add/groups-add.component';
import { AuthGuardGuard } from './auth-guard.guard';
import { JwtModule, JwtModuleOptions } from '@auth0/angular-jwt';
import { AuthenticationService, GroupsService, UsersService, TasksService } from './api';
import { GroupsEditComponent } from './groups-edit/groups-edit.component';
import { DashboardMyBoardComponent } from './dashboard-my-board/dashboard-my-board.component';
import { DashboardGroupBoardsComponent } from './dashboard-group-boards/dashboard-group-boards.component';
import { DashboardGroupBoardComponent } from './dashboard-group-board/dashboard-group-board.component';
import { BlankComponent } from './blank/blank.component';
import { UserLogoutComponent } from './user-logout/user-logout.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { DashboardGroupChangeAssigneeComponent } from './dashboard-group-change-assignee/dashboard-group-change-assignee.component';

export function getToken() {
  return localStorage.getItem('token');
}

// tslint:disable-next-line: variable-name
const JWT_Module_Options: JwtModuleOptions = {
  config: {
    tokenGetter: getToken
      // whitelistedDomains: yourWhitelistedDomains
  }
};

@NgModule({
  declarations: [
    AppComponent,
    HeaderBarComponent,
    FooterBarComponent,
    DashboardComponent,
    GroupsComponent,
    GroupsAddUserComponent,
    LoginComponent,
    GroupsAddComponent,
    GroupsEditComponent,
    DashboardMyBoardComponent,
    DashboardGroupBoardsComponent,
    DashboardGroupBoardComponent,
    BlankComponent,
    UserLogoutComponent,
    DashboardGroupChangeAssigneeComponent
  ],
  entryComponents: [
    GroupsAddUserComponent,
    LoginComponent,
    GroupsAddComponent,
    GroupsEditComponent,
    DashboardGroupChangeAssigneeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NbThemeModule.forRoot({ name: 'dark' }),
    NbLayoutModule,
    NbEvaIconsModule,
    NbSidebarModule.forRoot(),
    NbButtonModule,
    NbTabsetModule,
    NbToastrModule.forRoot(),
    NbTreeGridModule,
    NbTooltipModule,
    FontAwesomeModule,
    NbWindowModule.forRoot(),
    NbDialogModule.forRoot(),
    NbCardModule,
    NbSelectModule,
    NbInputModule,
    JwtModule.forRoot(JWT_Module_Options),
    DragDropModule,
    NbAccordionModule,
    NbIconModule
  ],
  providers: [
      AuthGuardGuard,
      AuthenticationService,
      GroupsService,
      UsersService,
      TasksService
    //{
     // provide: BASE_PATH, useValue: 'http://localhost:50352/v2',
    //},
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

 }
