import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastsManager } from 'ng6-toastr';
import { UloginService } from '../login/ulogin.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/merge';
import 'rxjs/add/operator/map';

import { VisitorsService } from '../visitors.service';
import { not } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginform: FormGroup;
  submitted = false;
  public loading = false;
  ipAddress;

  MainArray = [];
  SubMainArray = [];
  columns_FirstName = [];
  columns_LastName = [];
  columns_PropertyAddress = [];
  columns_Input_Mailing_Address=[];
  Input_Mailing_Address="-1";
  ItemSelected = [];
  FirstName = "-1";
  LastName = "-1";
  PropertyAddress = "-1";
  images = [1, 2, 3].map(() => `https://picsum.photos/900/500?random&t=${Math.random()}`);
  constructor(private router: Router, private formBuilder: FormBuilder, private http: HttpClient,
    public toastr: ToastsManager, vcr: ViewContainerRef, private ulService: UloginService, private visitorsService: VisitorsService) {
    this.toastr.setRootViewContainerRef(vcr);
  }



  ngOnInit() {
    localStorage.clear();
    this.loginform = this.formBuilder.group({
      email: ["", Validators.required],
      password: ["", Validators.required]
    });

    this.visitorsService.getIpAddress().subscribe(res => {
      localStorage.setItem('ipAddress', res['ip']);
    });
    //this.MainArray = [{ "name": "", "index": -1 }, { "index": 0, "name": "Input_First_Name" }, { "index": 1, "name": "Input_Last_Name" }, { "index": 2, "name": "Input_Mailing_Address" }, { "index": 3, "name": "Input_Mailing_City" }, { "index": 4, "name": "Input_Mailing_State" }, { "index": 5, "name": "Input_Mailing_Zip" }, { "index": 6, "name": "Input_Property_Address" }, { "index": 7, "name": "Input_Property_City" }, { "index": 8, "name": "Input_Property_State" }, { "index": 9, "name": "Input_Property_Zip" }, { "index": 10, "name": "Preferred_Mailing_Address" }, { "index": 11, "name": "Preferred_Mailing_City" }, { "index": 12, "name": "Preferred_Mailing_State" }, { "index": 13, "name": "Preferred_Mailing_Zip" }, { "index": 14, "name": "Preferred_Mailing_Date_First_Seen" }, { "index": 15, "name": "Preferred_Mailing_Date_Last_Seen" }, { "index": 16, "name": "Phone0_Number" }, { "index": 17, "name": "Phone0_Type" }, { "index": 18, "name": "Phone0_Score" }, { "index": 19, "name": "Phone1_Number" }];
    this.MainArray = [{ "name": "", "index": -1 }, { "index": 0, "name": "Input_First_Name" }, { "index": 1, "name": "Input_Last_Name" }, { "index": 2, "name": "Input_Mailing_Address" },  { "index": 6, "name": "Input_Property_Address" }];
    this.columns_FirstName = this.MainArray;
    this.columns_LastName = this.MainArray;
    this.columns_PropertyAddress = this.MainArray;
    this.SubMainArray = this.MainArray;

  }


  get f() { return this.loginform.controls; }

  onSubmit = function () {

    this.submitted = true;
    if (this.loginform.invalid) {
      return;
    }
    this.loading = true;
    this.ulService.uLogin(this.loginform.value).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);

        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          localStorage.setItem('UserId', obj.UserId);
          localStorage.setItem('dfClientId', obj.dfClientId);
          localStorage.setItem('IsRf', obj.IsRf);
          localStorage.setItem('chkDashboard', obj.chkDashboard);
          localStorage.setItem('chkPlayerDetail', obj.chkPlayerDetail);
          localStorage.setItem('chkPlaylistLibrary', obj.chkPlaylistLibrary);
          localStorage.setItem('chkScheduling', obj.chkScheduling);
          localStorage.setItem('chkAdvertisement', obj.chkAdvertisement);
          localStorage.setItem('chkInstantPlay', obj.chkInstantPlay);

          this.router.navigate(['Dashboard']);

        }
        else if (obj.Responce == "0") {
          this.toastr.error("Login user/password is wrong", '');
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
  


  
  changed1(id) {
    this.SelectedItem();
     this.FillValues();
  }
  
  changed2(id) {
    this.SelectedItem();
     this.FillValues();
  }

  changed3(id) {
    this.SelectedItem();
    this.FillValues();
  }
  changed4(id) {
    this.SelectedItem();
    this.FillValues();
  }








  PushItem(Value,ArrayList){
    var name=[];
      name = this.MainArray.filter(order => order.index == Value);
      if (name.length > 0) {
        name.forEach((obj) => {
         if (obj.index!="-1"){
         ArrayList.push(obj);
         }
         });
        };
  }
   FillValues(){
    this.columns_FirstName = [];
    this.columns_LastName = [];
    this.columns_PropertyAddress = [];
    this.columns_Input_Mailing_Address=[];
  
      this.columns_FirstName = this.Filter(); 
      this.columns_LastName = this.Filter(); 
      this.columns_PropertyAddress = this.Filter(); 
      this.columns_Input_Mailing_Address = this.Filter(); 

      this.PushItem(this.FirstName,this.columns_FirstName);
      this.PushItem(this.LastName,this.columns_LastName);
      this.PushItem(this.PropertyAddress,this.columns_PropertyAddress);
      this.PushItem(this.Input_Mailing_Address,this.columns_Input_Mailing_Address);
   }


  SelectedItem() {
    this.ItemSelected = [];
    if (this.FirstName != "-1") {
      this.ItemSelected.push(this.FirstName);
    }
    if (this.LastName != "-1") {
      this.ItemSelected.push(this.LastName);
    }
    if (this.PropertyAddress != "-1") {
      this.ItemSelected.push(this.PropertyAddress);
    }
    if (this.Input_Mailing_Address != "-1") {
      this.ItemSelected.push(this.Input_Mailing_Address);
    }
    this.ItemSelected = this.removeDuplicates(this.ItemSelected);

  }

  removeDuplicates(array) {
    let x = {};
    array.forEach(function (i) {
      if (!x[i]) {
        x[i] = true
      }
    })
    return Object.keys(x)
  };
  
  Filter() {
    var ObjLocal = [];
    var returnValue=[];
    var ObjList=[];
    
    for (var i = 0; i < this.ItemSelected.length; i++) {
      ObjLocal = this.MainArray.filter(order => order.index == this.ItemSelected[i]);
      if (ObjLocal.length > 0) {
        ObjLocal.forEach((obj) => {
          ObjList.push(obj);
          returnValue = this.removeDuplicateRecord(obj);
        });
      }
    }
     
    if (this.MainArray.length > 0){
      this.MainArray.forEach((objL) => {
          var existNotification = ObjList.find(({index}) => objL.index === index);
          var existNotification2 = this.SubMainArray.find(({index}) => objL.index === index);
           if ((!existNotification) && (!existNotification2)){
            returnValue.push(objL);
           }

      });
    }
     return returnValue;
  }
  removeDuplicateRecord(array) {
    this.SubMainArray = this.SubMainArray.filter(order => order.index !== array.index);
    return this.SubMainArray;
  }
  
}
//SaveModifyLogs

