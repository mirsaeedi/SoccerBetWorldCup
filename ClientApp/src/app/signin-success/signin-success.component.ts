import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../services/AuthService';
import { Router } from '@angular/router';

@Component({
  selector: 'signin-success-component',
  templateUrl: './signin-success.component.html'
})
export class SigninSuccessComponent implements OnInit {

  constructor(private router: Router, private http: HttpClient, private authService: AuthService) { }

  ngOnInit() {

    this.authService.fetchAuthToken()
      .subscribe(result => {

        this.authService.setToken(result.Token);
        this.router.navigate(['/user-setting']);
      });
  }
}
