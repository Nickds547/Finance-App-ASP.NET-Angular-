import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';
import {TOKEN_NAME} from '../imports/app.constants';

@Injectable({
  providedIn: 'root'
})
export class JwtauthService {

  constructor() { }

  private decodeToken(token: string){
    return jwtDecode(token);
  }

   getTokenExpiration(token: string): Date{

    const decoded: any = this.decodeToken(token);

    if(decoded.exp == null) return null;

    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }

   authenticateUser = (token: string): boolean =>{
    
    if(this.isTokenExpired(token))
      return false;
    else{
      localStorage.setItem(TOKEN_NAME, token);
      return true;
    }
  }

  isTokenExpired(token? : string): boolean{

    if(!token) token = this.getToken();
    if(!token) return true;

    const date = this.getTokenExpiration(token);

    if(date === undefined) return false;
    return !(date.valueOf() > new Date().valueOf())
  }

  getToken(){
    return localStorage.getItem(TOKEN_NAME);
  }

}
