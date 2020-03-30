import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng6-toastr';
//import * as _moment from 'moment';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StoreForwardService } from '../store-and-forward/store-forward.service';
import { retry } from 'rxjs/operators';
//const moment = (_moment as any).default ? (_moment as any).default : _moment;

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
  CustomerList: any[];
  PlaylistList = [];
  SearchFormatList = [];
  FormatList = [];
  page: number = 1;
  pageSize: number = 50;
  pageSearch: number = 1;
  pageSizeSearch: number = 20;
  dt = new Date('Mon Mar 09 2020 00:00:00 GMT+0530 (India Standard Time)');
  dt2 = new Date('Mon Mar 09 2020 00:00:00 GMT+0530 (India Standard Time)');
  cmbSearchCustomer = 0;
  cmbSearchFormat = 0;
  cmbSearchPlaylist = 0;
  frmTokenInfoModifyPlaylist: FormGroup;
  pSchid = 0;
  IsAdminLogin: boolean = false;
  cid;
  constructor(private formBuilder: FormBuilder, public toastrSF: ToastsManager,private vcr: ViewContainerRef,
    config: NgbModalConfig, private modalService: NgbModal, private sfService: StoreForwardService) {
    this.toastrSF.setRootViewContainerRef(vcr);
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    if (localStorage.getItem('dfClientId') == "6") {
      this.IsAdminLogin = true;
    }
    else {
      this.IsAdminLogin = false;
    }
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
    this.dropdownList = [
      { "id": "1", "itemName": "Monday" },
      { "id": "2", "itemName": "Tuesday" },
      { "id": "3", "itemName": "Wednesday" },
      { "id": "4", "itemName": "Thursday" },
      { "id": "5", "itemName": "Friday" },
      { "id": "6", "itemName": "Saturday" },
      { "id": "7", "itemName": "Sunday" }
    ];
    this.TokenList = [];
    this.selectedItems = [];
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
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        }
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
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
    if (this.IsAdminLogin == true) {
      q = "select DFClientID as id,  ClientName as displayname from DFClients where CountryCode is not null and DFClients.IsDealer=1 order by RIGHT(ClientName, LEN(ClientName) - 3)";
    }
    else {
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
        if (this.IsAdminLogin == true) {
          this.FillFormat();
        }
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  FillFormat() {
    var q = "";


    q = "FillFormat";



    this.loading = true;
    this.sfService.FillCombo(q).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.FormatList = JSON.parse(returnData);
        this.SearchFormatList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
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
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onChangeCustomer(deviceValue) {
    this.cid = deviceValue;
    this.TokenSelected=[];
    if (this.IsAdminLogin == false) {
      var q = "select max(sf.Formatid) as id , sf.formatname as displayname from tbSpecialFormat sf left join tbSpecialPlaylistSchedule_Token st on st.formatid= sf.formatid";
      q = q + " left join tbSpecialPlaylistSchedule sp on sp.pschid= st.pschid  where (st.dfclientid=" + deviceValue + " OR sf.dfclientid=" + deviceValue + ") group by  sf.formatname";

      this.loading = true;
      this.sfService.FillCombo(q).pipe()
        .subscribe(data => {
          var returnData = JSON.stringify(data);
          this.FormatList = JSON.parse(returnData);
          this.loading = false;
          this.FillTokenInfo(deviceValue);
        },
          error => {
            this.toastrSF.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
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
        this.loading = false;
        this.getSelectedRows();
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  allToken(event) {
   var  tokenItem={};
    const checked = event.target.checked;
    this.TokenSelected = [];
    this.TokenList.forEach(item => {
      tokenItem={};
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
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
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
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
        }
        this.loading = false;
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
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
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
        }
        this.loading = false;
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onChangeSearchCustomer(id) {
    this.ScheduleList = [];
    var q = "select max(sf.Formatid) as id , sf.formatname as displayname from tbSpecialFormat sf left join tbSpecialPlaylistSchedule_Token st on st.formatid= sf.formatid";
    q = q + " left join tbSpecialPlaylistSchedule sp on sp.pschid= st.pschid  where (st.dfclientid=" + id + " OR sf.dfclientid=" + id + ") group by  sf.formatname";

    this.loading = true;
    this.sfService.FillCombo(q).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.SearchFormatList = JSON.parse(returnData);
        this.loading = false;
        this.SearchContent();
      },
        error => {
          this.toastrSF.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
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
    this.toastrSF.setRootViewContainerRef(this.vcr);
  }
  getSelectedRows() {
    this.TokenList.forEach(itemList => {
      if (this.FindRecords(itemList.tokenid).length!=0){
        itemList.check= true;
      }
    });
  }
  FindRecords(Id) {
    var NewList=[];
    NewList = this.TokenSelected.filter(order => order.tokenId == Id);
    return NewList;
  }
  
}

