import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { JwtToken } from "../models/JwtToken";
import { UserToken } from "../models/UserToken";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Observable } from "rxjs";
import { Router } from "@angular/router";
import { CookieService } from 'angular2-cookie/core';
import { BetGroup } from "../models/BetGroup";

@Injectable()
export class AuthService {

  selectedBetGroup: BetGroup;

  setSelectedBetGroup(betGroup: BetGroup): void {

    this.selectedBetGroup = betGroup;
  }

  getSelectedBetGroup(): BetGroup {

    return this.selectedBetGroup
  }

  constructor(private router: Router
    , private http: HttpClient
    , private jwtHelper: JwtHelperService) {
  }

  public fetchAuthToken(): Observable<JwtToken> {

    return this.http.get<JwtToken>('/signin/getToken');   
  }

  public setToken(authResult:string) {
    localStorage.setItem('auth_token', authResult);
  }


  public getToken():string {
    return localStorage.getItem('auth_token');
  }

  public isTokenExpired(): boolean {

    return this.jwtHelper.isTokenExpired();
  }

  logout() {
    
    localStorage.removeItem("auth_token");
    this.router.navigate(['/signin']);
  }

  private getTokenExpirationDate():Date {
    return this.jwtHelper.getTokenExpirationDate();
  }

  private decodeToken():string {

    return this.jwtHelper.decodeToken(this.getToken());
  }

  getUser(): UserToken {

    let rawToken = this.jwtHelper.decodeToken(this.getToken());

    let tokenBasedUser: UserToken = {

      Name: rawToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
      PhoneNumber: rawToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone'],
      Email: rawToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
      ImageUrl: rawToken['ImageUrl']
    }

    return tokenBasedUser;
  }


}
