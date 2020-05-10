import { Component, OnInit, EventEmitter, ViewContainerRef } from '@angular/core';
import { FileUploader, FileLikeObject, FileItem, FileSelectDirective, } from 'ng2-file-upload';

import { ConfigAPI } from '../class/ConfigAPI';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { StreamService } from './stream.service';
@Component({
  selector: 'app-streaming',
  templateUrl: './streaming.component.html',
  styleUrls: ['./streaming.component.css']
})
export class StreamingComponent implements OnInit {
 
  StreamName = "";
  StreamLink = "";
  sImgPath = "";
   EditStreamId="0";
  CustomerId = "0";
  CustomerList: any[];
  cmbOwnerCustomer="0";
  cmbCopyCustomer="0";
  public loading = false;
  IsAdminLogin: boolean = true;
  IsEditStream:boolean=false;
  StreamList;
  
  UserId;
  chkDashboard;
  chkPlayerDetail;
  chkPlaylistLibrary;
  chkScheduling;
  chkAdvertisement;
  chkInstantPlay;
  TokenList: any[];
  page: number = 1;
  pageSize: number = 50;
  public uploader: FileUploader = new FileUploader({
    url: this.cf.UploadStreamImage,
    itemAlias: 'photo',
  });
  constructor(public toastr: ToastrService, vcr: ViewContainerRef, private cf: ConfigAPI,
    private serviceStream: StreamService, config: NgbModalConfig, private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
    this.uploader.onCompleteAll = () => {
     
      this.uploader = new FileUploader({
        url: this.cf.UploadStreamImage,
        itemAlias: 'photo',
      })
      
      this.toastr.info("Content Uploaded");
      this.FillStreamList(this.CustomerId);
      this.clear();
      // this.uploader.clearQueue();
      //  this.uploader.onProgressAll(0);
    };
  }
  FillClientList() {
    this.loading = true;
    var str = "";
    if (this.IsAdminLogin == true) {
      str = "select DFClientID as id,  ClientName as displayname from DFClients where CountryCode is not null and DFClients.IsDealer=1 order by RIGHT(ClientName, LEN(ClientName) - 3)";
    }
    else {
      str = "";
      str = "select DFClientID as id, ClientName  as displayname  from ( ";
      str = str + " select distinct DFClients.DFClientID,DFClients.ClientName from DFClients ";
      str = str + " inner join AMPlayerTokens on DFClients.DfClientid=AMPlayerTokens.Clientid ";
      str = str + " where DFClients.CountryCode is not null and DFClients.DealerDFClientID= " + localStorage.getItem('dfClientId') + "    ";
      str = str + " union all select distinct DFClients.DFClientID,DFClients.ClientName from DFClients ";
      str = str + " inner join AMPlayerTokens on DFClients.DfClientid=AMPlayerTokens.Clientid ";
      str = str + " where DFClients.CountryCode is not null and DFClients.MainDealerid= " + localStorage.getItem('dfClientId') + "    ";
      str = str + "   ) as a order by RIGHT(ClientName, LEN(ClientName) - 3) ";
    }

    this.serviceStream.FillCombo(str).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.CustomerList = JSON.parse(returnData);
        this.loading = false;
         this.StreamList=[];
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
   
   
  onChangeCustomer(id){
    this.uploader.clearQueue();
    this.IsEditStream=false;
    this.StreamName="";
    this.StreamLink="";
    this.FillStreamList(id);
     
  }
  ChooseFile(){
     
    this.uploader.clearQueue();
  }
  ngOnInit() {
    this.TokenList = [];
    this.uploader.onAfterAddingFile = (file) => { 
      file.withCredentials = false; 
      if (file._file.size>35850){
  this.toastr.info("File size limit is exceeded");
  this.uploader.clearQueue();
return;

      }
    };
    this.uploader.onCompleteItem = (item: any, response: any, status: any, headers: any) => {
      // console.log('ImageUpload:uploaded:', item, status, response);
      //8968680545-- rajinder singh-  Fast Tag

    };

    if (localStorage.getItem('dfClientId') == "6") {
      this.IsAdminLogin = true;
    }
    if (localStorage.getItem('dfClientId') != "6") {
      this.IsAdminLogin = false;
    }
    this.FillClientList();
    this.UserId= localStorage.getItem('UserId');
    this.chkDashboard=localStorage.getItem('chkDashboard');
    this.chkPlayerDetail=localStorage.getItem('chkPlayerDetail');
    this.chkPlaylistLibrary=localStorage.getItem('chkPlaylistLibrary');
    this.chkScheduling=localStorage.getItem('chkScheduling');
    this.chkAdvertisement=localStorage.getItem('chkAdvertisement');
    this.chkInstantPlay=localStorage.getItem('chkInstantPlay');
  }
  public hasBaseDropZoneOver: boolean = false;
  public hasAnotherDropZoneOver: boolean = false;

  fileObject: any;


  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }

  public onFileSelected(event: EventEmitter<File[]>) {

  }
  Upload() {
    if (this.CustomerId == "0") {
      this.toastr.info("Customer name cannot be blank");
      return;
    }
    if (this.StreamName == "") {
      this.toastr.info("Stream name cannot be blank");
      return;
    }
    if (this.StreamLink == "") {
      this.toastr.info("Stream link cannot be blank");
      return;
    }     
    this.uploader.onBuildItemForm = (fileItem: any, form: any) => {
      form.append('StreamName', this.StreamName);
      form.append('CustomerId', this.CustomerId);
      form.append('StreamLink', this.StreamLink);
    };
    this.uploader.uploadAll()
  }

     
  SaveModifyInfo(tokenid, ModifyText) {
    this.serviceStream.SaveModifyLogs(tokenid, ModifyText).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
      },
        error => {
        })
  };
  DeleteStream(){
    this.IsEditStream=false;
    this.loading = true;
    this.serviceStream.DeleteStream(this.EditStreamId).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        this.toastr.info("Stream Deleted", '');
       
        this.FillStreamList(this.CustomerId);
         this.clear();
        this.loading = false;
         
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onClickEditStream(id, sname, slink,imgPath){
this.IsEditStream=true;
this.EditStreamId=id,
this.StreamName=sname;
this.StreamLink= slink;
this.sImgPath=imgPath;
  }
  openStreamDeleteModal(mContent, id){
    this.EditStreamId = id;
    this.modalService.open(mContent, { centered: true });
  }
  RefreshStream(){
    this.IsEditStream=false;
    this.clear();
  }
  clear(){
    this.EditStreamId="0",
    this.StreamName="";
    this.StreamLink= "";
    this.sImgPath="";
    this.uploader.clearQueue();
  }
  UpdateStream(){
    this.IsEditStream=false;
    this.loading = true;
    this.serviceStream.UpdateStream(this.CustomerId, this.EditStreamId,this.StreamName,this.StreamLink, this.sImgPath).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        this.toastr.info("Update", '');
        
        this.FillStreamList(this.CustomerId);
        this.clear();
        this.loading = false;
         
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })

  }
  FillStreamList(id){
    this.loading = true;
    this.serviceStream.FillStreamList(id).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.StreamList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }











  onChangeOwnerCustomer(id){
    this.FillStreamList(id);
  }

  StreamSelected=[];
  allStream(event){
    const checked = event.target.checked;
    this.StreamSelected=[];
    this.StreamList.forEach(item=>{
      item.check = checked;
      this.StreamSelected.push(item.tokenid)
    });
    if (checked==false){
      this.StreamSelected=[];
    }
  }
  SelectStream(fileid, event) {
    if (event.target.checked) {
      this.StreamSelected.push(fileid);
    }
    else {
      const index: number = this.StreamSelected.indexOf(fileid);
      if (index !== -1) {
        this.StreamSelected.splice(index, 1);
      }
    }
  }
  TokenSelected=[];
  
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
  onChangeCopyCustomer(id){
    this.FillTokenInfo(id);
  }
  FillTokenInfo(deviceValue) {
    this.loading = true;
    this.serviceStream.FillTokenInfo(deviceValue).pipe()
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
  AssignStream(){
    if (this.StreamSelected.length == 0) {
      this.toastr.info("Please select a stream");
      return;
    }
    if (this.TokenSelected.length == 0) {
      this.toastr.info("Please select a token");
      return;
    }
    this.loading = true;
    this.serviceStream.AssignStream(this.cmbOwnerCustomer,this.TokenSelected,this.StreamSelected).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Saved", 'Success!');
          this.loading = false;
          this.cmbOwnerCustomer = "0";
          this.StreamSelected = [];
          this.StreamList = [];
          this.TokenList = [];
          this.TokenSelected = [];
          this.cmbCopyCustomer="0";
        }
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }

}
