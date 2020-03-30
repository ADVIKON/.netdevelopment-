import { Component, OnInit, ViewContainerRef , OnDestroy, AfterViewInit, ViewChild } from '@angular/core';
import { ToastsManager } from 'ng6-toastr';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SerReportService } from '../report/ser-report.service';
import { DataTableDirective } from 'angular-datatables';

import { Subject, Observable, Subscription } from 'rxjs';
import * as pdfMake from 'pdfmake/build/pdfmake.js';
import * as pdfFonts from 'pdfmake/build/vfs_fonts.js';
pdfMake.vfs = pdfFonts.pdfMake.vfs;

@Component({
  selector: 'app-rep-token-info',
  templateUrl: './rep-token-info.component.html',
  styleUrls: ['./rep-token-info.component.css'],
  providers: [NgbModalConfig, NgbModal]
})
export class RepTokenInfoComponent implements AfterViewInit, OnInit, OnDestroy {
  IsAdminLogin: boolean = false;
  CustomerList: any[];
  TokenList = [];
  public loading = false;
  file_Name="";
  dtOptions: any = {};
  dtTrigger: Subject<any> = new Subject();
  @ViewChild(DataTableDirective)
  dtElement: DataTableDirective;
  constructor(config: NgbModalConfig, private modalService: NgbModal, private rService: SerReportService, public toastr: ToastsManager, vcr: ViewContainerRef) {
    config.backdrop = 'static';
    config.keyboard = false;
    this.toastr.setRootViewContainerRef(vcr);
  }

  ngOnInit() {
    if ((localStorage.getItem('dfClientId') == "6") || (localStorage.getItem('dfClientId') == "2")) {
      this.IsAdminLogin = true;
    }
    else {
      this.IsAdminLogin = false;
    }
    this.dtOptions = {
      pagingType: 'numbers',
      pageLength: 50,
      processing: true,
      dom: 'Brtp',

      retrieve: true,
      buttons: [
        {
          extend: 'pdf', orientation: 'landscape',
          pageSize: 'A4', title: '',filename:this.file_Name
          
        }, {
          extend: 'excelHtml5', orientation: 'landscape',
          pageSize: 'A4', title: '',filename:this.file_Name
        }
      ]
    };


    this.FillClientList();
  }
  ngOnDestroy(): void {
    // Do not forget to unsubscribe the event
    this.dtTrigger.unsubscribe();
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

    this.rService.FillCombo(str).pipe()
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
  NewList;
  GetJSONRecord = (array): void => {
    this.NewList = this.CustomerList.filter(order => order.Id == array.Id);
  }
  onChangeCustomer(id) {
    this.rerender();
    if (id == "0") {
      this.TokenList = [];
      return;
    }
    var ArrayItem = {};
    ArrayItem["Id"] = id;
    ArrayItem["DisplayName"] = "";
    this.GetJSONRecord(ArrayItem);
    if (this.NewList.length > 0) {
      this.file_Name = this.NewList[0].DisplayName+ "_Token_Info";
    }
    this.dtOptions = {
      pagingType: 'numbers',
      pageLength: 50,
      processing: true,
      dom: 'Brtp',

      retrieve: true,
      buttons: [
        {
          extend: 'pdf', orientation: 'landscape',
          pageSize: 'A4', title: '',filename:this.file_Name
          
        }, {
          extend: 'excelHtml5', orientation: 'landscape',
          pageSize: 'A4', title: '',filename:this.file_Name
        }
      ]
    };







    this.loading = true;
    this.rService.FillTokenInfo(id, 0).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.TokenList = JSON.parse(returnData);
        this.rerender();
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })

  }
  ngAfterViewInit(): void {
    this.dtTrigger.next();
  }
  rerender(): void {
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.clear();
      // Destroy the table first      
      dtInstance.destroy();
      // Call the dtTrigger to rerender again       
      this.dtTrigger.next();

    });
  }
}
