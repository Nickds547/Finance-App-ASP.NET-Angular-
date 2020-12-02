import { Component, OnInit } from '@angular/core';
import { NgForm, NgModel} from '@angular/forms';
import {FormControl,Validator,Validators,FormGroup} from '@angular/forms'

import {Router} from '@angular/router';
import { User } from '../imports/server.models';
import {AuthService} from '../services/auth.service'
import { JwtauthService } from '../services/jwtauth.service';
import {errorObject} from '../imports/app.models'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router, private jwtService: JwtauthService) { }


  loginForm: FormGroup;
  email: FormControl;
  password: FormControl;

  register: boolean;
  messages: Array<errorObject> = [];

  ngOnInit(): void {
    this.register = false;
    this.createFormControls();
    this.createForm();
  }

  createFormControls(){
    this.email = new FormControl('', [Validators.required, this.isEmail]);
    this.password = new FormControl('', Validators.required)
  }

  createForm(){
    this.loginForm = new FormGroup({
      email: this.email,
      password: this.password
    })
  }

  login(){

    this.authService.login(this.email.value, this.password.value)
      .subscribe(
        data =>{
        console.log('data: ' , data);
        this.jwtService.authenticateUser(data.accessToken);
        this.authService.setUser(data.user);
        this.router.navigate(['']);
      },
      err =>{
        console.log("error: ", err.error,);
        this.messages.push(new errorObject(this.messages.length,false,err.error));
      }
      )
  }

  private authenticateUser(data: any){
    var isAuthenticated = this.jwtService.authenticateUser(data.accessToken);

    if(isAuthenticated)
    {
      let userData = data.user;
      let user = new User(userData.name)

      user.Email = userData.email;
      user.Id =  parseInt(userData.id);
      user.Name = userData.name;
      user.Role = userData.role;

      this.authService.setUser(user);
      this.router.navigate(['']);
    }
    else{
      this.messages.push(new errorObject(this.messages.length,false,"Error: Please try again"));
    }
    
  }

  removeErrorObject(id: number){

    let newMessages: Array<errorObject> = [];
    
    console.log('Item to be removed' ,this.messages[id])
    for(let i = 0; i < this.messages.length; i++){
      if(i !== id)
        newMessages.push(this.messages[i])
    }

     this.messages = newMessages;
     console.log('Updated message: ', this.messages)
  }

   isEmail = (control: FormControl) =>{

    let email = control.value;
    if(email && email.indexOf("@") && email.indexOf(".") != -1){
      if(email.indexOf("@") !=-1) //For some reason this had to be added twice
      {
        return null;
      }
    }
    return email;
  }
}
