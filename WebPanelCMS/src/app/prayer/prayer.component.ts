import { Component, OnInit, ViewContainerRef, EventEmitter, ViewChildren, QueryList, ElementRef, ViewChild, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import * as _moment from 'moment';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PrayerserService } from '../prayer/prayerser.service';
const moment = (_moment as any).default ? (_moment as any).default : _moment;

@Component({
  selector: 'app-prayer',
  templateUrl: './prayer.component.html',
  styleUrls: ['./prayer.component.css']
})
export class PrayerComponent implements OnInit {
  PrayerList = [];
  Prayerform: FormGroup;
  CustomerList = [];
  TokenList = [];
  CustomerSelected = [];
  TokenSelected = [];

  SearchCustomerList = [];
  SearchTokenList = [];
  loading: boolean = false;
  SearchPDate;
  cmbSearchCustomer = 0;
  cmbSearchToken = 0;
pid;
submitted;
  @ViewChildren("checkboxesCustomer") checkboxesCustomer: QueryList<ElementRef>;

  constructor(private formBuilder: FormBuilder, public toastr: ToastrService,
    vcr: ViewContainerRef, private pService: PrayerserService,config: NgbModalConfig, private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    var cd = new Date;
    this.Prayerform = this.formBuilder.group({
      sDate: [cd, Validators.required],
      eDate: [cd, Validators.required],
      startTime: [cd, Validators.required],
      duration: [0, Validators.required],
      tokenid: [this.TokenSelected],
    });
    this.PrayerList = [];
    this.SearchPDate = cd;
    this.CustomerList = [];

    this.TokenList = [];
    this.FillClientList();
  }

  get f() { return this.Prayerform.controls; }
  onSubmitPrayer = function () {

    this.submitted = true;
    if (this.Prayerform.invalid) {
      return;
    }
    if (this.Prayerform.value.duration == 0) {
      this.toastr.error("Duration should be greater than zero");
      return;
    }
    if (this.CustomerSelected.length == 0) {
      this.toastr.error("Please select customer from list");
      return;
    }
    if (this.TokenSelected.length == 0) {
      this.toastr.error("Select the token(s) from this list");
      return;
    }

    this.SavePrayer();
  }
  Refresh() {
    var cd = new Date;
    this.CustomerSelected = [];
    this.TokenList = [];
    this.TokenSelected = [];

    this.Prayerform = this.formBuilder.group({
      sDate: [cd, Validators.required],
      eDate: [cd, Validators.required],
      startTime: [cd, Validators.required],
      duration: [0, Validators.required],
      tokenid: [this.TokenSelected],
    });
    
    this.checkboxesCustomer.forEach((element) => {
      element.nativeElement.checked = false;
    });

  }
  SavePrayer() {
    this.loading = true;
    this.pService.SavePrayer(this.Prayerform.value).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Saved", '');
          this.Refresh();
        }
        else {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
        }
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }

  SelectCustomer(fileid, event) {
    if (event.target.checked) {
      this.CustomerSelected.push(fileid);
    }
    else {
      const index: number = this.CustomerSelected.indexOf(fileid);
      if (index !== -1) {
        this.CustomerSelected.splice(index, 1);
      }
    }
    if (this.CustomerSelected.length!=0){
      this.FillTokenInfo();
    }
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
  FillClientList() {
    this.loading = true;
    this.pService.FillClientCombo().pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.CustomerList = JSON.parse(returnData);
        this.SearchCustomerList = JSON.parse(returnData);
        this.loading = false;

      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }

  FillTokenInfo() {
    this.loading = true;
    this.pService.FillTokenInfoPrayer(this.CustomerSelected).pipe()
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
  onChangeCustomer(e) {
    this.loading = true;
    this.pService.FillTokenInfo(e).pipe()
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
  openPrayerDeleteModal(mContent, id){
    this.pid = id;
    this.modalService.open(mContent, { centered: true });
  }
  DeleteParyer(){
    this.loading = true;
    this.pService.DeletePrayer(this.pid).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce=="1"){
          this.toastr.info("Deleted", '');
          this.SearchPrayer();
        }
        else{
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
        }
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  SearchPrayer(){
   this.loading = true;
    this.pService.FillSearchPayer(this.SearchPDate, this.cmbSearchToken).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.PrayerList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
}
