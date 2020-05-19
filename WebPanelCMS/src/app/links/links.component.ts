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
  IsAdvikon:boolean= true;

VideoLink0="";
VideoLink90="";
AudioLink0="";
AudioLink90="";
WindowSFLink="";
WindowVideoLink="";
CopyrightLink="";
CopyleftLink="";
StreamLink="";
  constructor(private serviceLicense: SerLicenseHolderService, public toastr: ToastrService, vcr: ViewContainerRef) {
  }

  ngOnInit() {
    //================================== Advikon ==========================
this.IsAdvikon= true;
  this.VideoLink0="http://shorturl.at/fvEG5";
  this.VideoLink90="http://shorturl.at/cjmqE"; 
  this.AudioLink0="http://shorturl.at/DHN37"; 
  this.AudioLink90="http://shorturl.at/KSZ57"; 
  this.WindowSFLink="http://shorturl.at/wzDIU";
  this.WindowVideoLink="http://shorturl.at/ktER3";
  this.CopyrightLink="http://shorturl.at/puJX0";
  this.CopyleftLink="http://shorturl.at/wyPUY";
  this.StreamLink="http://shorturl.at/cjvQU";
//==============================================================================
//==============================================================================
    //================================== Nusign ==========================
     
    //this.IsAdvikon= false;
    // this.VideoLink0="http://shorturl.at/dgNU5";
    // this.VideoLink90="http://shorturl.at/hjmT7"; 
    // this.AudioLink="http://shorturl.at/myQV6"; 
    // this.WindowSFLink="http://shorturl.at/sCHZ4";
    // this.WindowVideoLink="http://shorturl.at/wCIUV";
    // this.CopyrightLink="http://shorturl.at/wxJQZ";
    // this.CopyleftLink="http://shorturl.at/boLSW";
    // this.StreamLink="http://shorturl.at/evxCH";

    if (localStorage.getItem('dfClientId') == "6") {
      this.IsAdminLogin = true;
      this.FillClientList();
    }
    else {
       this.IsAdminLogin = false;
    }
     
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
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
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
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
}
