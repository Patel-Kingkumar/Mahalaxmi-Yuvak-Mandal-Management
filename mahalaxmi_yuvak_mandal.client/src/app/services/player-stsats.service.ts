import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { API_ENDPOINTS } from '../core/api-endpoints';
import { environment } from '../../environment';

@Injectable({
  providedIn: 'root'
})
export class PlayerStsatsService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createPlayerStats(data: any) {
    return this.http.post(
      `${this.baseUrl}/${API_ENDPOINTS.PLAYER_STATS.BASE}/${API_ENDPOINTS.PLAYER_STATS.CREATE}`,
      data
    );
  }

  getAllPlayerStats() {
    return this.http.get(
      `${this.baseUrl}/${API_ENDPOINTS.PLAYER_STATS.BASE}/${API_ENDPOINTS.PLAYER_STATS.GET_ALL}`
    );
  }
}
