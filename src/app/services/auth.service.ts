import { Injectable } from '@angular/core';
import { User } from '../interfaces/user.interface';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  login(user: User) {
    return this.http.post(environment.userManagement.baseUrl + 'account/login', user);
  }

  register(user: User) {
    return this.http.post(environment.userManagement.baseUrl + 'account/register', user);
  }

  storeToken(tokenVal: string) {
    localStorage.setItem('token', tokenVal);
  }

  storeUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
  }
}
