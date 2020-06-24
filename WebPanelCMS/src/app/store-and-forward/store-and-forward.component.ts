import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StoreForwardService } from '../store-and-forward/store-forward.service';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-store-and-forward',
  templateUrl: './store-and-forward.component.html',
  styleUrls: ['./store-and-forward.component.css']
})
export class StoreAndForwardComponent implements OnInit {
  ScheduleList = [];
  dropdownSettings = {};

  dropdownList = [];
  selectedItems = [];
  SFform: FormGroup;
  submitted = false;
  public loading = false;
  TokenSelected = [];
  TokenList = [];
  MainTokenList = [];
  CustomerList: any[];
  PlaylistList = [];
  SearchFormatList = [];
  FormatList = [];
  page: number = 1;
  pageSize: number = 50;
  pageSearch: number = 1;
  pageSizeSearch: number = 20;
  dt = new Date('Mon Mar 09 2020 00:00:00');
  dt2 = new Date('Mon Mar 09 2020 23:59:00');
  cmbSearchCustomer = 0;
  cmbSearchFormat = 0;
  cmbSearchPlaylist = 0;
  frmTokenInfoModifyPlaylist: FormGroup;
  pSchid = 0;
  
  cid;
  CountryList = [];
  CountrySettings = {};
  StateList = [];
  StateSettings = {};
  CityList = [];
  CitySettings = {};
  GroupList = [];
  GroupSettings = {};
 
   
  constructor(private formBuilder: FormBuilder, public toastrSF: ToastrService, private vcr: ViewContainerRef,
    config: NgbModalConfig, private modalService: NgbModal, private sfService: StoreForwardService,
    public auth:AuthService) {
     
    config.backdrop = 'static';
    config.keyboard = false;
  }
  // d = new Date();
  // year = this.d.getHours();
  //   month = this.d.getMonth();
  // day = this.d.getDate();
  // hr= this.d.getHours();
  // public dateTime1 = new Date(this.year,this.month,this.day,this.hr,0,0,0);
  ngOnInit() {

     
 

    this.SFform = this.formBuilder.group({

      CustomerId: ["0", Validators.required],
      FormatId: ["0", Validators.required],
      PlaylistId: ["0", Validators.required],
      startTime: [this.dt, Validators.required],
      EndTime: [this.dt2, Validators.required],
      wList: [this.selectedItems, Validators.required],
      TokenList: [this.TokenSelected]
    });
    this.frmTokenInfoModifyPlaylist = this.formBuilder.group({
      ModifyPlaylistName: [""],
      ModifyStartTime: [""],
      ModifyEndTime: [""],
      pschid: [""]
    });
    this.ScheduleList = [];
    
    this.TokenList = [];
    this.selectedItems = [];
    this.dropdownList = [
      { "id": "1", "itemName": "Monday" },
      { "id": "2", "itemName": "Tuesday" },
      { "id": "3", "itemName": "Wednesday" },
      { "id": "4", "itemName": "Thursday" },
      { "id": "5", "itemName": "Friday" },
      { "id": "6", "itemName": "Saturday" },
      { "id": "7", "itemName": "Sunday" }
    ];
    this.dropdownSettings = {
      singleSelection: false,
      text: "Select Week",
      idField: 'id',
      textField: 'itemName',
      selectAllText: 'Week',
      unSelectAllText: 'Week',
      itemsShowLimit: 4
    };
    
    this.FillClient();


    


  }

  onItemSelect(item: any) {


  }
  onSelectAll(items: any) {

  }
  get f() { return this.SFform.controls; }

  onSubmitSF() {

    this.submitted = true;
    if (this.SFform.value.CustomerId == "0") {
      this.toastrSF.error("Please select a customer name");
      return;
    }
    if (this.SFform.value.FormatId == "0") {
      this.toastrSF.error("Please select a format name");
      return;
    }
    if (this.SFform.value.PlaylistId == "0") {
      this.toastrSF.error("Please select a playlist name");
      return;
    }


    if (this.SFform.invalid) {
      // return;
    }
    var startTime = new Date(this.SFform.controls["startTime"].value).getHours();
    var EndTime = new Date(this.SFform.controls["EndTime"].value).getHours();
    if (EndTime < startTime) {
      this.toastrSF.error("End time should be greater than start time");
      return;
    }
    if (this.SFform.value.wList.length == 0) {
      this.toastrSF.error("Please select a week day");
      return;

    }
    if (this.TokenSelected.length == 0) {
      this.toastrSF.error("Please select atleast one location", '');
      return;
    }

    this.SFform.controls["TokenList"].setValue(this.TokenSelected);
    var sTime = new Date(this.SFform.value.startTime);
    var eTime = new Date(this.SFform.value.EndTime);
    this.SFform.get('startTime').setValue(sTime.toTimeString().slice(0, 5));
    this.SFform.get('EndTime').setValue(eTime.toTimeString().slice(0, 5));


    this.loading = true;
    this.sfService.SaveSF(this.SFform.value).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastrSF.info("Saved", 'Success!');
          this.loading = false;
          this.SFform.get('startTime').setValue(sTime);
          this.SFform.get('EndTime').setValue(eTime);
          this.SaveModifyInfo(0, "New schedule is created");
        }
        else {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        }
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })

  }

  SelectToken(fileid, event, scheduleType) {
    var tokenItem = {};
    if (event.target.checked) {
      tokenItem["tokenId"] = fileid;
      tokenItem["schType"] = scheduleType;
      this.TokenSelected.push(tokenItem);
    }
    else {
      tokenItem["tokenId"] = fileid;
      tokenItem["schType"] = scheduleType;
      this.removeDuplicateRecord(tokenItem);
      //      const index: number = this.TokenSelected.indexOf(fileid);
      //      if (index !== -1) {
      //        this.TokenSelected.splice(index, 1);
      //     }
    }

  }
  removeDuplicateRecord = (array): void => {
    this.TokenSelected = this.TokenSelected.filter(order => order.tokenId !== array.tokenId);
  }
  FillClient() {
    var q = "";
    var i = this.auth.IsAdminLogin$.value ? 1 : 0; 
    q = "FillCustomer " + i + ", " + localStorage.getItem('dfClientId') + "," + localStorage.getItem('DBType');

    this.loading = true;
    this.sfService.FillCombo(q).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.CustomerList = JSON.parse(returnData);
        this.loading = false;
        if (this.auth.IsAdminLogin$.value == true) {
          this.FillFormat();
        }

      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  FillFormat() {
    var q = "";


    q = "FillFormat 0,'"+ localStorage.getItem('DBType') +"'";



    this.loading = true;
    this.sfService.FillCombo(q).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.FormatList = JSON.parse(returnData);
        this.SearchFormatList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }

  onChangeFormat(id, type) {
    this.ScheduleList = [];
    this.PlaylistList = [];
    this.FillPlaylist(id, type);
  }

  FillPlaylist(id, type) {
    this.loading = true;
    this.sfService.Playlist(id).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.PlaylistList = JSON.parse(returnData);
        this.loading = false;
        if (type == "Search") {
          this.SearchContent();
        }
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  onChangeCustomer(deviceValue) {
    this.cid = deviceValue;
    this.TokenSelected = [];
    this.SelectedGroupArray=[];
    this.SelectedCountryArray=[];
    this.SelectedStateArray=[];
    this.SelectedCityArray=[];
    if (this.auth.IsAdminLogin$.value == false) {
      var q = "select max(sf.Formatid) as id , sf.formatname as displayname from tbSpecialFormat sf left join tbSpecialPlaylistSchedule_Token st on st.formatid= sf.formatid";
      q = q + " left join tbSpecialPlaylistSchedule sp on sp.pschid= st.pschid  where (dbtype='"+ localStorage.getItem('DBType') +"' or dbtype='Both') and  (st.dfclientid=" + deviceValue + " OR sf.dfclientid=" + deviceValue + ") group by  sf.formatname";

      this.loading = true;
      this.sfService.FillCombo(q).pipe()
        .subscribe(data => {
          var returnData = JSON.stringify(data);
          this.FormatList = JSON.parse(returnData);
          this.loading = false;
          this.FillTokenInfo(deviceValue);

        },
          error => {
            this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
            this.loading = false;
          })
    }
    else {
      this.FillTokenInfo(deviceValue);
    }

  }
  FillTokenInfo(deviceValue) {
    this.loading = true;
    this.sfService.FillTokenInfo(deviceValue).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.TokenList = JSON.parse(returnData);
        this.MainTokenList = this.TokenList;
        this.loading = false;
        this.getSelectedRows();
        this.FillCountry();
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  allToken(event) {
    var tokenItem = {};
    const checked = event.target.checked;
    this.TokenSelected = [];
    this.TokenList.forEach(item => {
      tokenItem = {};
      item.check = checked;
      tokenItem["tokenId"] = item.tokenid;
      tokenItem["schType"] = item.ScheduleType;
      this.TokenSelected.push(tokenItem)
    });
    if (checked == false) {
      this.TokenSelected = [];
    }


  }
  onChangeSearchPlaylist() {
    this.SearchContent();
  }
  SearchContent() {
    if (this.cmbSearchCustomer == 0) {
      this.toastrSF.error("Select a customer", '');
      return;
    }
    this.loading = true;
    this.sfService.FillSF(this.cmbSearchCustomer, this.cmbSearchFormat, this.cmbSearchPlaylist).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.ScheduleList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  openModal(content, pname, pschid, stime, eTime) {
    var t = "1900-01-01 " + stime;
    var t2 = "1900-01-01 " + eTime;
    var dt = new Date(t);
    var dt2 = new Date(t2);


    this.frmTokenInfoModifyPlaylist = this.formBuilder.group({
      ModifyPlaylistName: [pname],
      ModifyStartTime: [dt],
      ModifyEndTime: [dt2],
      pschid: [pschid]
    });
    this.modalService.open(content, { centered: true });
  }
  onSubmitTokenInfoModifyPlaylist() {
    //this.loading = true;
    var sTime = new Date(this.frmTokenInfoModifyPlaylist.value.ModifyStartTime);
    var eTime = new Date(this.frmTokenInfoModifyPlaylist.value.ModifyEndTime);
    var pschid = this.frmTokenInfoModifyPlaylist.value.pschid;
    this.sfService.UpdateTokenSch(pschid, sTime.toTimeString().slice(0, 5), eTime.toTimeString().slice(0, 5)).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastrSF.info("Saved", 'Success!');
          this.SearchContent();
          this.SaveModifyInfo(0, "Token schedule time is modify and schedule id is " + pschid);
        }
        else {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
        }
        this.loading = false;
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  openDeleteModal(content, pschid) {
    this.pSchid = pschid;
    this.modalService.open(content, { centered: true });
  }
  DeleteTokenSchedule() {
    this.loading = true;
    this.sfService.DeleteTokenSch(this.pSchid).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastrSF.info("Deleted", 'Success!');
          this.loading = false;
          this.SearchContent();
          this.SaveModifyInfo(0, "Token schedule time is delete and schedule id is " + this.pSchid);

        }
        else {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
        }
        this.loading = false;
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  onChangeSearchCustomer(id) {
    this.ScheduleList = [];
    var q = "select max(sf.Formatid) as id , sf.formatname as displayname from tbSpecialFormat sf left join tbSpecialPlaylistSchedule_Token st on st.formatid= sf.formatid";
    q = q + " left join tbSpecialPlaylistSchedule sp on sp.pschid= st.pschid  where (dbtype='"+ localStorage.getItem('DBType') +"' or dbtype='Both') and  (st.dfclientid=" + id + " OR sf.dfclientid=" + id + ") group by  sf.formatname";

    this.loading = true;
    this.sfService.FillCombo(q).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.SearchFormatList = JSON.parse(returnData);
        this.loading = false;
        this.SearchContent();
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }

  SaveModifyInfo(tokenid, ModifyText) {

    this.sfService.SaveModifyLogs(tokenid, ModifyText).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
      },
        error => {
        })
  };
  open(content, tid) {
    localStorage.setItem("tokenid", tid);
    this.modalService.open(content, { size: 'lg' });
  }
  tokenInfoClose() {
    this.modalService.dismissAll();
    this.FillTokenInfo(this.cid);
     
  }
  getSelectedRows() {
    this.TokenList.forEach(itemList => {
      if (this.FindRecords(itemList.tokenid).length != 0) {
        itemList.check = true;
      }
    });
  }
  FindRecords(Id) {
    var NewList = [];
    NewList = this.TokenSelected.filter(order => order.tokenId == Id);
    return NewList;
  }
  FillCountry() {
    this.CountrySettings = {
      singleSelection: false,
      text: "Select Country",
      idField: 'Id',
      textField: 'DisplayName',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3
    };
    this.CountryList = [];

    this.loading = true;
    var qry = "select distinct  countrycodes.countrycode as Id, countrycodes.countryname  as DisplayName from AMPlayerTokens";
    qry += "  inner join countrycodes on countrycodes.countrycode =  AMPlayerTokens.countryid ";
    qry += " where clientid = " + this.cid;
    this.sfService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.CountryList = JSON.parse(returnData);
        this.loading = false;
        this.FillGroup();
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }

  SelectedCountryArray = [];

  onItemSelectCountry(item: any) {
    //this.SelectedCountryArray.push(item);
    this.SelectedStateArray = [];
    this.SelectedCityArray = [];
    if (this.SelectedCountryArray.length == 0) {
      this.StateList = [];
      this.CityList = [];
      this.SelectedCountryArray = [];
      return;
    }
    var ret = this.ReturnFncAddId(this.SelectedCountryArray);
    this.FillState(ret);
  }
  onItemDeSelectCountry(item: any) {
    this.TokenList = [];
    this.SelectedStateArray = [];
    this.SelectedCityArray = [];
    this.SelectedCountryArray = this.removeDuplicateRecordFilter(item, this.SelectedCountryArray);
    if (this.SelectedCountryArray.length == 0) {
      this.StateList = [];
      this.SelectedCountryArray = [];
      this.TokenList = this.MainTokenList;
      return;
    }
    var ret = this.ReturnFncAddId(this.SelectedCountryArray);
    this.FillState(ret);
    var FilterValue = this.ReturnFncAddId(this.SelectedCountryArray);
    this.FilterTokenInfo(FilterValue, 'CountryId');
  }
  removeDuplicateRecordFilter(array, SelectedArray) {
    return SelectedArray = SelectedArray.filter(order => order.Id !== array.Id);

  }

  onSelectAllCountry(items: any) {
    this.SelectedStateArray = [];
    this.SelectedCityArray = [];
    this.SelectedCountryArray = items;
    this.TokenList = [];
    if (this.SelectedCountryArray.length == 0) {
      this.StateList = [];
      this.SelectedCountryArray = [];
      this.TokenList = this.MainTokenList;
      return;
    }
    var ret = this.ReturnFncAddId(this.SelectedCountryArray);
    this.FillState(ret);
    var FilterValue = this.ReturnFncAddId(this.SelectedCountryArray);
    this.FilterTokenInfo(FilterValue, 'CountryId');
  }
  onDeSelectAllCountry(items: any) {
    this.StateList = [];
    this.CityList = [];
    this.SelectedCountryArray = [];
    this.SelectedStateArray = [];
    this.SelectedCityArray = [];
    this.TokenList = [];
    this.TokenList = this.MainTokenList;
  }











  FillState(CountryID) {
    this.StateSettings = {
      singleSelection: false,
      text: "Select State",
      idField: 'Id',
      textField: 'DisplayName',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3
    };
    this.loading = true;
    var qry = "select stateid as id, statename as displayname  from tbstate where countryid in( " + CountryID + " ) order by statename";
    this.sfService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.StateList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  SelectedStateArray = [];
  onItemSelectState(item: any) {
    this.TokenList = [];
    this.SelectedCityArray = [];
    //this.SelectedStateArray.push(item);
    if (this.SelectedStateArray.length == 0) {
      this.TokenList = this.MainTokenList;
      this.CityList = [];
      this.SelectedStateArray = [];
      return;
    }
    var k = this.ReturnFncAddId(this.SelectedStateArray);
    this.FillCity(k);
    var FilterValue = this.ReturnFncAddId(this.SelectedStateArray);
    this.FilterTokenInfo(FilterValue, 'StateId');
  }
  onItemDeSelectState(item: any) {
    this.TokenList = [];
    this.SelectedCityArray = [];
    this.SelectedStateArray = this.removeDuplicateRecordFilter(item, this.SelectedStateArray);
    if (this.SelectedStateArray.length == 0) {
      this.CityList = [];
      this.SelectedStateArray = [];
      this.TokenList = this.MainTokenList;
      return;
    }
    var ret = this.ReturnFncAddId(this.SelectedStateArray);
    this.FillCity(ret);
    var FilterValue = this.ReturnFncAddId(this.SelectedStateArray);
    this.FilterTokenInfo(FilterValue, 'StateId');

  }
  onSelectAllState(items: any) {
    this.SelectedStateArray = items;
    this.SelectedCityArray = [];
    this.TokenList = [];
    
    if (this.SelectedStateArray.length == 0) {
      this.CityList = [];
      this.SelectedStateArray = [];
      this.TokenList = this.MainTokenList;
      return;
    }
    var ret = this.ReturnFncAddId(this.SelectedStateArray);
     this.FillCity(ret);
    var FilterValue = this.ReturnFncAddId(this.SelectedStateArray);
    this.FilterTokenInfo(FilterValue, 'StateId');
  }

  onDeSelectAllState(items: any) {
    this.CityList = [];
    this.SelectedStateArray = [];
    this.SelectedCityArray = [];
    this.TokenList = [];
    this.TokenList = this.MainTokenList;
  }
  FillCity(StateID) {
    this.CitySettings = {
      singleSelection: false,
      text: "Select City",
      idField: 'Id',
      textField: 'DisplayName',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      defaultOpen:true
    };
    this.loading = true;
    var qry = "select distinct cityid as id, cityname as displayname  from tbcity where stateid in(" + StateID + ") order by cityname";
    console.log(qry);
    this.sfService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.CityList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  SelectedCityArray = [];
  onItemSelectCity(item: any) {
    this.TokenList = [];
   // this.SelectedCityArray.push(item);
    if (this.SelectedCityArray.length == 0) {
      this.SelectedCityArray = [];
      this.TokenList = this.MainTokenList;
      return;
    }
    var FilterValue = this.ReturnFncAddId(this.SelectedCityArray);
    this.FilterTokenInfo(FilterValue, 'CityId');
  }
  onItemDeSelectCity(item: any) {
    this.TokenList = [];
    this.SelectedCityArray = this.removeDuplicateRecordFilter(item, this.SelectedCityArray);
    if (this.SelectedCityArray.length == 0) {
      this.SelectedCityArray = [];
      this.TokenList = this.MainTokenList;
      return;
    }
    var FilterValue = this.ReturnFncAddId(this.SelectedCityArray);
    this.FilterTokenInfo(FilterValue, 'CityId');
  }
  onSelectAllCity(items: any) {
    this.SelectedCityArray = items;
    this.TokenList = [];
    
    if (this.SelectedCityArray.length == 0) {
      this.SelectedCityArray = [];
      this.TokenList = this.MainTokenList;
      return;
    }
    var FilterValue = this.ReturnFncAddId(this.SelectedCityArray);
    this.FilterTokenInfo(FilterValue, 'CityId');

  }

  onDeSelectAllCity(items: any) {
    this.SelectedCityArray = [];
    this.TokenList = [];
    this.TokenList = this.MainTokenList;
  }
  ReturnFncAddId(ArrayList) {
    var ReturnId = [];
    for (var i = 0; i < ArrayList.length; i++) {
      ReturnId.push(ArrayList[i].Id);
    }
    return ReturnId;
  }



  SelectedGroupArray = [];
  FillGroup() {
    this.GroupSettings = {
      singleSelection: false,
      text: "Select City",
      idField: 'Id',
      textField: 'DisplayName',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3
    };
    this.loading = true;
    var qry = "select GroupId as id, GroupName as displayname  from tbGroup where dfClientId in( " + this.cid + " ) order by GroupName";
    this.sfService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.GroupList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }

  onItemSelectGroup(item: any) {
    //this.SelectedGroupArray.push(item);
    this.TokenList = [];
    if (this.SelectedGroupArray.length == 0) {
      this.SelectedGroupArray = [];
      this.TokenList = this.MainTokenList;
      return;
    }
    var FilterValue = this.ReturnFncAddId(this.SelectedGroupArray);
    this.FilterTokenInfo(FilterValue, 'GroupId');

  }
  onItemDeSelectGroup(item: any) {
    this.TokenList = [];
    this.SelectedGroupArray = this.removeDuplicateRecordFilter(item, this.SelectedGroupArray);
    if (this.SelectedGroupArray.length == 0) {
      this.TokenList = this.MainTokenList;
      this.SelectedGroupArray = [];
      return;
    }
    var FilterValue = this.ReturnFncAddId(this.SelectedGroupArray);
    this.FilterTokenInfo(FilterValue, 'GroupId');

  }
  onSelectAllGroup(items: any) {
    this.SelectedGroupArray = items;
    this.TokenList = [];
    if (this.SelectedGroupArray.length == 0) {
      this.TokenList = this.MainTokenList;
      this.SelectedGroupArray = [];
      return;
    }
    var FilterValue = this.ReturnFncAddId(this.SelectedGroupArray);
    
    this.FilterTokenInfo(FilterValue, 'GroupId');

  }
  onDeSelectAllGroup(items: any) {
    this.SelectedGroupArray = [];
    this.TokenList = [];
    this.TokenList = this.MainTokenList;

  }

  FilterTokenInfo(FilterValue, FilterId) {
    var ObjLocal;
    for (var counter = 0; counter < FilterValue.length; counter++) {
      if (FilterId == "CountryId") {
        ObjLocal = this.MainTokenList.filter(order => order.CountryId == FilterValue[counter]);
      }
      if (FilterId == "StateId") {
        ObjLocal = this.MainTokenList.filter(order => order.StateId == FilterValue[counter]);
      }
      if (FilterId == "CityId") {
        ObjLocal = this.MainTokenList.filter(order => order.CityId == FilterValue[counter]);
      }
      if (FilterId == "GroupId") {
        ObjLocal = this.MainTokenList.filter(order => order.GroupId == FilterValue[counter]);
      }
      if (ObjLocal.length > 0) {
        ObjLocal.forEach((obj) => {
          this.TokenList.push(obj);
        });
      }
    }
  }

}

