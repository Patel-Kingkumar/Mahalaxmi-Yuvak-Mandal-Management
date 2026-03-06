import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environment';
import { API_ENDPOINTS } from '../core/api-endpoints';

@Injectable({
  providedIn: 'root'
})
export class DonationService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createDonation(data: any): Observable<any> {
    return this.http.post(
      `${this.baseUrl}/${API_ENDPOINTS.DONATIONS.BASE}/${API_ENDPOINTS.DONATIONS.CREATE}`,
      data
    );
  }

  // getDonations(): Observable<any[]> {
  //   return this.http.get<any[]>(
  //     `${this.baseUrl}/${API_ENDPOINTS.DONATIONS.BASE}/${API_ENDPOINTS.DONATIONS.GET_ALL}`
  //   );
  // }

  getDonations(userId?: number): Observable<any[]> {

    let url = `${this.baseUrl}/${API_ENDPOINTS.DONATIONS.BASE}/${API_ENDPOINTS.DONATIONS.GET_ALL}`;

    if (userId) {
      url += `?userId=${userId}`;
    }

    return this.http.get<any[]>(url);

  }


  getDonationById(id: number) {
    return this.http.get<any>(
      `${this.baseUrl}/${API_ENDPOINTS.DONATIONS.BASE}/${API_ENDPOINTS.DONATIONS.GET_BY_ID}/${id}`
    );
  }

  updateDonation(id: number, data: any) {
    return this.http.put(
      `${this.baseUrl}/${API_ENDPOINTS.DONATIONS.BASE}/${API_ENDPOINTS.DONATIONS.UPDATE}/${id}`,
      data
    );
  }

  deleteDonation(id: number) {
    return this.http.delete(
      `${this.baseUrl}/${API_ENDPOINTS.DONATIONS.BASE}/${API_ENDPOINTS.DONATIONS.DELETE}/${id}`
    );
  }

}
