import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng6-toastr';
import { Router } from '@angular/router';
import { CustomerRegService } from '../customer-registration/customer-reg.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as _moment from 'moment';
const moment = (_moment as any).default ? (_moment as any).default : _moment;
@Component({
  selector: 'app-customer-registration',
  templateUrl: './customer-registration.component.html',
  styleUrls: ['./customer-registration.component.css']
})
export class CustomerRegistrationComponent implements OnInit {
  Regform: FormGroup;
  submitted = false;
  CustomerList = [];
  CountryList = [];
  StateList = [];
  CityList = [];
  page: number = 1;
  pageSize: number = 10;
  public loading = false;
  searchText;
  cName;
  citName;
  Code;
  did;
  IsEditClick: string = "No";
  constructor(private router: Router, private formBuilder: FormBuilder, public toastr: ToastsManager, vcr: ViewContainerRef, private cService: CustomerRegService, config: NgbModalConfig, private modalService: NgbModal) {
    this.toastr.setRootViewContainerRef(vcr);
    config.backdrop = 'static';
    config.keyboard = false;
  }
  public dateTime1 = new moment();
  ngOnInit() {
    this.Regform = this.formBuilder.group({
      countryName: ["", Validators.required],
      cCode: [""],
      stateName: ["", Validators.required],
      cityName: ["", Validators.required],
      customerName: ["", Validators.required],
      customerEmail: ["", [Validators.required, Validators.email]],
      totalToken: ["", Validators.required],
      expiryDate: [this.dateTime1, Validators.required],
      supportEmail: [""],
      supportPhNo: [""],
      Street: [""],
      DfClientId: [""],
      LoginId: [""]
    });
    this.CustomerList = [];
    this.FillCountry();
    this.FillCustomer();
  }

  get f() { return this.Regform.controls; }
  Refresh = function () {
    this.IsEditClick = "No";
    this.Regform = this.formBuilder.group({
      countryName: ["", Validators.required],
      cCode: [""],
      stateName: ["", Validators.required],
      cityName: ["", Validators.required],
      customerName: ["", Validators.required],
      customerEmail: ["", [Validators.required, Validators.email]],
      totalToken: ["", Validators.required],
      expiryDate: ["", Validators.required],
      supportEmail: [""],
      supportPhNo: [""],
      Street: [""],
      DfClientId: [""],
      LoginId: [""]
    });
  };
  onChangeCity(event: Event) {
    let selectElementText = event.target['options']
    [event.target['options'].selectedIndex].text;
    this.citName = selectElementText;
  }
  onChangeToken() {
    if (this.IsEditClick == "No") {
      this.Code = this.Regform.value.customerName.replace("-", "").substring(0, 5) + "" + this.citName.substring(0, 2) + "00" + this.CustomerList.length;
    }
  }
  onSubmitReg = function () {
    this.submitted = true;
    if (this.Regform.invalid) {
      return;
    }

    this.loading = true;
    this.cService.SaveCustomer(this.Regform.value).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Saved", 'Success!');
          this.loading = false;
          this.Refresh();
          this.FillCustomer();
        }
        else {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        }
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }

  FillCountry() {
    this.loading = true;
    var qry = "select countrycode as id, countryname as displayname from countrycodes";
    this.cService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.CountryList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onChangeCountry(CountryID) {
    this.loading = true;
    var qry = "select stateid as id, statename as displayname  from tbstate where countryid = " + CountryID + " order by statename";
    this.cService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.StateList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })

    if (this.IsEditClick == "No") {
      var qry = "select countrycode as id , CountryNameShort as displayname from CountryCodes where countryCode = " + CountryID + " ";
      this.cService.FillCombo(qry).pipe()
        .subscribe(data => {
          var returnData = JSON.stringify(data);
          var obj = JSON.parse(returnData);

          this.cName = obj[0].DisplayName + "-";

          this.loading = false;
        },
          error => {
            this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
            this.loading = false;
          })
    }


  }
  onChangeState(StateID) {
    this.loading = true;
    var qry = "select cityid as id, cityname as displayname  from tbcity where stateid = " + StateID + " order by cityname";
    this.cService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.CityList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  FillCustomer() {
    this.loading = true;
    this.cService.FillCustomer().pipe()
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

  openCustomerDeleteModal(mContent, id) {
    this.did = id;
    this.modalService.open(mContent, { centered: true });
  }
  EditCustomer(id) {
    this.loading = true;
    this.cService.EditClickCustomer(id).pipe()
      .subscribe(data => {
        this.IsEditClick = "Yes";
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);

        var d = new Date(obj.expiryDate)
        this.onChangeCountry(obj.countryName);
        this.onChangeState(obj.stateName);
        this.Regform = this.formBuilder.group({
          countryName: [obj.countryName],
          cCode: [obj.cCode],
          stateName: [obj.stateName],
          cityName: [obj.cityName],
          customerName: [obj.customerName],
          customerEmail: [obj.customerEmail],
          totalToken: [obj.totalToken],
          expiryDate: [d],
          supportEmail: [obj.supportEmail],
          supportPhNo: [obj.supportPhNo],
          Street: [obj.Street],
          DfClientId: [obj.DfClientId],
          LoginId: [obj.LoginId]
        });
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  DeleteCustomer() {
    this.loading = true;
    this.cService.DeleteCustomer(this.did).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Deleted", 'Success!');
          this.loading = false;
          this.FillCustomer();
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

}
