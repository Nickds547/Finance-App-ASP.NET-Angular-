import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';

import {JwtauthService} from '../services/jwtauth.service'


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private jwtService: JwtauthService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      
      var isAuthenticated: boolean = false;
      const token = this.jwtService.getToken();

      if(token == undefined || token == null || this.jwtService.isTokenExpired(token))
      {
        isAuthenticated = false;
        this.router.navigate(['login']);
      }
      else{
        isAuthenticated = true;
      } 

      return isAuthenticated;
  }
  
}
