import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environment';
import { API_ENDPOINTS } from '../core/api-endpoints';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getDashboardStats(): Observable<any> {
    return this.http.get(
      `${this.baseUrl}/${API_ENDPOINTS.DASHBOARD.BASE}/${API_ENDPOINTS.DASHBOARD.GET_STATS}`
    );
  }
}
