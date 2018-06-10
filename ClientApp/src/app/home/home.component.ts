import { Component, ViewChild,OnInit } from '@angular/core';

import { AuthService } from '../services/AuthService';
import { NgbModal, NgbModalRef, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BetGroup } from '../models/BetGroup';
import { Match } from '../models/Match';
import * as moment from 'moment';
import { forEach } from '@angular/router/src/utils/collection';
import { MatchPrediction } from '../models/MatchPrediction';
import { Team } from '../models/Team';
import { BonusPrediction } from '../models/BonusPrediction';
import { MatchStat } from '../models/MatchStat';
import { ToasterService, ToasterConfig } from 'angular2-toaster';
import { UserStatus } from '../models/UserStatus';
import { group } from '@angular/animations';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit{

  filteredMatches: Match[];
  userStatus= new UserStatus();
  selectedMatchStats: MatchStat[];
  teams: Team[]
  bonusPredictions: BonusPrediction[];
  myBetGroups: BetGroup[];
  selectedBetGroup: BetGroup;
  selectedMatch: Match;
  matchPrediction= new MatchPrediction();
  showBetGroupSelection = false;
  matches: Match[];
  closeResult: string;

  firstTeamPrediction = new BonusPrediction();
  secondTeamPrediction = new BonusPrediction();
  thirdTeamPrediction = new BonusPrediction();

  firstTeamInGroupAPrediction=new BonusPrediction();
  secondTeamInGroupAPrediction = new BonusPrediction();

  firstTeamInGroupBPrediction = new BonusPrediction();
  secondTeamInGroupBPrediction = new BonusPrediction();

  firstTeamInGroupCPrediction = new BonusPrediction();
  secondTeamInGroupCPrediction = new BonusPrediction();

  firstTeamInGroupDPrediction = new BonusPrediction();
  secondTeamInGroupDPrediction = new BonusPrediction();

  firstTeamInGroupEPrediction = new BonusPrediction();
  secondTeamInGroupEPrediction = new BonusPrediction();

  firstTeamInGroupFPrediction = new BonusPrediction();
  secondTeamInGroupFPrediction = new BonusPrediction();

  firstTeamInGroupGPrediction = new BonusPrediction();
  secondTeamInGroupGPrediction = new BonusPrediction();

  firstTeamInGroupHPrediction = new BonusPrediction();
  secondTeamInGroupHPrediction = new BonusPrediction();

  teamsOfGroupA: Team[];
  teamsOfGroupB: Team[];
  teamsOfGroupC: Team[];
  teamsOfGroupD: Team[];
  teamsOfGroupE: Team[];
  teamsOfGroupF: Team[];
  teamsOfGroupG: Team[];
  teamsOfGroupH: Team[];

  @ViewChild('predictMatch') private predictMatchModal;
  @ViewChild('matchStats') private matchStatsModal;

  openedModal: NgbModalRef;
  config: ToasterConfig =
    new ToasterConfig({
      showCloseButton: false,
      tapToDismiss: true,
      timeout: 5000,
      animation: 'flyLeft'
    });

  constructor(private toasterService: ToasterService,private http: HttpClient, private authService: AuthService, private modalService: NgbModal) { }

  open(content, options?:any) {

    if (this.openedModal != null)
      this.openedModal.close();

    this.openedModal = this.modalService.open(content,options);

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

    this.http
      .get<BetGroup[]>('api/me/bet-groups')
      .subscribe(betGroups => {
        this.myBetGroups = betGroups;
        this.selectedBetGroup = betGroups[0];
        this.authService.setSelectedBetGroup(betGroups[0]);
        this.showBetGroupSelection = betGroups.length > 1;

        this.http.get<UserStatus>('api/me/status', { params: { BetGroupId: this.selectedBetGroup.Id.toString() } })
          .subscribe(userStatus => {
            this.userStatus = userStatus;
          });

        this.http.get<Team[]>('api/teams').subscribe(result => {

          this.teams = result;

          this.http.get<BonusPrediction[]>('api/bonus-predictions', { params: { BetGroupId: this.selectedBetGroup.Id.toString() } }).subscribe(result => {

            this.bonusPredictions = result;

            this.firstTeamPrediction = result.find(q => q.BonusPredictionType == 2);
            this.secondTeamPrediction = result.find(q => q.BonusPredictionType == 3);
            this.thirdTeamPrediction = result.find(q => q.BonusPredictionType == 4);

            this.firstTeamInGroupAPrediction = result.find(q => q.BonusPredictionType == 0 && q.WorldCupGroupName == 'Group A');
            this.secondTeamInGroupAPrediction = result.find(q => q.BonusPredictionType == 1 && q.WorldCupGroupName == 'Group A');
            this.teamsOfGroupA = this.teams.filter(q => q.WorldCupGroupName == 'Group A');

            this.firstTeamInGroupBPrediction = result.find(q => q.BonusPredictionType == 0 && q.WorldCupGroupName == 'Group B');
            this.secondTeamInGroupBPrediction = result.find(q => q.BonusPredictionType == 1 && q.WorldCupGroupName == 'Group B');
            this.teamsOfGroupB = this.teams.filter(q => q.WorldCupGroupName == 'Group B');

            this.firstTeamInGroupCPrediction = result.find(q => q.BonusPredictionType == 0 && q.WorldCupGroupName == 'Group C');
            this.secondTeamInGroupCPrediction = result.find(q => q.BonusPredictionType == 1 && q.WorldCupGroupName == 'Group C');
            this.teamsOfGroupC = this.teams.filter(q => q.WorldCupGroupName == 'Group C');

            this.firstTeamInGroupDPrediction = result.find(q => q.BonusPredictionType == 0 && q.WorldCupGroupName == 'Group D');
            this.secondTeamInGroupDPrediction = result.find(q => q.BonusPredictionType == 1 && q.WorldCupGroupName == 'Group D');
            this.teamsOfGroupD = this.teams.filter(q => q.WorldCupGroupName == 'Group D');

            this.firstTeamInGroupEPrediction = result.find(q => q.BonusPredictionType == 0 && q.WorldCupGroupName == 'Group E');
            this.secondTeamInGroupEPrediction = result.find(q => q.BonusPredictionType == 1 && q.WorldCupGroupName == 'Group E');
            this.teamsOfGroupE = this.teams.filter(q => q.WorldCupGroupName == 'Group E');

            this.firstTeamInGroupFPrediction = result.find(q => q.BonusPredictionType == 0 && q.WorldCupGroupName == 'Group F');
            this.secondTeamInGroupFPrediction = result.find(q => q.BonusPredictionType == 1 && q.WorldCupGroupName == 'Group F');
            this.teamsOfGroupF = this.teams.filter(q => q.WorldCupGroupName == 'Group F');

            this.firstTeamInGroupGPrediction = result.find(q => q.BonusPredictionType == 0 && q.WorldCupGroupName == 'Group G');
            this.secondTeamInGroupGPrediction = result.find(q => q.BonusPredictionType == 1 && q.WorldCupGroupName == 'Group G');
            this.teamsOfGroupG = this.teams.filter(q => q.WorldCupGroupName == 'Group G');

            this.firstTeamInGroupHPrediction = result.find(q => q.BonusPredictionType == 0 && q.WorldCupGroupName == 'Group H');
            this.secondTeamInGroupHPrediction = result.find(q => q.BonusPredictionType == 1 && q.WorldCupGroupName == 'Group H');
            this.teamsOfGroupH = this.teams.filter(q => q.WorldCupGroupName == 'Group H');

          });
        });

        this.http
          .get<Match[]>('api/me/matches', { params: { BetGroupId: this.selectedBetGroup.Id.toString() } })
          .subscribe(matches => {



            for (var i = 0; i < matches.length; i++) {
              matches[i].MatchUTCDateTime = moment(matches[i].MatchDateTime);
              matches[i].MatchDate = moment(matches[i].MatchDateTime).local().format('YYYY/MM/DD');
              matches[i].MatchTime = moment(matches[i].MatchDateTime).local().format('HH:mm');
              matches[i].MatchTypeDescription = (matches[i].MatchType == 0)
                ? matches[i].WorldCupGroupName
                : (matches[i].MatchType == 1)
                  ? 'Round of 16'
                  : (matches[i].MatchType == 2)
                    ? 'Quarter-final'
                    : (matches[i].MatchType == 3)
                      ? 'Semi-final'
                      : (matches[i].MatchType == 4)
                        ? 'Play-off'
                        : (matches[i].MatchType == 4) ?'Final': '';
            }

            this.matches = matches.sort((a, b) => a.MatchUTCDateTime.unix() - b.MatchUTCDateTime.unix());
            this.filteredMatches = matches;

          });
      });
  }

  filterMatchesBasedOnType(matchType: number, groupName: string): void {

    if (matchType == 0) {
      this.filteredMatches = this.matches.filter(q => q.MatchType == matchType && q.WorldCupGroupName == `Group ${groupName}`);
    }
    else {
      this.filteredMatches = this.matches.filter(q => q.MatchType == matchType);
    }

  }

  filterMatchesBasedOnPredictionType(predictionType: number|null): void {

    if (predictionType == 0) {
      this.filteredMatches = this.matches.filter(q => q.AwayTeamPredictionResult!=null);
    }
    else if (predictionType == 1) {
      this.filteredMatches = this.matches.filter(q => q.AwayTeamPredictionResult==null);
    }
    else if (predictionType == null) {
      this.filteredMatches = this.matches;
    }
  }

  onChangeBonusPrediction(bonusPrediction: BonusPrediction, teamId: number | null): void {

    if ((teamId == null && bonusPrediction.TeamId==null) || bonusPrediction.TeamId == teamId)
      return;

    this.http.post(`/api/bonus-predictions/${bonusPrediction.Id}`,
      {
        BonusPredictionId: bonusPrediction.Id,
        TeamId: teamId,
        BetGroupId: this.selectedBetGroup.Id
      })
      .subscribe(
        () => {
        let teamName = this.teams.find(q => q.TeamId == teamId).TeamName;
        bonusPrediction.TeamId = teamId;
        this.toasterService.pop('success', 'Successfully Saved', `Let's pray for ${teamName} 🤣😁✌`);
      },
      error => {
        this.toasterService.pop('error', 'I`m sorry!', `Try harder! Choose another team.`);
      });
  }
    
  openPredictDialog(match:Match): void {

    if (match.MatchHasStarted)
      return;

    this.selectedMatch = match;
    this.matchPrediction.AwayTeamPredictionResult = match.AwayTeamPredictionResult != null
      ?match.AwayTeamPredictionResult!: 0;
    this.matchPrediction.HomeTeamPredictionResult = match.HomeTeamPredictionResult
      ? match.HomeTeamPredictionResult: 0;

    if (match.MatchType != 0 && match.AwayTeamPredictionResult == match.HomeTeamPredictionResult) {
      this.matchPrediction.PenaltyWinnerTeamId =
        (match.AwayTeamPredictionPenaltyResult - match.HomeTeamPredictionPenaltyResult > 0)
        ? match.AwayTeamId : match.HomeTeamId;
    }
    else {
      this.matchPrediction.PenaltyWinnerTeamId = null;
    }
    this.open(this.predictMatchModal);

  }

  showPenaltyWinnerSelection(match: Match, matchPrediction: MatchPrediction): boolean {

    return match.MatchType != 0 && matchPrediction.HomeTeamPredictionResult!=null &&
      matchPrediction.HomeTeamPredictionResult == matchPrediction.AwayTeamPredictionResult;
  }

  saveMatchPrediction(): void {

    var prediction = {
      MatchId: this.selectedMatch.Id,
      HomeMatchResult: this.matchPrediction.HomeTeamPredictionResult,
      AwayMatchResult: this.matchPrediction.AwayTeamPredictionResult,
      BetGroupId: this.selectedBetGroup.Id,
      PenaltyWinnerTeamId:(this.matchPrediction.HomeTeamPredictionResult == this.matchPrediction.AwayTeamPredictionResult)
        ? this.matchPrediction.PenaltyWinnerTeamId
        : null
    };

    this.http.post(`/api/match/${this.selectedMatch.Id}/prediction`, prediction)
      .subscribe(() => {
        this.selectedMatch.AwayTeamPredictionResult = this.matchPrediction.AwayTeamPredictionResult;
        this.selectedMatch.HomeTeamPredictionResult = this.matchPrediction.HomeTeamPredictionResult;
        this.toasterService.pop('success', 'Successfully Saved', `Sit relax and hope for a bit of luck 🤑🤑🤑`);
      });

    this.openedModal.close();

  }

  showEditPrediction(match: Match): boolean {

    return !match.MatchHasStarted && (match.HomeTeamPredictionResult != null);

  }

  showNewPrediction(match: Match): boolean {

    return !match.MatchHasStarted && (match.HomeTeamPredictionResult == null);

  }

  openMatchStatsDialog(match: Match): void {

    if (!match.MatchHasStarted)
      return;

    this.selectedMatch = match;

    this.http.get<MatchStat[]>(`/api/match/${match.Id}/stats`, {
      params: {
        MatchId: this.selectedMatch.Id.toString(),
        BetGroupId: this.selectedBetGroup.Id.toString()
      }
    }).subscribe(result => {
      this.selectedMatchStats = result;
      this.open(this.matchStatsModal, { size: 'lg' });
    });
  }

}

