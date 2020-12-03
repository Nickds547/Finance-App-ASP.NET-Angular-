import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';

import {LINK,HEADERS} from '../imports/server.config';
import {User} from '../imports/server.models'
import {USER_STORAGE, TOKEN_NAME} from '../imports/app.constants'
import {JwtauthService} from './jwtauth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private jwtService: JwtauthService) { }

  login = (email: string, password: string): Observable<any> => {

    let user = new User(email);
    user.Password = password;
    user.Role = "user";

    let body = JSON.stringify(user);

    return this.http.post(LINK + "User/login", body, {'headers': HEADERS})

  }

  createUser = (user: User): Observable<any> =>{

    user.Role = "user";
    let body = JSON.stringify(user);

    return this.http.post(LINK + "User", body, {'headers': HEADERS})
  }

  setUser = (user: User) =>{
    localStorage.setItem(USER_STORAGE, JSON.stringify(user));
  }

  getUser = (): User =>{
     let storedData = localStorage.getItem(USER_STORAGE);

     if(storedData === null || storedData === undefined)
      return null

      let userData = JSON.parse(storedData);
      let user: User = new User(userData.name)
      user.Id = userData.id;
      user.Email = userData.email;
      user.Role = userData.role;

      return user;
  }

  logout = () =>{
    localStorage.removeItem(USER_STORAGE);
    localStorage.removeItem(TOKEN_NAME);
  }

  isLoggedIn(){
   return !this.jwtService.isTokenExpired();
  }



}
