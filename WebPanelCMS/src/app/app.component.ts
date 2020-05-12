import { Component } from '@angular/core';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  constructor(public authService: AuthService) {}
  IsAdminLogin:boolean=true;
  UserId=0;
  chkDashboard='true';
  chkPlayerDetail='true';
  chkPlaylistLibrary='true';
  chkScheduling  ='true';
  chkAdvertisement='true';
  chkInstantPlay='true';
  title = 'WebPanelCMS';
}
