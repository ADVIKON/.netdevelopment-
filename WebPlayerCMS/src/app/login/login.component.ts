import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastsManager } from 'ng6-toastr';
import { UloginService } from '../login/ulogin.service';
import {HttpClient,HttpErrorResponse  } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/merge';
import 'rxjs/add/operator/map'; 

import { VisitorsService } from '../visitors.service';

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
  images = [1, 2, 3].map(() => `https://picsum.photos/900/500?random&t=${Math.random()}`);
  constructor(private router: Router, private formBuilder: FormBuilder,  private http: HttpClient,
    public toastr: ToastsManager, vcr: ViewContainerRef, private ulService: UloginService,private visitorsService:VisitorsService) {
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
      console.log(res['ip']);
      //console.log(res);

    });
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
}
//SaveModifyLogs