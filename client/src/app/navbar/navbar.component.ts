import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router"

import {AuthService} from '../services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(public router: Router, private authService: AuthService) { }

  ngOnInit(): void {
  }

  logout(){
    if(confirm("Are you sure to logout? ")) {
      this.authService.logout();
      this.router.navigate(['login'])
    }
  }

}
