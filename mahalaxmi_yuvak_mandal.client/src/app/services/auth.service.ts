import { Injectable } from '@angular/core';
import { environment } from '../../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = `${environment.apiUrl}/Users`;
  private baseUrlAuth = `${environment.apiUrl}/Auth`;

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
}
