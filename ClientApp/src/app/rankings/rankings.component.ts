import { Component, ViewChild,OnInit } from '@angular/core';

import { AuthService } from '../services/AuthService';
import { NgbModal, NgbModalRef, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient } from '@angular/common/http';
import { BetGroup } from '../models/BetGroup';
import { Match } from '../models/Match';
import * as moment from 'moment';
import { forEach } from '@angular/router/src/utils/collection';
import { UserRank } from '../models/UserRank';
import { UserToken } from '../models/UserToken';

@Component({
  selector: 'app-home',
  templateUrl: './rankings.component.html',
})
export class RankingsComponent implements OnInit{

  userToken: UserToken;
  userRanks: UserRank[];

  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit(): void {

    this.userToken = this.authService.getUser();

    this.http
      .get<UserRank[]>('api/ranks', { params: { BetGroupId: this.userToken.BetGroupId } })
      .subscribe(userRanks =>
      {
          this.userRanks = userRanks;
      });
  }
}
