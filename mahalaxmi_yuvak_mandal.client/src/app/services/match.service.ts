import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environment';
import { API_ENDPOINTS } from '../core/api-endpoints';

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
