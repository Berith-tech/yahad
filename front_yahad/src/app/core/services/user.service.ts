import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserService {
  private http = inject(HttpClient);

  getProfile(): Observable<any> {
    return this.http.get<any>('/api/users/profile');
  }

  updateProfile(data: any): Observable<any> {
    return this.http.put<any>('/api/users/profile', data);
  }
}
