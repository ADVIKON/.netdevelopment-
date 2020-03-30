import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng6-toastr';
import { StoreForwardService } from '../store-and-forward/store-forward.service';
import { TokenInfoServiceService } from '../token-info/token-info-service.service';
import { SerLicenseHolderService } from '../license-holder/ser-license-holder.service';
import { SerCopyDataService } from '../copy-data/ser-copy-data.service';

@Component({
  selector: 'app-copy-data',
  templateUrl: './copy-data.component.html',
  styleUrls: ['./copy-data.component.css']
})
export class CopyDataComponent implements OnInit {

  page: number = 1;
  pageSize: number = 50;
  IsAdminLogin: boolean = false;
  public loading = false;
  CustomerList: any[];
  SearchTokenList = [];
  ScheduleList = [];
  TokenList: any[];
  TokenSelected = [];
  CDform: FormGroup;
  CustomerSelected;
  cmbCustomer: number;
  cmbSearchCustomer: number;
  cmbSearchToken: number;

  constructor(private formBuilder: FormBuilder, public toastr: ToastsManager, vcr: ViewContainerRef,
    private sfService: StoreForwardService, private tService: TokenInfoServiceService,
    private serviceLicense: SerLicenseHolderService, private cService: SerCopyDataService) {
    this.toastr.setRootViewContainerRef(vcr);
  }

  ngOnInit() {
    this.CDform = this.formBuilder.group({
      SchList: [this.ScheduleList],
      TokenList: [this.TokenSelected],
      dfClientId: [this.CustomerSelected]
    });
    if (localStorage.getItem('dfClientId') == "6") {
      this.IsAdminLogin = true;
    }
    else {
      this.IsAdminLogin = false;
    }
    this.TokenList = [];
    this.FillClient();
  }
  FillClient() {
    var q = "";
    if (this.IsAdminLogin == true) {
      q = "select DFClientID as id,  ClientName as displayname from DFClients where CountryCode is not null and DFClients.IsDealer=1 order by RIGHT(ClientName, LEN(ClientName) - 3)";
    }
    else{
    q = "select DFClientID as id,ClientName as displayname from ( ";
    q = q + " select distinct DFClients.DFClientID,DFClients.ClientName from DFClients ";
    q = q + " inner join AMPlayerTokens on DFClients.DfClientid=AMPlayerTokens.Clientid ";
    q = q + " where DFClients.CountryCode is not null and DFClients.DealerDFClientID= " + localStorage.getItem('dfClientId') + "    ";
    q = q + " union all select distinct DFClients.DFClientID,DFClients.ClientName from DFClients ";
    q = q + " inner join AMPlayerTokens on DFClients.DfClientid=AMPlayerTokens.Clientid ";
    q = q + " where DFClients.CountryCode is not null and DFClients.MainDealerid= " + localStorage.getItem('dfClientId') + "    ";
    q = q + "   ) as a order by ClientName desc ";
    }
    this.loading = true;
    this.sfService.FillCombo(q).pipe()
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
  onChangeSearchCustomer(id) {
    this.ScheduleList = [];

    this.loading = true;
    this.serviceLicense.FillTokenInfo(id).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.SearchTokenList = JSON.parse(returnData);

        this.loading = false;

      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onChangeToken(id) {
    this.ScheduleList = [];
    this.loading = true;
    this.tService.FillTokenContent(id).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        this.ScheduleList = obj.lstPlaylistSch;
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onChangeCustomer(id) {
    this.CustomerSelected = id;
    this.FillTokenInfo(id);
  }
  FillTokenInfo(deviceValue) {
    this.loading = true;
    this.sfService.FillTokenInfo(deviceValue).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.TokenList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  SelectToken(fileid, event) {
    if (event.target.checked) {
      this.TokenSelected.push(fileid);
    }
    else {
      const index: number = this.TokenSelected.indexOf(fileid);
      if (index !== -1) {
        this.TokenSelected.splice(index, 1);
      }
    }
  }
  SaveContent() {
    if (this.ScheduleList.length == 0) {
      this.toastr.info("Please select a schedule");
      return;
    }
    if (this.TokenSelected.length == 0) {
      this.toastr.info("Please select a token");
      return;
    }


    this.CDform.get('SchList').setValue(this.ScheduleList);
    this.CDform.get('dfClientId').setValue(this.CustomerSelected);

    this.loading = true;
    this.cService.SaveCopySchedule(this.CDform.value).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Saved", 'Success!');
          this.loading = false;
          this.cmbSearchCustomer = 0;
          this.cmbSearchToken = 0;
          this.SearchTokenList = [];
          this.ScheduleList = [];
          this.TokenList = [];
          this.TokenSelected = [];
          this.cmbCustomer = 0;

        }
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  allToken(event){
    const checked = event.target.checked;
    this.TokenSelected=[];
    this.TokenList.forEach(item=>{
      item.check = checked;
      this.TokenSelected.push(item.tokenid)
    });
    if (checked==false){
      this.TokenSelected=[];
    }
    
  }
}
