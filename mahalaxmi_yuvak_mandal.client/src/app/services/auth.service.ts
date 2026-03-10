import { Injectable } from '@angular/core';
import { environment } from '../../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../core/api-endpoints';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = `${environment.apiUrl}/Users`;
  private baseUrlAuth = `${environment.apiUrl}/Auth`;

  private baseUrltWO = environment.apiUrl;

  constructor(private http: HttpClient) { }

  login(request: any): Observable<any> {
    return this.http.post(`${this.baseUrlAuth}/login`, request, { withCredentials: true });
  }

  sendOTP(email: string): Observable<any> {
    return this.http.post(`${this.baseUrlAuth}/send-otp`, { email });
  }

  resetPassword(request: any): Observable<any> {
    return this.http.post(`${this.baseUrlAuth}/reset-password`, request);
  }

  downloadAdminsPdf(): Observable<Blob> {
    return this.http.get(
      `${this.baseUrltWO}/${API_ENDPOINTS.AUTH.BASE}/${API_ENDPOINTS.AUTH.DOWNLOAD_PDF}`,
      { responseType: 'blob' } // Critical for PDF files
    );
  }

  logout() {
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('userId');
    sessionStorage.removeItem('role');
  }

}
