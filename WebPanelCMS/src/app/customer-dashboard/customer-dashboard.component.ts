import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { DashboardService } from '../customer-dashboard/dashboard.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-customer-dashboard',
  templateUrl: './customer-dashboard.component.html',
  styleUrls: ['./customer-dashboard.component.css']
})
export class CustomerDashboardComponent implements OnInit {
  TokenList = [];
  page: number = 1;
  pageSize: number = 20;
  loading: boolean;
  TotalPlayers = 0;
  OnlinePlayers = 0; 
  OfflinePlayer = 0;
  PlayerFillType = "Total Players";
  TokenInfo;
  searchText;
  IsAdminLogin: boolean = false;
  CustomerList = [];
 

  constructor(public toastr: ToastrService, vcr: ViewContainerRef, private dService: DashboardService,
    config: NgbModalConfig, private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
    console.log("Dashboard");
  }

  ngOnInit() {
     
    this.TokenList = [{}];
    
    if (localStorage.getItem('dfClientId') == "6") {
      this.IsAdminLogin = true;
      this.FillClientList();
    }
    else  if (localStorage.getItem('dfClientId') == "71") {
      this.IsAdminLogin = true;
      this.FillSubClientList();
    }
    else {
      this.IsAdminLogin = false;
      this.GetCustomerTokenDetail('Total', localStorage.getItem('dfClientId'));
    } 
  }
  FillSubClientList(){
    var q = "";
    q = "select DFClientID as id,ClientName as displayname from ( ";
    q = q + " select distinct DFClients.DFClientID,DFClients.ClientName from DFClients ";
    q = q + " inner join AMPlayerTokens on DFClients.DfClientid=AMPlayerTokens.Clientid ";
    q = q + " where DFClients.CountryCode is not null and DFClients.DealerDFClientID= " + localStorage.getItem('dfClientId') + "    ";
    q = q + " union all select distinct DFClients.DFClientID,DFClients.ClientName from DFClients ";
    q = q + " inner join AMPlayerTokens on DFClients.DfClientid=AMPlayerTokens.Clientid ";
    q = q + " where DFClients.CountryCode is not null and DFClients.MainDealerid= " + localStorage.getItem('dfClientId') + "    ";
    q = q + "   ) as a order by ClientName desc ";
    this.loading = true;
    this.dService.FillCombo(q).pipe()
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
  FillClientList() {
    this.loading = true;
    var str = "";
    str = "select DFClientID as id,  ClientName as displayname from DFClients where CountryCode is not null and DFClients.IsDealer=1 order by RIGHT(ClientName, LEN(ClientName) - 3)";
    this.dService.FillCombo(str).pipe()
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
  GetCustomerTokenDetailFilter(filter) {
    if (filter == "Total") {
      this.searchText = "";
      this.PlayerFillType = filter + " Players";
    }
    else {
      this.searchText = filter;
      if (filter == "Away") {
        this.PlayerFillType = "Offline Players";
      }
      else {
        this.PlayerFillType = filter + " Players";
      }
    }
  }
  cmbCustomerId;
  onChangeCustomer(deviceValue) {
    this.cmbCustomerId= deviceValue;
    this.GetCustomerTokenDetail('Total', deviceValue);
  }
  RefershClick() {
var cid;
if (this.IsAdminLogin==true){
cid=this.cmbCustomerId;
}
else{
cid=localStorage.getItem('dfClientId');
}
    this.GetCustomerTokenDetail('Total',cid);
  }
  GetCustomerTokenDetail(type, cid) {
    this.PlayerFillType = type + " Players";
    
    if (this.IsAdminLogin==true){
      cid=this.cmbCustomerId;
      }
      else{
      cid=localStorage.getItem('dfClientId');
      }
    this.loading = true;
    this.dService.GetCustomerTokenDetailSummary(type, cid).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        
        if (obj.TotalPlayers!='0'){
        this.TotalPlayers = obj.TotalPlayers;
        this.OnlinePlayers = obj.OnlinePlayers;
        this.OfflinePlayer = obj.OfflinePlayer;
        this.TokenList = obj.lstToken;
        this.loading = false;
        this.GetCustomerTokenDetailFilter('Total');
        }
        else{
          this.loading = false;
        }
        this.loading = false;
        
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  openModal(content, tid, location, city) {
    this.TokenInfo = tid + "-" + location + "-" + city;
    localStorage.setItem("tokenid", tid);
    this.modalService.open(content, { size: 'lg' });
    
  }
}
