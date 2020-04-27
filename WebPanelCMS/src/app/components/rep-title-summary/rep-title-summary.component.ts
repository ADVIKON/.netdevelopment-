import { Component, OnInit, ViewContainerRef, OnDestroy, AfterViewInit, ViewChild } from '@angular/core';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject, Observable, Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import * as pdfMake from 'pdfmake/build/pdfmake.js';
import * as pdfFonts from 'pdfmake/build/vfs_fonts.js';
import { SerReportService } from 'src/app/report/ser-report.service';
import { ToastrService } from 'ngx-toastr';
pdfMake.vfs = pdfFonts.pdfMake.vfs;

@Component({
  selector: 'app-rep-title-summary',
  templateUrl: './rep-title-summary.component.html',
  styleUrls: ['./rep-title-summary.component.css'],
  providers: [NgbModalConfig, NgbModal]
})
export class RepTitleSummaryComponent implements AfterViewInit, OnInit, OnDestroy {
  IsAdminLogin: boolean = false;
  CustomerList: any[];
  TokenList = [];
  public loading = false;
  SearchFromDate;
  SearchToDate;
  dtOptions: any = {};
  dtTrigger: Subject<any> = new Subject();
  tokenid = "0";
  PlayedSongList;
  file_Name = "";
  Client_Name = "";
  cid;
  @ViewChild(DataTableDirective)
  dtElement: DataTableDirective;
  constructor(config: NgbModalConfig, private modalService: NgbModal,
     private rService: SerReportService, public toastr: ToastrService, vcr: ViewContainerRef) {
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    var cd = new Date();
    this.SearchFromDate = cd;
    this.SearchToDate = cd;
    if ((localStorage.getItem('dfClientId') == "6") || (localStorage.getItem('dfClientId') == "2")) {
      this.IsAdminLogin = true;
    }
    else {
      this.IsAdminLogin = false;
    }
    this.DataTableSettings();
    this.FillClientList();
  }
  DataTableSettings() {
    this.dtOptions = {
      pagingType: 'numbers',
      pageLength: 50,
      processing: false,
      dom: 'Brtp',
      columnDefs: [{
        'targets': [0, 1,2], // column index (start from 0)
        'orderable': false,
      },{
        'width':'110px', 'targets': 2 
      }],
      retrieve: true,
      buttons: [
        {
          extend: 'pdf',
          pageSize: 'A4', title: '', filename: this.file_Name,
          exportOptions: {
            columns: [0, 1,2]
          }
        }, {
          extend: 'excelHtml5',
          pageSize: 'A4', title: '', filename: this.file_Name,
          exportOptions: {
            columns: [0, 1,2]
          }
        }
      ]
    };
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
    this.PlayedSongList = [];
    this.cid=id;
    if (id == "0") {
      this.TokenList = [];
      return;
    }

    var ArrayItem = {};
    ArrayItem["Id"] = id;
    ArrayItem["DisplayName"] = "";
    this.GetJSONRecord(ArrayItem);
    if (this.NewList.length > 0) {
      this.Client_Name = this.NewList[0].DisplayName;
    }

    this.loading = true;
    this.rService.FillTokenInfo(id, 1).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.TokenList = JSON.parse(returnData);
        this.loading = false;
        this.SearchPlayedTitleSummary();
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })

  }
  onChangePlayer(id) {
    this.PlayedSongList = [];
    this.tokenid = id;
    this.SearchPlayedTitleSummary();
  }
  ngAfterViewInit(): void {
    this.dtTrigger.next();
  }
  ngOnDestroy(): void {
    // Do not forget to unsubscribe the event
    this.dtTrigger.unsubscribe();
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
  SearchPlayedTitleSummary() {
    var cd = new Date();
    var FromDate = new Date(this.SearchFromDate);
    var ToDate = new Date(this.SearchToDate);

    if (FromDate.getDate() > ToDate.getDate()) {
      this.SearchFromDate = cd;
      this.SearchToDate = cd;
      return;
    }
    
    this.PlayedSongList = [];
    this.rerender();
if (this.tokenid=="0"){
  this.file_Name = this.Client_Name + "_"  + FromDate.toDateString() + "_" + ToDate.toDateString() + "_TitleSummary";
}
else{
  this.file_Name = this.Client_Name + "_" + this.tokenid + "_" + FromDate.toDateString() + "_" + ToDate.toDateString() + "_TitleSummary";
}
this.DataTableSettings();

    this.loading = true;
 
    this.rService.FillPlayedTitleSummary(FromDate.toDateString(), ToDate.toDateString(), this.tokenid,this.cid).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.PlayedSongList = JSON.parse(returnData);
        this.rerender();
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }

}
