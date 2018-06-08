import { NgbModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToasterModule, ToasterService } from 'angular2-toaster';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { SigninSuccessComponent } from './signin-success/signin-success.component';
import { SigninComponent } from './signin/signin.component';
import { UserSettingComponent } from './user-setting/user-setting.component';

import { JwtModule } from '@auth0/angular-jwt';
import { AuthService } from './services/AuthService';
import { AuthGuard } from './services/AuthGaurd';
import { RankingsComponent } from './rankings/rankings.component';
import { SignoutComponent } from './signout/signout.component';

export function tokenGetter() {
  return localStorage.getItem('auth_token');
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SigninSuccessComponent,
    SigninComponent,
    UserSettingComponent,
    RankingsComponent,
    SignoutComponent
  ],
  imports: [
    BrowserAnimationsModule,
    ToasterModule.forRoot(),
    JwtModule.forRoot({
      config: {
        headerName: 'Authorization',
        authScheme:'Bearer ',
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:4200', 'localhost:44380'],
      }
    }),
    NgbModule.forRoot(),
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'dashboard', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'rankings', component: RankingsComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'signin-success', component: SigninSuccessComponent},
      { path: 'signin', component: SigninComponent },
      { path: 'signout', component: SignoutComponent },
      { path: 'user-setting', component: UserSettingComponent, canActivate: [AuthGuard]},
    ])
  ],
  providers: [AuthService, AuthGuard, NgbModal],
  bootstrap: [AppComponent]
})
export class AppModule { }

