import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng6-toastr';
import { Router } from '@angular/router';
import * as _moment from 'moment';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TokenInfoServiceService } from '../token-info/token-info-service.service';
import { PlaylistLibService } from '../playlist-library/playlist-lib.service';
import { from } from 'rxjs';
const moment = (_moment as any).default ? (_moment as any).default : _moment;

@Component({
  selector: 'app-token-info',
  templateUrl: './token-info.component.html',
  styleUrls: ['./token-info.component.css']
})

export class TokenInfoComponent implements OnInit {
  TokenInfo: FormGroup;
  submitted = false;
  public loading = false;
  CountryList = [];
  StateList = [];
  CityList = [];
  tid: string = "";
  scheduleList = [];
  ModifySchList = [];
  adList = [];
  prayerList = [];
  shortmonths: Array<string>;
  TokenInfoModifyPlaylist: FormGroup;
  StateName = "";
  DelpSchid;
  SongsList = [];
  LogoId;
  constructor(private router: Router, private formBuilder: FormBuilder, public toastr: ToastsManager,
    vcr: ViewContainerRef, config: NgbModalConfig, private modalService: NgbModal,
    private pService: PlaylistLibService, private tService: TokenInfoServiceService) {
    this.toastr.setRootViewContainerRef(vcr);
    config.backdrop = 'static';
    config.keyboard = false;
  }
  public dateTime1 = new moment();
  ngOnInit() {

    this.shortmonths = [
      'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul',
      'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
    ];
    this.tid = localStorage.getItem("tokenid");
    this.TokenInfo = this.formBuilder.group({
      Tokenid: [this.tid],
      token: [""],
      personName: [""],
      country: [""],
      state: [""],
      city: [""],
      street: [""],
      location: [""],
      ExpiryDate: [this.dateTime1],
      PlayerType: [""],
      LicenceType: [""],
      chkMediaType: ["", Validators.required],
      chkuserRights: ["", Validators.required],
      chkType: ["", Validators.required],
      TokenNoBkp: [""],
      DeviceId: [""],
      ScheduleType: [""],
      chkIndicator: [false]

    });
    this.TokenInfoModifyPlaylist = this.formBuilder.group({
      ModifyPlaylistName: [""],
      ModifyStartTime: [""],
      ModifyEndTime: [""],
      pschid: [""]
    });
    this.FillCountry();
    this.scheduleList = [];
    this.adList = [];
    this.prayerList = [];
    this.FillTokenInfo();
  }
  get f() { return this.TokenInfo.controls; }

  onSubmitTokenInfo = function () {
    this.submitted = true;
    if (this.TokenInfo.invalid) {
      return;
    }
    // var date = new Date(this.TokenInfo.value.ExpiryDate);
    //var FromDateS = (date.getDate() + '-' + this.shortmonths[date.getMonth()] + '-' + date.getFullYear());

    this.loading = true;
    this.tService.SaveTokenInfo(this.TokenInfo.value).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Saved", 'Success!');
          this.SaveModifyInfo(this.TokenInfo.value.Tokenid, "Token information is modify");
        }
        else {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
        }
        this.loading = false;
        //this.modalService.dismissAll('Cross click');
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })


  }
  SaveModifyInfo(tokenid, ModifyText) {
    this.tService.SaveModifyLogs(tokenid, ModifyText).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
      },
        error => {
        })
  };











  openModal(content, pname, pschid, stime, eTime) {
    var t = "1900-01-01 " + stime;
    var t2 = "1900-01-01 " + eTime;
    var dt = new Date(t);
    var dt2 = new Date(t2);


    this.TokenInfoModifyPlaylist = this.formBuilder.group({
      ModifyPlaylistName: [pname],
      ModifyStartTime: [dt],
      ModifyEndTime: [dt2],
      pschid: [pschid]
    });
    this.modalService.open(content, { centered: true });
  }
  onSubmitTokenInfoModifyPlaylist() {

    //this.loading = true;
    var sTime = new Date(this.TokenInfoModifyPlaylist.value.ModifyStartTime);
    var eTime = new Date(this.TokenInfoModifyPlaylist.value.ModifyEndTime);
    var pschid = this.TokenInfoModifyPlaylist.value.pschid;
    this.tService.UpdateTokenSch(pschid, sTime.toTimeString().slice(0, 5), eTime.toTimeString().slice(0, 5)).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Saved", 'Success!');
          this.SaveModifyInfo(this.TokenInfo.value.Tokenid, "Token schedule time is modify and schedule id is " + pschid);

          this.FillTokenInfo();
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

  ResetToken() {
    this.loading = true;
    this.tService.ResetToken(this.TokenInfo.value.Tokenid, this.TokenInfo.value.TokenNoBkp).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Token Reset", 'Success!');
          this.SaveModifyInfo(this.TokenInfo.value.Tokenid, "Token is reset");
          this.loading = false;
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
  FillCountry() {
    this.loading = true;
    var qry = "select countrycode as id, countryname as displayname from countrycodes";
    this.tService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.CountryList = JSON.parse(returnData);
        //this.loading=false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onChangeCountry(CountryID) {
    this.loading = true;
    var qry = "select stateid as id, statename as displayname  from tbstate where countryid = " + CountryID + " order by statename";
    this.tService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.StateList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onChangeState(StateID) {
    this.loading = true;
    var qry = "select cityid as id, cityname as displayname  from tbcity where stateid = " + StateID + " order by cityname";
    this.tService.FillCombo(qry).pipe()
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
  FillTokenInfo() {
    this.loading = true;


    this.tService.FillTokenContent(this.tid).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);


        this.scheduleList = obj.lstPlaylistSch;
        this.adList = obj.lstAds;
        this.prayerList = obj.lstPrayer;

        if (obj.lstTokenData == null) {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
          return;
        }
        var objTdata = JSON.stringify(obj.lstTokenData);

        var objTokenData = JSON.parse(objTdata);

        var d = new Date(objTokenData[0].ExpiryDate)
        this.onChangeCountry(objTokenData[0].country);
        this.onChangeState(objTokenData[0].state);

        this.TokenInfo = this.formBuilder.group({
          Tokenid: [this.tid],
          token: [objTokenData[0].token],
          personName: [objTokenData[0].personName],
          country: [objTokenData[0].country],
          state: [objTokenData[0].state],
          city: [objTokenData[0].city],
          street: [objTokenData[0].street],
          location: [objTokenData[0].location],
          ExpiryDate: [d],
          PlayerType: [objTokenData[0].PlayerType],
          LicenceType: [objTokenData[0].LicenceType],
          chkMediaType: [objTokenData[0].chkMediaType, Validators.required],
          chkuserRights: [objTokenData[0].chkuserRights, Validators.required],
          chkType: [objTokenData[0].chkType, Validators.required],
          TokenNoBkp: [objTokenData[0].TokenNoBkp],
          DeviceId: [objTokenData[0].DeviceId],
          ScheduleType: [objTokenData[0].ScheduleType],
          chkIndicator: [objTokenData[0].Indicator],
        });
        this.loading = false;
        this.onChangeSchedule(objTokenData[0].ScheduleType);

      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }


  openResetModal(mContent) {
    this.modalService.open(mContent, { centered: true });
  }
  openStateModal(content) {
    this.modalService.open(content, { centered: true });
  }
  onSubmitState() {

  }
  openDeleteModal(content, pschid) {
    this.DelpSchid = pschid;
    this.modalService.open(content, { centered: true });
  }
  DeleteSchPlaylist() {
    this.loading = true;
    this.tService.DeleteTokenSch(this.DelpSchid).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Deleted", 'Success!');
          this.SaveModifyInfo(this.tid, "Token schedule time is delete and schedule id is " + this.DelpSchid);
          this.loading = false;
          this.FillTokenInfo();
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

  onChangeSchedule(schType) {
    var schItem={};
    this.loading = true;
    this.ModifySchList=[];
    if (schType == "Normal") {
      this.scheduleList.forEach(item => {
        schItem={};
        schItem["formatName"] = item.formatName;
        schItem["playlistName"] = item.playlistName;
        schItem["StartTime"] = item.StartTime;
        schItem["EndTime"] = item.EndTime;
        schItem["WeekDay"] = item.WeekDay;
        schItem["id"] = item.id;
        this.ModifySchList.push(schItem)
      });
    }
    if (schType == "OneToOnePlaylist") {
      this.scheduleList.forEach(item => {
        schItem={};
        schItem["formatName"] = item.formatName;
        schItem["playlistName"] = item.playlistName;
        schItem["StartTime"] = "00:00";
        schItem["EndTime"] = "00:00";
        schItem["WeekDay"] = item.WeekDay;
        schItem["id"] = item.id;
        this.ModifySchList.push(schItem)
      });
    }
    this.loading = false;    
  }
}
