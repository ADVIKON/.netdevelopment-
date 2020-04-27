import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { CustomerRegService } from '../customer-registration/customer-reg.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
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
  pageSize: number = 20;
  public loading = false;
  searchText;
  cName;
  citName;
  Code;
  did;
  IsEditClick: string = "No";
  ModalType;
  NewName;
  ModifyStateName;
  ModifyStateId="0";
  ModifyCityId;
  HeaderText;
  CommonId;
  Country_Id="0";
  MainCustomerList;
  iCheckMain:boolean=true;
  iCheckSub:boolean=false;


  IsAdminLogin: boolean = false;
  UserId;
chkDashboard;
chkPlayerDetail;
chkPlaylistLibrary;
chkScheduling;
chkAdvertisement;
chkInstantPlay;


  constructor(private router: Router, private formBuilder: FormBuilder, public toastr: ToastrService, vcr: ViewContainerRef, private cService: CustomerRegService, config: NgbModalConfig, private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
  }
  public dateTime1 = new Date();
  ngOnInit() {
    if (localStorage.getItem('dfClientId') == "6") {
      this.IsAdminLogin = true;
    
    }
    else  if (localStorage.getItem('dfClientId') == "2") {
      this.IsAdminLogin = true;
       
    }
    else {
      this.IsAdminLogin = false;
      
    }


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
      LoginId: [""],
      CustomerType: [""],
      MainCustomer: ["0"]
    });
    this.CustomerList = [];
    this.FillCountry();
    this.FillCustomer();

    this.UserId= localStorage.getItem('UserId');
    this.chkDashboard=localStorage.getItem('chkDashboard');
    this.chkPlayerDetail=localStorage.getItem('chkPlayerDetail');
    this.chkPlaylistLibrary=localStorage.getItem('chkPlaylistLibrary');
    this.chkScheduling=localStorage.getItem('chkScheduling');
    this.chkAdvertisement=localStorage.getItem('chkAdvertisement');
    this.chkInstantPlay=localStorage.getItem('chkInstantPlay');

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
    let selectElementText = event.target['options'][event.target['options'].selectedIndex].text;
    let selectElementValue = event.target['options'][event.target['options'].selectedIndex].value;
    this.citName = selectElementText;
    this.ModifyCityId = selectElementValue;
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
          return;
        }
        else if (obj.Responce == "0") {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
          return;
        }
        else if (obj.Responce != "1") {
          this.toastr.info("Saved", 'Success!');
          this.loading = false;
          this.Refresh();
          this.SendMail(obj.Responce);
          return;
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
    this.Country_Id = CountryID;
    this.FillState(CountryID);
  }
  FillState(CountryID) {
    this.loading = true;
    this.Regform.get('stateName').setValue("0");
    this.ModifyStateId="0";
    this.StateList=[];
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
    var ArrayItem = {};
    ArrayItem["Id"] = StateID;
    ArrayItem["DisplayName"] = "";
    this.NewFilterList=[];
    this.GetJSONRecord(ArrayItem, this.StateList);
    if (this.NewFilterList.length > 0) {
      this.ModifyStateName = this.NewFilterList[0].DisplayName;
    }
    this.ModifyStateId = StateID;
    this.FillCity(StateID);
  }
  FillCity(StateID) {
    this.Regform.get('cityName').setValue("0");
    this.ModifyCityId="0";
    this.citName="";
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
          LoginId: [obj.LoginId],
          MainCustomer:[obj.MainCustomer],
          CustomerType:[obj.CustomerType]
        });
        
          if (obj.CustomerType=="MainCustomer"){
              this.iCheckMain=true;   
              this.iCheckSub=false;         
          }
          else{
            this.MainCustomerList= this.CustomerList;
            this.iCheckMain=false;   
            this.iCheckSub=true;         
        }

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

  SendMail(Id) {
    this.loading = true;
    this.cService.SendMail(Id).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Email is Sent", 'Success!');
          this.loading = false;
          this.FillCustomer();
        }
        else {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
          return;
        }
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })

  }

  openCommonModal(modal, ModalType) {
    this.ModalType = ModalType;
    if (ModalType == 'State') {
      this.HeaderText = "State";
      if (this.Country_Id =="0"){
        this.toastr.error("Please select a county");
        return;
      }
      this.NewName = this.ModifyStateName;
      this.CommonId = this.ModifyStateId;
      this.modalService.open(modal);

    }
    if (ModalType == 'City') {
      if (this.ModifyStateId=="0"){
        this.toastr.error("Please select a state");
        return;
      }
      this.HeaderText = "City";
      this.NewName = this.citName;
      this.CommonId = this.ModifyCityId;
      this.modalService.open(modal);
    }
  }
  NewFilterList;
  GetJSONRecord = (array, list): void => {
    this.NewFilterList = list.filter(order => order.Id == array.Id);
  }

  onSubmitModal() {
    this.loading = true;
    this.cService.CitySateNewModify(this.CommonId, this.NewName, this.ModalType, this.ModifyStateId, this.Country_Id).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Saved", 'Success!');
          this.loading = false;
          if (this.ModalType == 'State') {
            this.FillState(this.Country_Id);
          }
          if (this.ModalType == 'City') {
           this.FillCity(this.ModifyStateId);
          }
        }
       else if (obj.Responce == "-2"){
          this.toastr.info("Name is already exixts", '');
          this.loading = false;
        }
        else {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
          return;
        }
        this.NewName="";
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  SetCustomerType(cType){
    this.MainCustomerList=[];
    if (cType=="SubCustomer"){
      this.SetMainCustomerCombo();
    }
    this.Regform.get('MainCustomer').setValue('6');
    this.Regform.get('CustomerType').setValue(cType);
  }
  SetMainCustomerCombo(){
    this.MainCustomerList= this.CustomerList;
  }
}
