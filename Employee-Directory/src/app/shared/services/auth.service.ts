import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environments';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserModel } from '../../models/user/user.model';
import { LoginModel } from '../../models/login/login.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  private apiUrl = environment.apiUrl;

  constructor(
    private http: HttpClient
  ){}

  login(data: LoginModel): Observable<void>{

    return this.http.post<void>(
      `${this.apiUrl}/Auth/login`,
      data,
      {
        withCredentials:true
      }
    );

  }

  getCurrentUser():Observable<UserModel>{

    return this.http.get<UserModel>(
      `${this.apiUrl}/Auth/currentUser`,
      {
        withCredentials:true
      }
    );

  }

  logout(){

    return this.http.post(
      `${this.apiUrl}/Auth/logout`,
      {},
      {
        withCredentials:true
      }
    );
  }

}