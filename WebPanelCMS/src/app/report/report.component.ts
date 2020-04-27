import { Component, OnInit } from '@angular/core';
@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css'],
})
export class ReportComponent implements OnInit {
  UserId;
chkDashboard;
chkPlayerDetail;
chkPlaylistLibrary;
chkScheduling;
chkAdvertisement;
chkInstantPlay;
IsAdminLogin:boolean= false;
  constructor(){}
  ngOnInit() {
    if ((localStorage.getItem('dfClientId') == "2") || (localStorage.getItem('dfClientId') == "6")) {
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
/*
"./node_modules/jszip/dist/jszip.js",
"./node_modules/datatables.net-buttons/js/dataTables.buttons.js",
"./node_modules/datatables.net-buttons/js/buttons.colVis.js",
"./node_modules/datatables.net-buttons/js/buttons.flash.js",
"./node_modules/datatables.net-buttons/js/buttons.html5.js",
"./node_modules/datatables.net-buttons/js/buttons.print.js",

*/