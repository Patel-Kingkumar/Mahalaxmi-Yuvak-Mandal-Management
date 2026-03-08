import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environment';
import { API_ENDPOINTS } from '../core/api-endpoints';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MatchService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Create Match
  createMatch(data: any) {
    return this.http.post(
      `${this.baseUrl}/${API_ENDPOINTS.MATCHES.BASE}/${API_ENDPOINTS.MATCHES.CREATE}`,
      data
    );
  }

  // Get All Matches
  getAllMatches() {
    return this.http.get(
      `${this.baseUrl}/${API_ENDPOINTS.MATCHES.BASE}/${API_ENDPOINTS.MATCHES.GET_ALL}`
    );
  }

  // Get Match By Id
  getMatchById(id: number) {
    return this.http.get(
      `${this.baseUrl}/${API_ENDPOINTS.MATCHES.BASE}/${API_ENDPOINTS.MATCHES.GET_BY_ID}/${id}`
    );
  }

  getScores(matchId: number) {
    return this.http.get<any>(
      `${this.baseUrl}/${API_ENDPOINTS.MATCH_SCORE.BASE}/${API_ENDPOINTS.MATCH_SCORE.GET_BY_MATCH}/${matchId}`
    );
  }

  downloadMatchesPdf(): Observable<Blob> {
    return this.http.get(
      `${this.baseUrl}/${API_ENDPOINTS.MATCHES.BASE}/${API_ENDPOINTS.MATCHES.DOWNLOAD_PDF}`,
      { responseType: 'blob' } // Critical for PDF files
    );
  }

  createScore(data: any) {
    return this.http.post<any>(
      `${this.baseUrl}/${API_ENDPOINTS.MATCH_SCORE.BASE}/${API_ENDPOINTS.MATCH_SCORE.CREATE}`,
      data
    );
  }

  // Update Match
  updateMatch(id: number, data: any) {
    return this.http.put(
      `${this.baseUrl}/${API_ENDPOINTS.MATCHES.BASE}/${API_ENDPOINTS.MATCHES.UPDATE}/${id}`,
      data
    );
  }

  // Delete Match
  deleteMatch(id: number) {
    return this.http.delete(
      `${this.baseUrl}/${API_ENDPOINTS.MATCHES.BASE}/${API_ENDPOINTS.MATCHES.DELETE}/${id}`
    );
  }

}
