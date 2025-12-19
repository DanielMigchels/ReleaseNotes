import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginRequestModel } from './models/login-request-model';
import { RegisterRequestModel } from './models/register-request-model';
import { RegisterResponseModel } from './models/register-response-model';
import { LoginResponseModel } from './models/login-response-model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private apiUrl = '/api/authentication';

  constructor(private http: HttpClient, private router: Router) { }

  login(loginRequestModel: LoginRequestModel): Observable<LoginResponseModel> {
    return this.http.post<LoginResponseModel>(`${this.apiUrl}/login`, loginRequestModel);
  }

  register(registerRequestModel: RegisterRequestModel): Observable<RegisterResponseModel> {
    return this.http.post<RegisterResponseModel>(`${this.apiUrl}/register`, registerRequestModel);
  }

  setJwt(jwt: string) {
    localStorage.setItem('jwt', jwt);
  }

  getJwt(): String | null {
    return localStorage.getItem('jwt');
  }

  isLoggedIn(): boolean {
    return this.getJwt() !== null;
  }

  logout() {
    localStorage.removeItem('jwt');
    this.router.navigate(['/login']);
  }
}
