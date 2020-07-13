import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { NgbModalConfig, NgbModal, NgbButtonsModule } from '@ng-bootstrap/ng-bootstrap';
import { SerLicenseHolderService } from '../license-holder/ser-license-holder.service';
import { ToastrService } from 'ngx-toastr';
import { ExcelServiceService } from '../license-holder/excel-service.service';
import { ConfigAPI } from '../class/ConfigAPI';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth/auth.service';
@Component({
  selector: 'app-license-holder',
  templateUrl: './license-holder.component.html',
  styleUrls: ['./license-holder.component.css'],
  providers: [NgbModalConfig, NgbModal]
})

export class LicenseHolderComponent implements OnInit {
  Adform: FormGroup;
  TokenList = [];
  CustomerList: any[];
  FolderList: any[];

  public loading = false;
  TokenInfoPopup: boolean = false;
  page: number = 1;
  pageSize: number = 20;


  searchText;
  cid = "0";
  LogoId = 0;
  SongsList = [];
  DelLogoId = 0;
  uExcel: boolean = false;
  IsForceUpdateRunning: boolean = false;
  ForceUpdateBar: number = 0;
  IsIndicatorShow: boolean = false;
  interval;
  cmbFolder = "0";

  constructor(config: NgbModalConfig, private formBuilder: FormBuilder, private modalService: NgbModal,
    private cf: ConfigAPI, private serviceLicense: SerLicenseHolderService,
    private excelService: ExcelServiceService, public toastr: ToastrService, public auth:AuthService,
    vcr: ViewContainerRef) {
    config.backdrop = 'static';
    config.keyboard = false;


  }

  ngOnInit() {
    this.Adform = this.formBuilder.group({
      FilePathNew: ['']
    });


    this.LogoId = 0;
    this.TokenList = [];
 
     
    this.FillClientList();



  }
  FillClientList() {
    this.loading = true;
    var str = "";
    var i = this.auth.IsAdminLogin$.value ? 1 : 0; 
    var i = this.auth.IsAdminLogin$.value ? 1 : 0;
    str = "FillCustomer " + i + ", " + localStorage.getItem('dfClientId') + "," + localStorage.getItem('DBType');


    this.serviceLicense.FillCombo(str).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.CustomerList = JSON.parse(returnData);
        this.loading = false;

      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  open(content, tid) {
    localStorage.setItem("tokenid", tid);
    this.modalService.open(content, { size: 'lg' });
  }
  onChangeCustomer(deviceValue) {

    this.SongsList = [];
    if (deviceValue == "0") {
      this.TokenList = [];
      this.LogoId = 0;
      this.cid = "0";
      return;
    }

    this.loading = true;
    this.cid = deviceValue;
    this.serviceLicense.FillTokenInfo(deviceValue).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.TokenList = JSON.parse(returnData);
if (this.TokenList.length!=0){
        this.LogoId = this.TokenList[0].AppLogoId;
        if (this.TokenList[0].IsIndicatorActive == "1") {
          this.IsIndicatorShow = true;
        }
        else {
          this.IsIndicatorShow = true;
        }
      }
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  tokenInfoClose() {
    this.onChangeCustomer(this.cid);
    this.modalService.dismissAll();
  }


  FullImageUrl;
  OpenFullImageModal(ObjModal, url) {
    this.FullImageUrl = url;
    this.modalService.open(ObjModal, { size: 'lg' });

  }

  SetLogo(LogoId) {
    if (this.cid == "0") {
      this.toastr.info("Please select a customer name");
      return;
    }
    this.loading = true;
    this.serviceLicense.UpdateAppLogo(this.cid, LogoId).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Logo is set", 'Success!');
          this.loading = false;
          this.LogoId = LogoId;
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

  SetIndicator(Indicator) {

    this.IsIndicatorShow = Indicator;


    if (this.cid == "0") {
      this.toastr.info("Please select a customer name");
      return;
    }
    this.loading = true;
    this.serviceLicense.SetOnlineIndicator(this.cid, Indicator).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Online Indicator is set for all locations", 'Success!');
          this.loading = false;
          this.onChangeCustomer(this.cid);
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
  startTimer() {
    this.interval = setInterval(() => {
      if ((this.ForceUpdateBar >= 0) && (this.ForceUpdateBar <= 99)) {
        this.ForceUpdateBar++;
      } else {
        this.ForceUpdateBar = 100;
        this.IsForceUpdateRunning = false;
        clearInterval(this.interval);
      }
    }, 1000)
  }
  ForceUpdate(tokenid) {
    if (this.cid == "0") {
      this.toastr.info("Please select a customer name");
      return;
    }

    this.loading = true;
    this.serviceLicense.ForceUpdate(this.cid, tokenid).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Saved", 'Success!');
          this.loading = false;
          this.IsForceUpdateRunning = true;
          this.startTimer();
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
  DeleteLogoModal(mContent, id) {

    this.DelLogoId = id;
    this.modalService.open(mContent);
  }
  DeleteLogo() {
    this.loading = true;
    this.serviceLicense.DeleteLogo(this.DelLogoId).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Deleted", 'Success!');
          this.loading = false;
          this.DelLogoId = 0;
          this.FillLogo(this.cmbFolder);
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
  ExportExcel() {
    if (this.cid == "0") {
      this.toastr.info("Please select a customer name");
      return;
    }
    this.uExcel = false;
    var ExportList = [];
    var ExportItem = {}
    for (var j = 0; j < this.TokenList.length; j++) {
      ExportItem = {};
      if (this.TokenList[j].token != "used") {
        ExportItem["TokenId"] = this.TokenList[j].tokenid;
        ExportItem["TokenCode"] = this.TokenList[j].tokenCode;
        ExportItem["Serial-MAC"] = "";
        ExportItem["Location"] = "";
        ExportItem["IsAndroidPlayer"] = "";
        ExportItem["IsWindowPlayer"] = "";
        ExportItem["IsAudioPlayer"] = "";
        ExportItem["IsVideoPlayer"] = "";
        ExportItem["IsSanitizerPlayer"] = "";
        ExportList.push(ExportItem);
      }
    }
    this.excelService.exportAsExcelFile(ExportList, 'BulkActivation');
  }
  BulkActivation(modalContant) {
    if (this.cid == "0") {
      this.toastr.info("Please select a customer name");
      return;
    }
    this.uExcel = false;
    this.modalService.open(modalContant, { centered: true, windowClass: 'fade' });
  }
  InputFileName: string = "No file chosen...";
  fileUpload = { status: '', message: '', filePath: '' };
  error: string;
  InputAccept: string = "*.xlsx";
  onSelectedFile(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.Adform.get('FilePathNew').setValue(file);
      this.InputFileName = file.name.replace("C:\\fakepath\\", "");
    }
    else {
      this.InputFileName = "No file chosen...";
    }
  }
  Clear() {

  }
  Upload() {
    if (this.Adform.get('FilePathNew').value.length == 0) {
      this.toastr.info("Please select a file");
      return;
    }
    const formData = new FormData();
    formData.append('name', "Excel");
    formData.append('profile', this.Adform.get('FilePathNew').value);

    this.serviceLicense.upload(formData).subscribe(
      res => {
        this.fileUpload = res
        var returnData = JSON.stringify(res);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info(obj.message, '');
          this.modalService.dismissAll();
          this.tokenInfoClose();
          this.loading = false;
        }
        if (obj.Responce == "0") {
          this.toastr.error(obj.message);
          this.InputFileName = "No file chosen...";
          this.loading = false;
        }
        this.Adform.get('FilePathNew').setValue('');
      }
      ,
      err => {
        this.toastr.error("p")
        this.error = err
        this.loading = false;
      }
    );

  }
  UploadExcel() {
    this.uExcel = true;
  }
  Cancel() {
    this.uExcel = false;
  }


  FillLogo(fid) {

    this.loading = true;
    this.serviceLicense.FillSignageLogo(this.cid, fid).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        this.SongsList = obj;
        this.loading = false;

      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  SetSignage(ObjModal) {
    if (this.cid == "0") {
      this.toastr.info("Please select a customer name");
      return;
    }
    this.FolderList=[];
    this.cmbFolder="0";
    this.FillFolder();
    this.modalService.open(ObjModal, { size: 'lg' });
  }

  onChangeFolder(fid) {
    if (fid == 0) {
      this.SongsList = [];
      return;
    }
    this.FillLogo(fid);
  }
  FillFolder(){
    this.loading = true;
    var str = "";
      str = "";
      str = "select distinct  f.folderId as id ,f.folderName as displayname FROM tbFolder f ";
      str = str + " inner join Titles t on t.folderId= f.folderId ";
      str = str + " where t.GenreId= 326 and t.folderId is not null ";
      if (this.auth.IsAdminLogin$.value==false) {
        str = str + " and f.dfclientId="+localStorage.getItem('dfClientId')+" ";
      }
  

    this.serviceLicense.FillCombo(str).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.FolderList = JSON.parse(returnData);
        this.loading = false;

      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
} 