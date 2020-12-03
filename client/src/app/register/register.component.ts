import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import {AuthService} from '../services/auth.service';
import {errorObject} from '../imports/app.models';
import { User } from '../imports/server.models';
import {JwtauthService} from '../services/jwtauth.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  email: FormControl;
  name: FormControl;
  password: FormControl;
  confirmPass: FormControl;

  error: errorObject = null;
  
  constructor(private authService:AuthService, private jwtService: JwtauthService, private router: Router) { }

  ngOnInit(): void {
    this.createFormControls();
    this.createForm();
  }

  createFormControls(){
    this.email = new FormControl('', [Validators.required, this.isEmail]);
    this.name = new FormControl('', Validators.required);
    this.password = new FormControl('', [Validators.required]);
    this.confirmPass = new FormControl('', [Validators.required, this.passwordMatch]);
  }

  createForm(){
    this.registerForm = new FormGroup({
      email: this.email,
      name: this.name,
      password: this.password,
      confirmPass: this.confirmPass
    })
  }

  registerUser(){

    let user: User = new User(this.email.value);
    user.Name = this.name.value;
    user.Password = this.password.value;

    this.authService.createUser(user).subscribe(
      data =>{
        console.log(data);
        this.jwtService.authenticateUser(data.accessToken);
        this.authService.setUser(data.user);
        this.router.navigate(['']);
      },
      error =>{
        console.log(error);
      }
    );

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

  passwordMatch = (control: FormControl) =>{
   let confirmPass = control.value;

   if(this.password.value !== undefined && confirmPass !== undefined && this.password.value){
    
    if(this.password.value === confirmPass)
      return null;
   }

  
   return confirmPass;

  }


}
