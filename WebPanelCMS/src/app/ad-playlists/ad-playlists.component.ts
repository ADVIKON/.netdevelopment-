import { Component, OnInit ,ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StoreForwardService } from '../store-and-forward/store-forward.service';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-ad-playlists',
  templateUrl: './ad-playlists.component.html',
  styleUrls: ['./ad-playlists.component.css']
})
export class AdPlaylistsComponent implements OnInit {
  ScheduleList = [];
  dropdownSettings = {};
  dropdownList = [];
  selectedItems = [];
  Plform: FormGroup;
  submitted = false;
  public loading = false;
  TokenSelected = [];
  TokenList = [];
  CustomerList: any[];
  PlaylistList = [];
  SearchTokenList = [];
  FormatList = [];
  page: number = 1;
  pageSize: number = 50;
  pageSearch: number = 1;
  pageSizeSearch: number = 20;
  dt = new Date();
  dt2 = new Date();
  cmbSearchCustomer = 0;
  cmbSearchToken = 0;
  cmbSearchPlaylist = 0;
  TokenInfoModifyPlaylist: FormGroup;
  pSchid = 0;
  IsAdminLogin: boolean = false;
   
  constructor(private formBuilder: FormBuilder, public toastr: ToastrService, vcr: ViewContainerRef,
    config: NgbModalConfig, private modalService: NgbModal, private sfService: StoreForwardService) {
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    var cd = new Date();
    if (localStorage.getItem('dfClientId') == "6") {
      this.IsAdminLogin = true;
    }
    else {
      this.IsAdminLogin = false;
    }
     

    this.Plform = this.formBuilder.group({
      CustomerId: ["0", Validators.required],
      FormatId: ["0", Validators.required],
      PlaylistId: ["0", Validators.required],
      sDate: [cd, Validators.required],
      eDate: [cd, Validators.required],
      startTime: [this.dt, Validators.required],
      EndTime: [this.dt2, Validators.required],
      pMode: ["Minutes"],
      TotalFrequancy: [0],
      wList: [this.selectedItems, Validators.required],
      TokenList: [this.TokenSelected]
    });
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
        if (this.IsAdminLogin == true) {
          this.FillFormat();
        }
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  FillFormat() {
    var q = "";
    q = "select Formatid  as id , formatname as displayname from tbSpecialFormat  where Formatid not in (46,45,1,59,25,9,6,13,4,15,44,35,30,17,47,8,22,66) order by formatname ";
    this.loading = true;
    this.sfService.FillCombo(q).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        
        this.FormatList = JSON.parse(returnData);
        
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  onChangeFormat(id) {
    this.ScheduleList = [];
    this.PlaylistList = [];
    this.FillPlaylist(id);
  }
  FillPlaylist(id) {
    this.loading = true;
    this.sfService.Playlist(id).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.PlaylistList = JSON.parse(returnData);
        this.loading = false;

      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  onChangeCustomer(deviceValue) {

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
            this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
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
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  onChangeSearchToken(id){
   
    this.SearchContent();
  }
  SearchContent() {
    if (this.cmbSearchCustomer == 0) {
      this.toastr.error("Select a customer", '');
      return;
    }
    this.ScheduleList=[]
    this.loading = true;
     
    this.sfService.FillAdPlaylist(this.cmbSearchCustomer, this.cmbSearchToken).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.ScheduleList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  onChangeSearchCustomer(id) {
    this.ScheduleList = [];
    this.loading = true;
    this.sfService.FillTokenInfo(id).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.SearchTokenList = JSON.parse(returnData);
        this.loading = false;
        this.SearchContent();
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  get f() { return this.Plform.controls; }

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
  
  onSubmitPL  = function () {
    
    this.submitted = true;
    if(this.Plform.value.CustomerId == 0){
      this.toastr.error("Please select a customer name");
      return;
    }
    if(this.Plform.value.FormatId == 0){
      this.toastr.error("Please select a format name");
      return;
    }
    if(this.Plform.value.PlaylistId == 0){
      this.toastr.error("Please select a playlist name");
      return;
    }
    if(this.Plform.value.TotalFrequancy == 0){
      this.toastr.error("Frequancy should be grater than zero");
      return;
    }
    if(this.Plform.value.wList.length == 0){
      this.toastr.error("Please select a Week Day");
      return;
    }
    if (this.TokenSelected.length == 0) {
      this.toastr.error("Select atleast one token", '');
      return;
    }
    
    
    if (this.Plform.invalid) {
      return;
    }
    var startTime = new Date(this.Plform.controls["startTime"].value).getHours();
    var EndTime = new Date(this.Plform.controls["EndTime"].value).getHours();
    if (EndTime < startTime) {
     // this.toastr.error("End time should be greater than start time");
     // return;
    }
    
    


    var sTime = new Date(this.Plform.value.startTime);
    var eTime = new Date(this.Plform.value.EndTime);
    this.Plform.get('startTime').setValue(sTime.toTimeString().slice(0, 5));
    this.Plform.get('EndTime').setValue(eTime.toTimeString().slice(0, 5));
 
    this.loading = true;
    this.sfService.SaveAdPlaylist(this.Plform.value).pipe()
    .subscribe(data => {
      var returnData = JSON.stringify(data);
      var obj = JSON.parse(returnData);
      if (obj.Responce == "1") {
        this.toastr.info("Saved", 'Success!');
        this.loading = false;
       this.clear();
        this.SaveModifyInfo(0,"Playlist is schedule for advertisement");
      }
      else {
        this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
        this.loading = false;
      }
    },
      error => {
        this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
        this.loading = false;
      })
  }
  clear(){
    var cd = new Date();
    this.selectedItems=[];
    this.TokenSelected=[];
    this.Plform.get('sDate').setValue(cd);
    this.Plform.get('eDate').setValue(cd);
    this.Plform.get('pMode').setValue("Minutes");
    this.Plform.get('TotalFrequancy').setValue(0);

    this.PlaylistList=[];
    this.FormatList=[];
    this.dropdownList=[];
    this.Plform.get('CustomerId').setValue("0");
    this.Plform.get('FormatId').setValue("0");
    this.Plform.get('PlaylistId').setValue("0");
    this.Plform.get('wList').setValue('');
    this.TokenList=[];

    this.dropdownList = [
      { "id": "1", "itemName": "Monday" },
      { "id": "2", "itemName": "Tuesday" },
      { "id": "3", "itemName": "Wednesday" },
      { "id": "4", "itemName": "Thursday" },
      { "id": "5", "itemName": "Friday" },
      { "id": "6", "itemName": "Saturday" },
      { "id": "7", "itemName": "Sunday" }
    ];
  }
  SaveModifyInfo(tokenid, ModifyText){
  
    this.sfService.SaveModifyLogs(tokenid, ModifyText).pipe()
    .subscribe(data => {
      var returnData = JSON.stringify(data);
    },
      error => {
      })
  };
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
