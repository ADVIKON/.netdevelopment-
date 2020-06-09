import { Component, OnInit } from '@angular/core';
import { ConfigAPI } from '../class/ConfigAPI';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-machine-announcement',
  templateUrl: './machine-announcement.component.html',
  styleUrls: ['./machine-announcement.component.css']
})
export class MachineAnnouncementComponent implements OnInit {
  public loading = false;
  CustomerList: any[];
  IsAdminLogin;
  constructor(public toastr: ToastrService,  private cf: ConfigAPI,
     config: NgbModalConfig, private modalService: NgbModal) {
      config.backdrop = 'static';
    config.keyboard = false;
     }

  ngOnInit(): void {
  }
  // FillClientList() {
  //   this.loading = true;
  //   var str = "";
  //   if (this.IsAdminLogin == true) {
  //     str = "select DFClientID as id,  ClientName as displayname from DFClients where CountryCode is not null and DFClients.IsDealer=1 order by RIGHT(ClientName, LEN(ClientName) - 3)";
  //   }
  //   else {
  //     str = "";
  //     str = "select DFClientID as id, ClientName  as displayname  from ( ";
  //     str = str + " select distinct DFClients.DFClientID,DFClients.ClientName from DFClients ";
  //     str = str + " inner join AMPlayerTokens on DFClients.DfClientid=AMPlayerTokens.Clientid ";
  //     str = str + " where DFClients.CountryCode is not null and DFClients.DealerDFClientID= " + localStorage.getItem('dfClientId') + "    ";
  //     str = str + " union all select distinct DFClients.DFClientID,DFClients.ClientName from DFClients ";
  //     str = str + " inner join AMPlayerTokens on DFClients.DfClientid=AMPlayerTokens.Clientid ";
  //     str = str + " where DFClients.CountryCode is not null and DFClients.MainDealerid= " + localStorage.getItem('dfClientId') + "    ";
  //     str = str + "   ) as a order by RIGHT(ClientName, LEN(ClientName) - 3) ";
  //   }

  //   this.serviceStream.FillCombo(str).pipe()
  //     .subscribe(data => {
  //       var returnData = JSON.stringify(data);
  //       this.CustomerList = JSON.parse(returnData);
  //       this.loading = false;
  //       this.StreamList = [];
  //     },
  //       error => {
  //         this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
  //         this.loading = false;
  //       })
  // }
}
