import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from '../services/AuthService';

@Component({
  selector: 'signin-success-component',
  templateUrl: './signout.component.html'
})
export class SignoutComponent {

  constructor(private authService: AuthService) { }

  ngOnInit() {

    this.authService.logout();
  }
  
}
