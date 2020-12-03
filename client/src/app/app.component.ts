import { Component } from '@angular/core';

import {LoadingService} from './services/loading.service'
import {AuthService} from './services/auth.service'
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'client';

  constructor(public loadingService: LoadingService, public authService: AuthService){}

  
}
