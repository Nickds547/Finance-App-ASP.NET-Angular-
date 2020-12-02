import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  checkToken(){
    console.log("token: " + localStorage.getItem("id_token"));
    console.log("User: " +localStorage.getItem("user"));
  }
}
