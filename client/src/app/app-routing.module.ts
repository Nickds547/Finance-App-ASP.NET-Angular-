import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import * as Components from './imports/components';
import {AuthGuard} from './guards/auth.guard'

const routes: Routes = [
  {path: "" ,component: Components.HomeComponent, canActivate: [AuthGuard]},
  {path: "login", component: Components.LoginComponent},
 

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
