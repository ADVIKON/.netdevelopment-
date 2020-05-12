import { Component, OnInit,ViewContainerRef } from '@angular/core';
import { SerAdminLogService } from '../admin-logs/ser-admin-log.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-admin-logs',
  templateUrl: './admin-logs.component.html',
  styleUrls: ['./admin-logs.component.css']
})
export class AdminLogsComponent implements OnInit {
  CustomerList: any[];
  LogList = [];
  public loading = false;
  IsAdminLogin: boolean = true
  page: number = 1;
  pageSize: number = 20;
   
  
  constructor(private adminService: SerAdminLogService, public toastr: ToastrService, vcr: ViewContainerRef) {
   }

  ngOnInit() {
    if (localStorage.getItem('dfClientId') == "6") {
      this.IsAdminLogin = true;
    }
    if (localStorage.getItem('dfClientId') != "6") {
      this.IsAdminLogin = false;
    }
    this.FillClientList();
 
  }
  FillClientList() {
    this.loading = true;
    var str = "";
    if (this.IsAdminLogin == true) {
      str = "select DFClientID as id,  ClientName as displayname from DFClients where CountryCode is not null and DFClients.IsDealer=1 order by RIGHT(ClientName, LEN(ClientName) - 3)";
    }
    else {
      str = "";
      str = "select DFClientID as id, ClientName  as displayname  from ( ";
      str = str + " select distinct DFClients.DFClientID,DFClients.ClientName from DFClients ";
      str = str + " inner join AMPlayerTokens on DFClients.DfClientid=AMPlayerTokens.Clientid ";
      str = str + " where DFClients.CountryCode is not null and DFClients.DealerDFClientID= " + localStorage.getItem('dfClientId') + "    ";
      str = str + " union all select distinct DFClients.DFClientID,DFClients.ClientName from DFClients ";
      str = str + " inner join AMPlayerTokens on DFClients.DfClientid=AMPlayerTokens.Clientid ";
      str = str + " where DFClients.CountryCode is not null and DFClients.MainDealerid= " + localStorage.getItem('dfClientId') + "    ";
      str = str + "   ) as a order by RIGHT(ClientName, LEN(ClientName) - 3) ";
    }

    this.adminService.FillCombo(str).pipe()
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
     
    this.adminService.FillAdminLogs(deviceValue).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.LogList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }


}
