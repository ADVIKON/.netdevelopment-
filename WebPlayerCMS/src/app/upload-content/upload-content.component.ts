import { Component, OnInit, EventEmitter, ViewContainerRef } from '@angular/core';
import { FileUploader, FileLikeObject, FileItem, FileSelectDirective, } from 'ng2-file-upload';
import { ToastsManager } from 'ng6-toastr';
import { ConfigAPI } from '../class/ConfigAPI';
import { SerLicenseHolderService } from '../license-holder/ser-license-holder.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
const URL2 = 'http://localhost:60328/api/UploadImage';

@Component({
  selector: 'app-upload-content',
  templateUrl: './upload-content.component.html',
  styleUrls: ['./upload-content.component.css']
})
//disableMultipart:true  

export class UploadContentComponent implements OnInit {
  cmbGenre = "0";
  GenreName = "";
  FolderName = "";
  cmbFolder = "0";
  CustomerId = "0";
  CustomerList: any[];
  GenreList: any[];
  FolderList:any[];
  public loading = false;
  IsAdminLogin: boolean = true;
  NewFolderName: string = "";
  InputAccept="";
  MediaType="";
  public uploader: FileUploader = new FileUploader({
    url: this.cf.UploadImage,
    itemAlias: 'photo',
  });
  constructor(public toastr: ToastsManager, vcr: ViewContainerRef, private cf: ConfigAPI,
    private serviceLicense: SerLicenseHolderService, config: NgbModalConfig, private modalService: NgbModal) {
    this.toastr.setRootViewContainerRef(vcr);
    config.backdrop = 'static';
    config.keyboard = false;
    this.uploader.onCompleteAll = () => {
      this.cmbGenre = "0";
      this.GenreName="";
      this.FolderName="";
      this.cmbFolder="0";
      this.uploader = new FileUploader({
        url: this.cf.UploadImage,
        itemAlias: 'photo',
      })
      
      this.toastr.info("Content Uploaded");
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

    this.serviceLicense.FillCombo(str).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.CustomerList = JSON.parse(returnData);
        this.loading = false;
        this.FillGenre();
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  NewList;
  GetJSONRecord = (array): void => {
    this.NewList = this.GenreList.filter(order => order.Id == array.Id);
  }
  onChangeCustomer(id){
    this.uploader.clearQueue();
    this.FillFolder(id);
  }
  FillFolder(cid) {
    this.loading = true;
    var qry = "select folderId as Id, foldername as DisplayName  from tbFolder ";
    qry = qry + " where dfclientId="+cid+" ";
    qry = qry + " order by foldername ";
    this.serviceLicense.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.FolderList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onChangeGenre(id) {
    
    this.uploader.clearQueue();
    var ArrayItem = {};
    var fName = "";
    ArrayItem["Id"] = id;
    ArrayItem["DisplayName"] = "";
    this.cmbGenre = id;
    this.GetJSONRecord(ArrayItem);
    if (this.NewList.length > 0) {
      this.GenreName = this.NewList[0].DisplayName;
    }
    if ((id=='303') || (id=='297')){
      this.InputAccept=".mp4";
      this.MediaType="Video";
    }
    else{
      this.InputAccept="image/jpeg, image/x-png";
      this.MediaType="Image";
    }
    if (id=='0'){
      this.InputAccept="";
    }
  }
  ngOnInit() {
    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };
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
    if (this.cmbGenre == "0") {
      this.toastr.info("Genre cannot be blank");
      return;
    }
     
    this.uploader.onBuildItemForm = (fileItem: any, form: any) => {
      form.append('GenreId', this.cmbGenre);
      form.append('GenreName', this.GenreName);
      form.append('CustomerId', this.CustomerId);
      form.append('MediaType', this.MediaType);
      form.append('FolderId', this.cmbFolder);
    };
    this.uploader.uploadAll()
  }

  FillGenre() {
    this.loading = true;
    var qry = "select tbGenre.GenreId as Id, genre as DisplayName  from tbGenre ";
    qry = qry + " where mediatype='Image'  or genreid in(303,297) ";
    qry = qry + " order by genre ";
    this.serviceLicense.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.GenreList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  openGenreModal(mdl) {
    this.NewFolderName = this.FolderName;
    this.modalService.open(mdl);
  }
  onSubmitNewGenre() {
    if (this.NewFolderName == "") {
      this.toastr.info("Folder name cannot be blank", '');
      return;
    }

    this.serviceLicense.SaveFolder(this.cmbFolder, this.NewFolderName, this.CustomerId).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce != "-2") {
          this.toastr.info("Saved", 'Success!');

          this.loading = false;
          if (this.cmbFolder == "0") {
            this.SaveModifyInfo(0, "New folder is create with name " + this.NewFolderName);
          }
          else {
            this.SaveModifyInfo(0, "Folder name is modify. Now New name is " + this.NewFolderName);

          }
          this.cmbFolder = "0";
          this.FolderName="";
          this.FillFolder(this.CustomerId);
          this.modalService.dismissAll();
        }
        else if (obj.Responce == "-2") {
          this.toastr.info("This folder name already exists", '');
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
  SaveModifyInfo(tokenid, ModifyText) {
    this.serviceLicense.SaveModifyLogs(tokenid, ModifyText).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
      },
        error => {
        })
  };
  NewfList;
  GetJSONFolderRecord = (array): void => {
    this.NewfList = this.FolderList.filter(order => order.Id == array.Id);
  }
  onChangeFolder(id){
    this.FolderName="";
    var ArrayItem = {};
    var fName = "";
    ArrayItem["Id"] = id;
    ArrayItem["DisplayName"] = "";
    this.cmbFolder = id;
    this.GetJSONFolderRecord(ArrayItem);
    if (this.NewfList.length > 0) {
      this.FolderName = this.NewfList[0].DisplayName;
    }
  }
}
