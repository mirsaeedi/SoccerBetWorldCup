import { Component, OnInit, ViewChild  } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UserSetting } from '../models/UserSetting';
import { AuthService } from '../services/AuthService';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { NgbModal, ModalDismissReasons,  NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { BetGroup } from '../models/BetGroup';
import { Router } from '@angular/router';

@Component({
  selector: 'signin-success-component',
  templateUrl: './user-setting.component.html'
})
export class UserSettingComponent implements OnInit{

  userImageUrl= '/assets/images/default-user.png';
  closeResult: string;
  viewModel: UserSetting;
  @ViewChild('newGroupSuccessModal') private newGroupSuccessModal;
  newJoinedBetGroup: BetGroup;
  openedModal: NgbModalRef;

  constructor(private http: HttpClient, private authService: AuthService, private modalService: NgbModal, private router: Router) { }

  open(content) {

    if (this.openedModal != null)
      this.openedModal.close();

    this.openedModal = this.modalService.open(content);

    this.openedModal.result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  ngOnInit(): void {

    var userToken = this.authService.getUser();

    this.viewModel = {
      Name: userToken.Name,
      PhoneNumber: userToken.PhoneNumber,
      Email: userToken.Email,
      ImageUrl: userToken.ImageUrl
    };

    if (this.viewModel.ImageUrl != null)
      this.userImageUrl = this.viewModel.ImageUrl;

  }

  joinBetGroup(groupCode: string): void {

    this.http.post<BetGroup>('/api/join', {GroupCode:groupCode})
      .pipe(
      catchError(this.handleError)
    )
      .subscribe(group => {
        this.authService.joinGroup(groupCode)
          .subscribe(data => {
            this.newJoinedBetGroup = group;
            this.authService.setToken(data.Token);
            this.open(this.newGroupSuccessModal);
          })
        
      });

  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // return an observable with a user-facing error message
    return throwError(
      'Something bad happened; please try again later.');
  };

  goToDashboard(): void {

    this.router.navigateByUrl('/dashboard');
    this.openedModal.close();
  }
}
