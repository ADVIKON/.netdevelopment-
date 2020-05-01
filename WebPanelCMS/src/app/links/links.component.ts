import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { SerLicenseHolderService } from '../license-holder/ser-license-holder.service';
import { ToastrService } from 'ngx-toastr';



@Component({
  selector: 'app-links',
  templateUrl: './links.component.html',
  styleUrls: ['./links.component.css']
})
export class LinksComponent implements OnInit {

  CustomerList: any[];
  lName;
  lPwd;
  public loading = false;
  IsAdminLogin: boolean = true;
  UserId;
chkDashboard;
chkPlayerDetail;
chkPlaylistLibrary;
chkScheduling;
chkAdvertisement;
chkInstantPlay;

VideoLink0="";
VideoLink90="";
AudioLink="";
WindowSFLink="";
WindowVideoLink="";
CopyrightLink="";
CopyleftLink="";

  constructor(private serviceLicense: SerLicenseHolderService, public toastr: ToastrService, vcr: ViewContainerRef) {
  }

  ngOnInit() {
    //================================== Advikon ==========================

  // this.VideoLink0="http://shorturl.at/fvEG5";
  // this.VideoLink90="http://shorturl.at/cjmqE"; 
  // this.AudioLink="http://shorturl.at/fvEG5"; 
  // this.WindowSFLink="https://api.advikon.com:4437/app/StoreAndForwardPlayer.zip";
  // this.WindowVideoLink="https://api.advikon.com:4437/app/VideoPlayer.zip";
  // this.CopyrightLink="https://api.advikon.com:4437/app/copyright.zip";
  // this.CopyleftLink="https://api.advikon.com:4437/app/copyleft.zip";
//==============================================================================
//==============================================================================
    //================================== Nusign ==========================
     
    this.VideoLink0="http://shorturl.at/dgNU5";
    this.VideoLink90="http://shorturl.at/hjmT7"; 
    this.AudioLink="http://shorturl.at/myQV6"; 
    this.WindowSFLink="https://api.nusign.eu:4477/app/StoreAndForwardPlayer.zip";
    this.WindowVideoLink="https://api.nusign.eu:4477/app/VideoPlayer.zip";
    this.CopyrightLink="https://api.nusign.eu:4477/app/copyright.zip";
    this.CopyleftLink="https://api.nusign.eu:4477/app/copyleft.zip";
  

    if (localStorage.getItem('dfClientId') == "6") {
      this.IsAdminLogin = true;
      this.FillClientList();
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
  FillClientList() {
    this.loading = true;
    var str = "";
    str = "select DFClientID as id,  ClientName    as displayname from DFClients where CountryCode is not null and DFClients.IsDealer=1 order by RIGHT(ClientName, LEN(ClientName) - 3)";
    this.serviceLicense.FillCombo(str).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.CustomerList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onChangeCustomer(deviceValue) {
    this.loading = true;
    this.serviceLicense.CustomerLoginDetail(deviceValue).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        
        var obj = JSON.parse(returnData);
        this.lName= obj.LoginName;
        this.lPwd= obj.LoginPassword;
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
}
