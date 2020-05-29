import { Component,OnInit } from '@angular/core';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit{
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
  public isCollapsed = true;
  ngOnInit() {
    this.isCollapsed=true;
  }
}
