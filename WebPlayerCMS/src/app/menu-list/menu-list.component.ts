import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-menu-list',
  templateUrl: './menu-list.component.html',
  styleUrls: ['./menu-list.component.css']
})
export class MenuListComponent implements OnInit {

  constructor() { }
IsAdminLogin:boolean=false;
UserId;
chkDashboard;
chkPlayerDetail;
chkPlaylistLibrary;
chkScheduling;
chkAdvertisement;
chkInstantPlay;
  ngOnInit() {
    if ((localStorage.getItem('dfClientId') == "6") ||(localStorage.getItem('dfClientId') == "2")) {
      this.IsAdminLogin = true;
    }
    else {
      this.IsAdminLogin = false;
    }
       
      this.UserId= localStorage.getItem('UserId');
      this.chkDashboard=localStorage.getItem('chkDashboard');
      this.chkPlayerDetail=localStorage.getItem('chkPlayerDetail');
      this.chkPlaylistLibrary=localStorage.getItem('chkPlaylistLibrary');
      this.chkScheduling=localStorage.getItem('chkScheduling');
      this.chkAdvertisement=localStorage.getItem('chkAdvertisement');
      this.chkInstantPlay=localStorage.getItem('chkInstantPlay');
 
            
            
            
  }

}
