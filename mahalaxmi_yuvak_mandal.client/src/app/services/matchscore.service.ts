import { Injectable } from '@angular/core';
import { environment } from '../../environment';
import { HttpClient } from '@angular/common/http';
import { API_ENDPOINTS } from '../core/api-endpoints';

@Injectable({
  providedIn: 'root'
})
export class MatchscoreService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createScore(data: any) {
    return this.http.post(
      `${this.baseUrl}/${API_ENDPOINTS.MATCH_SCORE.BASE}/${API_ENDPOINTS.MATCH_SCORE.CREATE}`,
      data
    );
  }

  getScores(matchId: number) {
    return this.http.get(
      `${this.baseUrl}/${API_ENDPOINTS.MATCH_SCORE.BASE}/${API_ENDPOINTS.MATCH_SCORE.GET_BY_MATCH}/${matchId}`
    );
  }
}
