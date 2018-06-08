export class Match{

  Id : number;
  HomeTeamName: string;
  AwayTeamName: string;
  HomeTeamId: number;
  AwayTeamId: number;
  MatchDateTime: Date;
  MatchDate: string;
  MatchTime: string;
  HomeTeamResult: number | null;
  AwayTeamResult: number | null;
  HomeTeamPenaltyResult: number | null;
  AwayTeamPenaltyResult: number | null;
  HomeTeamPredictionResult: number | null;
  AwayTeamPredictionResult: number | null;
  HomeTeamPredictionPenaltyResult: number | null;
  AwayTeamPredictionPenaltyResult: number | null;
  CityName: string;
  StadiumName: string;
  StadiumImageUrl: string;
  MatchHasStarted: boolean;
  MatchType: number;
  MatchTypeDescription: string;
  Score: number;
  WorldCupGroupName: string;
}
