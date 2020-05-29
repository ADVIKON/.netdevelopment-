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
// this.IsAdvikon= true;
//   this.VideoLink0="https://bit.ly/3c3P3GM";
//   this.VideoLink90="https://bit.ly/2TKqbO3"; 
//   this.AudioLink0="https://bit.ly/2XDJj13"; 
//   this.AudioLink90="https://bit.ly/2B0Awij"; 

//   this.WindowSFLink="https://bit.ly/3gtO8mk";
//   this.WindowVideoLink="https://bit.ly/2TLFzcG";
//   this.CopyrightLink="https://bit.ly/3d9RpW1";
//   this.CopyleftLink="https://bit.ly/2BanHly";
//   this.StreamLink="https://bit.ly/2AhYhSm";
//==============================================================================
//==============================================================================
    //================================== Nusign ==========================
     
    this.IsAdvikon= false;

    this.VideoLink0="http://shorturl.at/dgNU5";
    this.VideoLink90="http://shorturl.at/hjmT7"; 
    this.AudioLink0="http://shorturl.at/myQV6"; 
    this.AudioLink90="Not available";
    this.WindowSFLink="http://shorturl.at/sCHZ4";
    this.WindowVideoLink="http://shorturl.at/wCIUV";
    this.CopyrightLink="http://shorturl.at/wxJQZ";
    this.CopyleftLink="http://shorturl.at/boLSW";
    this.StreamLink="http://shorturl.at/evxCH";

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
