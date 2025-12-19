import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserResponseModel } from './models/user-response-model';
import { ChangeActivationRequestModel } from './models/change-activation-request-model';
import { CreateUserRequestModel } from './models/create-user-request-model';
import { UpdatePasswordRequestModel } from './models/update-password-request-model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = '/api/user';

  constructor(private http: HttpClient) { }

  getUsers(): Observable<UserResponseModel[]> {
    return this.http.get<UserResponseModel[]>(`${this.apiUrl}`);
  }
  
  createUser(model: CreateUserRequestModel) {
    return this.http.post(`${this.apiUrl}`, model);
  }

  updatePassword(model: UpdatePasswordRequestModel) {
    return this.http.put(`${this.apiUrl}`, model);
  }

  changeAccess(userId: string, model: ChangeActivationRequestModel) {
    return this.http.put(`${this.apiUrl}/${userId}`, model);
  }
}
