import { Component } from '@angular/core';
import { AuthService } from './services/AuthService';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  isSignedIn = false;

  constructor(public authService: AuthService) {

    this.isSignedIn = !this.authService.isTokenExpired();
  }

}
