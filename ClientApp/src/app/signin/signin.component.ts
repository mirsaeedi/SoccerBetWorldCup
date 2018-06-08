import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'signin-success-component',
  templateUrl: './signin.component.html'
})
export class SigninComponent {

  constructor(private http: HttpClient) { }
  
}
