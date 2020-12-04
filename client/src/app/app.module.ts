import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http'
import { ReactiveFormsModule } from '@angular/forms'
import { MDBBootstrapModule, ChartsModule } from 'angular-bootstrap-md';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { NavbarComponent } from './navbar/navbar.component';
import { TransactionsComponent } from './transactions/transactions.component';
import { ViewTransactionsComponent } from './view-transactions/view-transactions.component';
import {JwtTokenInterceptor} from './interceptors/jwtToken.interceptor'

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    RegisterComponent,
    NavbarComponent,
    TransactionsComponent,
    ViewTransactionsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    MDBBootstrapModule.forRoot(),
    ChartsModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtTokenInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
