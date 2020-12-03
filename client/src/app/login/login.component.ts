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
        console.log(typeof(err.error));
        console.log(typeof(err.error) != typeof(String))
        
        if(typeof(err.error) != typeof(String))
          err.error = "An unexpected error occurred, please try again later"

        this.messages.push(new errorObject(this.messages.length,false,err.error));
      }
      )
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

  switchRegister(){
    this.register = !this.register;
  }
}
