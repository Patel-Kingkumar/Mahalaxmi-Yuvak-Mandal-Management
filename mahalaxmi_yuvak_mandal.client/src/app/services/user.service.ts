import { Injectable } from '@angular/core';
import { environment } from '../../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../core/api-endpoints';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllUsers(): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/${API_ENDPOINTS.USERS.BASE}/${API_ENDPOINTS.USERS.GET_ALL}`
    );
  }

  downloadUsersPdf(): Observable<Blob> {
    return this.http.get(
      `${this.baseUrl}/${API_ENDPOINTS.USERS.BASE}/${API_ENDPOINTS.USERS.DOWNLOAD_PDF}`,
      { responseType: 'blob' } // Critical for PDF files
    );
  }

  createUser(user: any): Observable<any> {
    return this.http.post(
      `${this.baseUrl}/${API_ENDPOINTS.USERS.BASE}`,
      user
    );
  }

  deleteUser(id: number) {
    return this.http.delete(
      `${this.baseUrl}/${API_ENDPOINTS.USERS.BASE}/${id}`
    );
  }

  getUserById(id: number) {
    return this.http.get<any>(
      `${this.baseUrl}/${API_ENDPOINTS.USERS.BASE}/${id}`
    );
  }


  updateUser(id: number, user: any) {
    return this.http.put(
      `${this.baseUrl}/${API_ENDPOINTS.USERS.BASE}/${id}`,
      user
    );
  }
}
