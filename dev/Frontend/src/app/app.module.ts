import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NbThemeModule, NbLayoutModule, NbSidebarModule, NbButtonModule, NbTabsetModule, NbToastrModule, NbTreeGridModule, NbTooltipModule, NbWindowModule, NbDialogModule, NbCardModule, NbSelectModule, NbInputModule } from '@nebular/theme';
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
import { AuthenticationService, GroupsService } from './api';


// tslint:disable-next-line: variable-name
const JWT_Module_Options: JwtModuleOptions = {
  config: {
      // tokenGetter: yourTokenGetter,
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
  ],
  entryComponents: [GroupsAddUserComponent, LoginComponent, GroupsAddComponent],
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
  ],
  providers: [
      AuthGuardGuard,
      AuthenticationService,
      GroupsService
    //{
      // provide: BASE_PATH, useValue: 'http://localhost:50352/v2',
    //},
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

 }
