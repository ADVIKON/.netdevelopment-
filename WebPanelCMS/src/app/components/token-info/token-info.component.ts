import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TokenInfoServiceService } from './token-info-service.service';
@Component({
  selector: 'app-token-info',
  templateUrl: './token-info.component.html',
  styleUrls: ['./token-info.component.css']
})

export class TokenInfoComponent implements OnInit {
  TokenInfo: FormGroup;
  submitted = false;
  chkIndicatorBox:boolean=false;
  public loading = false;
  CountryList = [];
  StateList = [];
  CityList = [];
  GroupList = [];
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
  ModalType;
  NewName;
  ModifyStateName;
  ModifyStateId = "0";
  ModifyCityId = "0";
  HeaderText;
  CommonId;
  citName;
  Country_Id = "0";
  ModifyGroupId = "0";
  ModifyGroupName = "";
  ClientId=0;
  constructor(private router: Router, private formBuilder: FormBuilder, public toastr: ToastrService,
    vcr: ViewContainerRef, config: NgbModalConfig, private modalService: NgbModal,
      private tService: TokenInfoServiceService) {
    
    config.backdrop = 'static';
    config.keyboard = false;
  }
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
      ExpiryDate: [""],
      PlayerType: [""],
      LicenceType: [""],
      chkMediaType: ["", Validators.required],
      chkuserRights: ["", Validators.required],
      chkType: ["", Validators.required],
      TokenNoBkp: [""],
      DeviceId: [""],
      ScheduleType: [""],
      chkIndicator: [false],
      GroupId: [0],
      Rotation:["0"]
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
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
        }
        this.loading = false;
        //this.modalService.dismissAll('Cross click');
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
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
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
        }
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
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
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
        }
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
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
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  onChangeCountry(CountryID) {
    this.Country_Id = CountryID;
    this.FillState(CountryID);
  }
  FillState(CountryID) {
    this.loading = true;
    var qry = "select stateid as id, statename as displayname  from tbstate where countryid = " + CountryID + " order by statename";
    this.tService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.StateList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  onChangeState(StateID) {
    var ArrayItem = {};
    ArrayItem["Id"] = StateID;
    ArrayItem["DisplayName"] = "";
    this.NewFilterList = [];
    this.GetJSONRecord(ArrayItem, this.StateList);
    if (this.NewFilterList.length > 0) {
      this.ModifyStateName = this.NewFilterList[0].DisplayName;
    }
    this.ModifyStateId = StateID;
    this.FillCity(StateID);
  }
  FillCity(StateID) {
    this.loading = true;
    var qry = "select cityid as id, cityname as displayname  from tbcity where stateid = " + StateID + " order by cityname";
    this.tService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.CityList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
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
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
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
          GroupId:[objTokenData[0].GroupId],
          Rotation:[objTokenData[0].Rotation]
        });
        this.chkIndicatorBox=objTokenData[0].Indicator;
         
        this.ClientId = objTokenData[0].ClientId;
        this.ModifyStateId = objTokenData[0].state;
        this.ModifyCityId = objTokenData[0].city;
        this.ModifyStateName = objTokenData[0].StateName;
        this.citName = objTokenData[0].CityName;

        this.ModifyGroupId = objTokenData[0].GroupId;
        this.ModifyGroupName = objTokenData[0].GroupName;
        this.FillGroup();
        this.loading = false;
        this.onChangeSchedule(objTokenData[0].ScheduleType);

      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
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
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
        }
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }

  onChangeSchedule(schType) {
    var schItem = {};
    this.loading = true;
    this.ModifySchList = [];
    if (schType == "Normal") {
      this.scheduleList.forEach(item => {
        schItem = {};
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
        schItem = {};
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

  openCommonModal(modal, ModalType) {
    this.ModalType = ModalType;
    if (ModalType == 'State') {
      this.HeaderText = "State";
      if (this.Country_Id == "0") {
        this.toastr.error("Please select a county");
        return;
      }
      this.NewName = this.ModifyStateName;
      this.CommonId = this.ModifyStateId;
      this.modalService.open(modal);
    }
    if (ModalType == 'City') {
      if (this.ModifyStateId == "0") {
        this.toastr.error("Please select a state");
        return;
      }
      this.HeaderText = "City";
      this.NewName = this.citName;
      this.CommonId = this.ModifyCityId;
      this.modalService.open(modal);
    }
    if (ModalType == 'Group') {

      this.HeaderText = "Group";
      this.NewName = this.ModifyGroupName;
      this.CommonId = this.ModifyGroupId;
      this.modalService.open(modal);
    }
  }
  NewFilterList;
  GetJSONRecord = (array, list): void => {
    this.NewFilterList = list.filter(order => order.Id == array.Id);
  }

  onSubmitModal() {
    this.loading = true;
    this.tService.CitySateNewModify(this.CommonId, this.NewName, this.ModalType, this.ModifyStateId, this.Country_Id, this.ClientId).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Saved", 'Success!');
          this.loading = false;
          if (this.ModalType == 'State') {
            this.FillState(this.Country_Id);
            this.ModifyStateName = this.NewName;
          }
          if (this.ModalType == 'City') {
            this.FillCity(this.ModifyStateId);
            this.citName = this.NewName;
          }
          if (this.ModalType == 'Group') {
            this.FillGroup();
            this.ModifyGroupName = this.NewName;
          }

        }
        else if (obj.Responce == "-2") {
          this.toastr.info("Name is already exixts", '');
          this.loading = false;
        }
        else {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
          return;
        }
        this.NewName = "";
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  onChangeCity(Id) {
    var ArrayItem = {};
    ArrayItem["Id"] = Id;
    ArrayItem["DisplayName"] = "";
    this.NewFilterList = [];
    this.GetJSONRecord(ArrayItem, this.CityList);
    if (this.NewFilterList.length > 0) {
      this.citName = this.NewFilterList[0].DisplayName;
    }
    this.ModifyCityId = Id;
  }
  onChangeGroup(Id) {
    var ArrayItem = {};
    ArrayItem["Id"] = Id;
    ArrayItem["DisplayName"] = "";
    this.NewFilterList = [];
    this.GetJSONRecord(ArrayItem, this.GroupList);
    if (this.NewFilterList.length > 0) {
      this.ModifyGroupName = this.NewFilterList[0].DisplayName;
    }
    this.ModifyGroupId = Id;
  }
  FillGroup() {
    this.loading = true;
    var qry = "select GroupId as id, GroupName as displayname  from tbGroup where dfClientId = "+this.ClientId+" order by GroupName";
    this.tService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.GroupList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
}
