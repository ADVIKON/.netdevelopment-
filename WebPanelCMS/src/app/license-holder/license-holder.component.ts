import {Component,  OnInit,  ViewContainerRef,  Input,  Output,  ElementRef} from '@angular/core';
import {  NgbModalConfig,NgbModal,NgbNavChangeEvent,NgbTimepickerConfig,NgbTimeStruct} from '@ng-bootstrap/ng-bootstrap';
import { SerLicenseHolderService } from '../license-holder/ser-license-holder.service';
import { ToastrService } from 'ngx-toastr';
import { ExcelServiceService } from '../license-holder/excel-service.service';
import { ConfigAPI } from '../class/ConfigAPI';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth/auth.service';

@Directive({
  selector: '[appList]',
  host: {
    '[class.asc]': 'direction === "asc"',
    '[class.desc]': 'direction === "desc"',
    '(click)': 'rotate()',
  },
})
export class NgbdSortableHeader {
  constructor(private targetElem: ElementRef) {}
  elem = this.targetElem.nativeElement;

  sortable = this.elem.getAttribute('data-sortable');
  @Input() direction: SortDirection = '';
  @Input() appList: Array<any>;
  @Output() sort = new EventEmitter<SortEvent>();

  rotate() {
    this.direction = rotate[this.direction];
    this.sort.emit({
      column: this.sortable,
      direction: this.direction,
      arList: this.appList,
    });
  }
}
@Component({
  selector: 'app-license-holder',
  templateUrl: './license-holder.component.html',
  styleUrls: ['./license-holder.component.css'],
  providers: [NgbModalConfig, NgbModal],
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
  cid = '0';
  LogoId = 0;
  SongsList = [];
  DelLogoId = 0;
  uExcel: boolean = false;
  IsForceUpdateRunning: boolean = false;
  IsForceUpdateAll: boolean = false;
  ForceUpdateBar: number = 0;
  IsIndicatorShow: boolean = false;
  interval;
  cmbFolder = '0';
  TokenSelected = [];
  chkAll: boolean = false;
  ActiveTokenList = [];
  MainTokenList = [];
  InfoTokenList = [];
  active = 2;
  GroupList = [];
  SearchGroupList = [];
  cmbSearchGroup = '';
  cmbGroup = '0';
  ModifyGroupName = '';
  GroupTokenList = [];
  OpeningHoursList = [];
  GroupActive = 1;
  grpSearchText = '';
  GroupSearchTokenList = [];
  GroupTokenSelected = [];
  txtDelPer;
  cmbPlaylist = '0';
  tokenid;
  TokenPlaylistList = [];
  dropdownSettings = {};

  dropdownList = [];
  selectedItems = [];
  openSearchText;
  OpeningHourTokenSelected = [];
  frmOpeningHour: FormGroup;
  time: NgbTimeStruct = {hour: 0, minute: 0, second: 0};
  time2: NgbTimeStruct = {hour: 23, minute: 59, second: 0};
  CountryList= [];
  StateList = [];
  CityList= [];
  cmbCustomerId = '0';
  constructor(
    config: NgbModalConfig,
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    private cf: ConfigAPI,
    private serviceLicense: SerLicenseHolderService,
    private excelService: ExcelServiceService,
    public toastr: ToastrService,
    public auth: AuthService,
    private tService: TokenInfoServiceService,
    private pService: PlaylistLibService,
    vcr: ViewContainerRef,
    configTime: NgbTimepickerConfig
  ) {
    config.backdrop = 'static';
    config.keyboard = false;
    configTime.seconds = false;
    configTime.spinners = false;
  }
  public onNavChange(changeEvent: NgbNavChangeEvent) {
    if (changeEvent.nextId === 1) {
      changeEvent.preventDefault();
    }
  }
 async ngOnInit() {
    this.Adform = this.formBuilder.group({
      FilePathNew: [''],
    });

    this.LogoId = 0;
    this.TokenList = [];
    await this.FillClientList();
    this.selectedItems = [];
    this.dropdownList = [
      { id: '1', itemName: 'Mon' },
      { id: '2', itemName: 'Tue' },
      { id: '3', itemName: 'Wed' },
      { id: '4', itemName: 'Thu' },
      { id: '5', itemName: 'Fri' },
      { id: '6', itemName: 'Sat' },
      { id: '7', itemName: 'Sun' },
    ];
    this.dropdownSettings = {
      singleSelection: false,
      text: 'Select Week',
      idField: 'id',
      textField: 'itemName',
      selectAllText: 'Week',
      unSelectAllText: 'Week',
      itemsShowLimit: 3,
    };
  }

  SetFormOpeningHour() {
    this.frmOpeningHour = this.formBuilder.group({
      startTime: [this.time, Validators.required],
      EndTime: [this.time2, Validators.required],
      wList: [this.selectedItems, Validators.required],
      TokenList: [this.OpeningHourTokenSelected],
    });
  }
  UpdateTokenOpeningHours() {
    if (!this.frmOpeningHour.invalid) {
      return;
    }
    if (this.OpeningHourTokenSelected.length === 0) {
      this.toastr.info('Please select atleast one location');
      return;
    }
    this.frmOpeningHour.controls.TokenList.setValue(
      this.OpeningHourTokenSelected
    );
    const sTime = this.frmOpeningHour.value.startTime;
    const eTime = this.frmOpeningHour.value.EndTime;
    const dt = new Date(
      'Mon Mar 09 2020 ' + sTime.hour + ':' + sTime.minute + ':00'
    );
    const dt2 = new Date(
      'Mon Mar 09 2020 ' + eTime.hour + ':' + eTime.minute + ':00'
    );
    this.frmOpeningHour
      .get('startTime')
      .setValue(dt.toTimeString().slice(0, 5));
    this.frmOpeningHour.get('EndTime').setValue(dt2.toTimeString().slice(0, 5));
    this.loading = true;
    this.serviceLicense
      .SaveOpeningHours(this.frmOpeningHour.value)
      .pipe()
      .subscribe(
        (data) => {
          const returnData = JSON.stringify(data);
          const obj = JSON.parse(returnData);
          if (obj.Responce === '1') {
            this.toastr.info('Saved', 'Success!');
          }
          this.selectedItems = [];
          this.OpeningHourTokenSelected = [];
          this.frmOpeningHour.get('startTime').setValue(sTime);
          this.frmOpeningHour.get('EndTime').setValue(eTime);
          this.frmOpeningHour.get('wList').setValue(this.selectedItems);

          this.FillTokenOpeningHours();
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }

  onItemSelect(item: any) {}
  onSelectAll(items: any) {}
  FillClientList() {
    this.loading = true;
    var str = '';
    var i = this.auth.IsAdminLogin$.value ? 1 : 0;
    var i = this.auth.IsAdminLogin$.value ? 1 : 0;
    str =
      'FillCustomer ' +
      i +
      ', ' +
      localStorage.getItem('dfClientId') +
      ',' +
      localStorage.getItem('DBType');

    this.serviceLicense
      .FillCombo(str)
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          this.CustomerList = JSON.parse(returnData);
          this.loading = false;
          if ((this.auth.IsAdminLogin$.value == false)) {
            this.cmbCustomerId = localStorage.getItem('dfClientId');
            this.onChangeCustomer(this.cmbCustomerId);
          }
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }
  open(content, tid) {
    localStorage.setItem('tokenid', tid);
    this.modalService.open(content, { size: 'lg' });
  }
  onChangeCustomer(deviceValue) {
    this.SongsList = [];

    if (deviceValue == '0') {
      this.TokenList = [];
      this.LogoId = 0;
      this.cid = '0';
      this.MainTokenList = [];

      return;
    }

    this.loading = true;
    this.cid = deviceValue;

    this.serviceLicense
      .FillTokenInfo(deviceValue, '0')
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          this.TokenList = JSON.parse(returnData);
          this.MainTokenList = JSON.parse(returnData);
          this.InfoTokenList = JSON.parse(returnData);
          console.log(this.MainTokenList);

          // this.TokenList.sort(this.GetSortOrder("token",false));

          if (this.TokenList.length != 0) {
            this.LogoId = this.TokenList[0].AppLogoId;
            if (this.TokenList[0].IsIndicatorActive == '1') {
              this.IsIndicatorShow = true;
            } else {
              this.IsIndicatorShow = true;
            }
          }
          this.loading = false;
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
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
    if (this.cid == '0') {
      this.toastr.info('Please select a customer name');
      return;
    }
    this.loading = true;
    this.serviceLicense
      .UpdateAppLogo(this.cid, LogoId)
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          var obj = JSON.parse(returnData);
          if (obj.Responce == '1') {
            this.toastr.info('Logo is set', 'Success!');
            this.loading = false;
            this.LogoId = LogoId;
          } else {
            this.toastr.error(
              'Apologies for the inconvenience.The error is recorded.',
              ''
            );
          }
          this.loading = false;
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }

  SetIndicator(Indicator) {
    this.IsIndicatorShow = Indicator;

    if (this.cid == '0') {
      this.toastr.info('Please select a customer name');
      return;
    }
    this.loading = true;
    this.serviceLicense
      .SetOnlineIndicator(this.cid, Indicator)
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          var obj = JSON.parse(returnData);
          if (obj.Responce == '1') {
            if (Indicator == true) {
              this.toastr.info(
                'Online Indicator is set for all locations',
                'Success!'
              );
            }
            if (Indicator == false) {
              this.toastr.info(
                'Online Indicator is disable for all locations',
                'Success!'
              );
            }
            this.loading = false;
            this.onChangeCustomer(this.cid);
          } else {
            this.toastr.error(
              'Apologies for the inconvenience.The error is recorded.',
              ''
            );
          }
          this.loading = false;
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }
  startTimer() {
    this.interval = setInterval(() => {
      if (this.ForceUpdateBar >= 0 && this.ForceUpdateBar <= 99) {
        this.ForceUpdateBar++;
      } else {
        this.ForceUpdateBar = 100;
        this.IsForceUpdateRunning = false;
        this.IsForceUpdateAll = false;
        clearInterval(this.interval);
      }
    }, 1000);
  }
  enableEditIndex = null;
  ForceUpdate(tokenid, i) {
    if (this.cid == '0') {
      this.toastr.info('Please select a customer name');
      return;
    }

    this.loading = true;
    this.TokenSelected = [];
    this.TokenSelected.push(tokenid);
    this.serviceLicense
      .ForceUpdate(this.TokenSelected)
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          var obj = JSON.parse(returnData);
          if (obj.Responce == '1') {
            this.toastr.info('Saved', 'Success!');
            this.loading = false;
            if (i == '-1') {
              this.enableEditIndex = null;
              this.IsForceUpdateRunning = false;
              this.IsForceUpdateAll = true;
            } else {
              this.IsForceUpdateAll = false;
              this.enableEditIndex = i;
              this.IsForceUpdateRunning = true;
            }
            this.ForceUpdateBar = 0;
            this.startTimer();
          } else {
            this.toastr.error(
              'Apologies for the inconvenience.The error is recorded.',
              ''
            );
          }
          this.loading = false;
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }
  DeleteLogoModal(mContent, id) {
    this.DelLogoId = id;
    this.modalService.open(mContent);
  }
  DeleteLogo() {
    this.loading = true;
    this.serviceLicense
      .DeleteLogo(this.DelLogoId)
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          var obj = JSON.parse(returnData);
          if (obj.Responce == '1') {
            this.toastr.info('Deleted', 'Success!');
            this.loading = false;
            this.DelLogoId = 0;
            this.FillLogo(this.cmbFolder);
          } else {
            this.toastr.error(
              'Apologies for the inconvenience.The error is recorded.',
              ''
            );
          }
          this.loading = false;
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }
  ExportExcel() {
    if (this.cid == '0') {
      this.toastr.info('Please select a customer name');
      return;
    }
    this.uExcel = false;
    var ExportList = [];
    var ExportItem = {};
    for (var j = 0; j < this.TokenList.length; j++) {
      ExportItem = {};
      if (this.TokenList[j].token != 'used') {
        ExportItem['TokenId'] = this.TokenList[j].tokenid;
        ExportItem['TokenCode'] = this.TokenList[j].tokenCode;
        ExportItem['Serial-MAC'] = '';
        ExportItem['Location'] = '';
        ExportItem['IsAndroidPlayer'] = '';
        ExportItem['IsWindowPlayer'] = '';
        ExportItem['IsAudioPlayer'] = '';
        ExportItem['IsVideoPlayer'] = '';
        ExportItem['IsSanitizerPlayer'] = '';
        ExportList.push(ExportItem);
      }
    }
    this.excelService.exportAsExcelFile(ExportList, 'BulkActivation');
  }
  BulkActivation(modalContant) {
    if (this.cid == '0') {
      this.toastr.info('Please select a customer name');
      return;
    }
    this.uExcel = false;
    this.modalService.open(modalContant, {
      centered: true,
      windowClass: 'fade',
    });
  }
  InputFileName: string = 'No file chosen...';
  fileUpload = { status: '', message: '', filePath: '' };
  error: string;
  InputAccept: string = '*.xlsx';
  onSelectedFile(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.Adform.get('FilePathNew').setValue(file);
      this.InputFileName = file.name.replace('C:\\fakepath\\', '');
    } else {
      this.InputFileName = 'No file chosen...';
    }
  }
  Clear() {}
  Upload() {
    if (this.Adform.get('FilePathNew').value.length == 0) {
      this.toastr.info('Please select a file');
      return;
    }
    const formData = new FormData();
    formData.append('name', 'Excel');
    formData.append('profile', this.Adform.get('FilePathNew').value);

    this.serviceLicense.upload(formData).subscribe(
      (res) => {
        this.fileUpload = res;
        var returnData = JSON.stringify(res);
        var obj = JSON.parse(returnData);
        if (obj.Responce == '1') {
          this.toastr.info(obj.message, '');
          this.modalService.dismissAll();
          this.tokenInfoClose();
          this.loading = false;
        }
        if (obj.Responce == '0') {
          this.toastr.error(obj.message);
          this.InputFileName = 'No file chosen...';
          this.loading = false;
        }
        this.Adform.get('FilePathNew').setValue('');
      },
      (err) => {
        this.toastr.error('p');
        this.error = err;
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
    this.serviceLicense
      .FillSignageLogo(this.cid, fid)
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          var obj = JSON.parse(returnData);
          this.SongsList = obj;
          this.loading = false;
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }
  SetSignage(ObjModal) {
    if (this.cid == '0') {
      this.toastr.info('Please select a customer name');
      return;
    }
    this.FolderList = [];
    this.cmbFolder = '0';
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
  FillFolder() {
    this.loading = true;
    var str = '';
    str = '';
    str =
      'select distinct  f.folderId as id ,f.folderName as displayname FROM tbFolder f ';
    str = str + ' inner join Titles t on t.folderId= f.folderId ';
    str = str + ' where t.GenreId= 326 and t.folderId is not null ';

    str = str + ' and f.dfclientId=' + this.cid + ' ';

    this.serviceLicense
      .FillCombo(str)
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          this.FolderList = JSON.parse(returnData);
          this.loading = false;
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }

  allActiveToken(event) {
    const checked = event.target.checked;
    this.TokenSelected = [];
    this.ActiveTokenList.forEach((item) => {
      item.check = checked;
      this.TokenSelected.push(item.tokenid);
    });
    if (checked == false) {
      this.TokenSelected = [];
    }
  }

  SelectActiveToken(fileid, event) {
    if (event.target.checked) {
      this.TokenSelected.push(fileid);
    } else {
      const index: number = this.TokenSelected.indexOf(fileid);
      if (index !== -1) {
        this.TokenSelected.splice(index, 1);
      }
    }
  }

  ForceUpdateModal(modalContant) {
    if (this.cid == '0') {
      this.toastr.info('Please select a customer name');
      return;
    }
    this.loading = true;
    this.serviceLicense
      .FillTokenInfo(this.cid, '1')
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          this.ActiveTokenList = JSON.parse(returnData);
          this.loading = false;
          if (this.ActiveTokenList.length != 0) {
            this.modalService.open(modalContant, { size: 'lg' });
          } else {
            this.toastr.info('Regsiter tokens are not found');
          }
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }
  ForceUpdateAll() {
    if (this.TokenSelected.length == 0) {
      this.toastr.info('Please select a token');
      return;
    }

    this.loading = true;
    this.serviceLicense
      .ForceUpdate(this.TokenSelected)
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          var obj = JSON.parse(returnData);
          if (obj.Responce == '1') {
            this.toastr.info('Update request is submit', 'Success!');
            this.loading = false;
            this.chkAll = false;
            this.TokenSelected = [];
            this.modalService.dismissAll();
          } else {
            this.toastr.error(
              'Apologies for the inconvenience.The error is recorded.',
              ''
            );
          }
          this.loading = false;
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }

  GetSortOrder(prop, asc) {
    return function (a, b) {
      if (asc) {
        return a[prop] > b[prop] ? 1 : a[prop] < b[prop] ? -1 : 0;
      } else {
        return b[prop] > a[prop] ? 1 : b[prop] < a[prop] ? -1 : 0;
      }
      return 0;
    };
  }
  FilterTokenList(FilterValue) {
    this.TokenList = [];
    if (FilterValue == 'All') {
      this.TokenList = this.MainTokenList;
    }
    if (FilterValue == 'Regsiter') {
      this.TokenList = this.MainTokenList.filter(
        (order) => order.token === 'used'
      );
    }
    if (FilterValue == 'Audio') {
      this.TokenList = this.MainTokenList.filter(
        (order) => order.MediaType === 'Audio'
      );
    }
    if (FilterValue == 'Video') {
      this.TokenList = this.MainTokenList.filter(
        (order) => order.MediaType === 'Video'
      );
    }
    if (FilterValue == 'Signage') {
      this.TokenList = this.MainTokenList.filter(
        (order) => order.MediaType === 'Signage'
      );
    }
    if (FilterValue == 'UnRegsiter') {
      this.TokenList = this.MainTokenList.filter(
        (order) => order.token !== 'used'
      );
    }
  }
  OpenGroupsModal(gModal) {
    if (this.cid === '0') {
      this.toastr.info('Please select a customer name');
      return;
    }
    this.FillGroup();
    this.modalService.open(gModal, { size: 'lg' });
  }
  FillGroup() {
    this.loading = true;
    const qry =
      'select GroupId as id, GroupName as displayname  from tbGroup where dfClientId = ' +
      this.cid +
      ' order by GroupName';
    this.serviceLicense
      .FillCombo(qry)
      .pipe()
      .subscribe(
        (data) => {
          const returnData = JSON.stringify(data);
          this.GroupList = JSON.parse(returnData);
          this.SearchGroupList = JSON.parse(returnData);
          this.loading = false;
          this.GroupTokenList = [];
          this.GroupTokenList = this.MainTokenList.filter(
            (order) => order.GroupId === '0'
          );
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }
  onChangeSearchGroup(id) {
    this.GroupSearchTokenList = [];
    if (id !== '0') {
      this.GroupSearchTokenList = this.MainTokenList.filter(
        (order) => order.GroupId === id
      );
    }
  }
  onChangeGroup(id) {
    let NewFilterList = [];
    NewFilterList = this.GroupList.filter((order) => order.Id === id);
    if (NewFilterList.length > 0) {
      this.ModifyGroupName = NewFilterList[0].DisplayName;
    }
  }
  openCommonModal(modal, ModalType) {
    this.modalService.open(modal);
  }
  onSubmitModal() {
    this.loading = true;
    this.tService
      .CitySateNewModify(
        this.cmbGroup,
        this.ModifyGroupName,
        'Group',
        '0',
        '0',
        this.cid
      )
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          var obj = JSON.parse(returnData);
          if (obj.Responce == '1') {
            this.toastr.info('Saved', 'Success!');
            this.loading = false;
            this.FillGroup();
          } else if (obj.Responce == '-2') {
            this.toastr.info('Name is already exixts', '');
            this.loading = false;
          } else {
            this.toastr.error(
              'Apologies for the inconvenience.The error is recorded.',
              ''
            );
            this.loading = false;
            return;
          }
          this.cmbGroup = '0';
          this.ModifyGroupName = '';
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }

  SelectGroupToken(fileid, event) {
    if (event.target.checked) {
      this.GroupTokenSelected.push(fileid);
    } else {
      const index: number = this.GroupTokenSelected.indexOf(fileid);
      if (index !== -1) {
        this.GroupTokenSelected.splice(index, 1);
      }
    }
  }
  SelectOpeningHourToken(fileid, event) {
    if (event.target.checked) {
      this.OpeningHourTokenSelected.push(fileid);
    } else {
      const index: number = this.OpeningHourTokenSelected.indexOf(fileid);
      if (index !== -1) {
        this.OpeningHourTokenSelected.splice(index, 1);
      }
    }
  }
  UpdateTokenGroups() {
    if (this.cmbGroup === '0') {
      this.toastr.info('Please select a group');
      return;
    }
    if (this.GroupTokenSelected.length === 0) {
      this.toastr.info('Please select atleast one location');
      return;
    }
    this.CallAPIGropsTokenUpdate(this.GroupTokenSelected, this.cmbGroup);
  }
  openDeleteModal(id) {
    this.GroupTokenSelected = [];
    this.GroupTokenSelected.push(id);
    this.CallAPIGropsTokenUpdate(this.GroupTokenSelected, '0');
  }
  CallAPIGropsTokenUpdate(gts, grpid) {
    this.loading = true;
    this.serviceLicense
      .UpdateTokenGroups(gts, grpid)
      .pipe()
      .subscribe(
        (data) => {
          const returnData = JSON.stringify(data);
          const obj = JSON.parse(returnData);
          if (obj.Responce === '1') {
            this.toastr.info('Saved', 'Success!');
            this.loading = false;
            this.MainTokenList.forEach((e) => {
              const index = gts.indexOf(e.tokenid);
              if (index >= 0) {
                e.GroupId = grpid;
              }
            });
            this.GroupTokenSelected = [];
            this.GroupTokenList = [];
            this.GroupTokenList = this.MainTokenList.filter(
              (order) => order.GroupId === '0'
            );
            this.GroupSearchTokenList = [];
            if (this.cmbSearchGroup !== '0') {
              this.GroupSearchTokenList = this.MainTokenList.filter(
                (order) => order.GroupId === this.cmbSearchGroup
              );
            }
            this.cmbGroup = '0';
          } else {
            this.toastr.error(
              'Apologies for the inconvenience.The error is recorded.',
              ''
            );
            this.loading = false;
            return;
          }
          this.cmbGroup = '0';
          this.ModifyGroupName = '';
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }

  OpenDeleteGroupModal(modal) {
    if (this.cmbGroup === '0') {
      this.toastr.info('Please select a group');
      return;
    }
    this.modalService.open(modal);
  }
  DeleteGroup() {
    this.loading = true;
    this.serviceLicense
      .DeleteGroup(this.cmbGroup)
      .pipe()
      .subscribe(
        (data) => {
          const returnData = JSON.stringify(data);
          const obj = JSON.parse(returnData);
          if (obj.Responce === '1') {
            this.toastr.info('Saved', 'Success!');
            this.loading = false;
            this.MainTokenList.forEach((e) => {
              const gid = e.GroupId;
              if (gid === this.cmbGroup) {
                e.GroupId = '0';
              }
            });
            console.log(JSON.stringify(this.MainTokenList));
            this.GroupTokenSelected = [];
            this.GroupSearchTokenList = [];
            this.FillGroup();
          } else {
            this.toastr.error(
              'Apologies for the inconvenience.The error is recorded.',
              ''
            );
            this.loading = false;
            return;
          }
          this.cmbGroup = '0';
          this.cmbSearchGroup = '0';
          this.ModifyGroupName = '';
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }
  onDeletePercentageClick(mContent) {
    this.modalService.open(mContent);
  }
  DeleteTitlePercentage() {
    this.loading = true;
    this.pService
      .DeleteTitlePercentage(this.cmbPlaylist, this.txtDelPer)
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          var obj = JSON.parse(returnData);
          if (obj.Responce == '1') {
            this.toastr.info('Deleted', 'Success!');
            this.loading = false;
            this.FillTokenPlaylists();
          } else {
            this.toastr.error(
              'Apologies for the inconvenience.The error is recorded.',
              ''
            );
          }
          this.loading = false;
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }
  onChangePlaylist(e) {}

  onDeletePlaylistSongModal(Modal, tokenid) {
    this.tokenid = tokenid;
    this.txtDelPer = 0;
    this.FillTokenPlaylists();
    this.modalService.open(Modal);
  }

  FillTokenPlaylists() {
    this.loading = true;
    const qry = 'GetTokenPlaylist ' + this.tokenid;
    this.pService
      .FillCombo(qry)
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          this.TokenPlaylistList = JSON.parse(returnData);
          this.loading = false;
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }
  OpenOpeningHoursModal(gModal) {
    if (this.cid === '0') {
      this.toastr.info('Please select a customer name');
      return;
    }
    this.OpeningHoursList = [];
    this.SetFormOpeningHour();
    this.FillTokenOpeningHours();
    this.modalService.open(gModal, { size: 'lg' });
  }
  FillTokenOpeningHours() {
    this.loading = true;
    this.serviceLicense
      .FillTokenOpeningHours(this.cid, '0')
      .pipe()
      .subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          this.OpeningHoursList = JSON.parse(returnData);
          this.loading = false;
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }

  OpenUpdateInfo(InfoModal){
    if (this.cid == '0') {
      this.toastr.info('Please select a customer name');
      return;
    }
    this.uExcel = false;
    this.modalService.open(InfoModal, {
      centered: true,
      windowClass: 'fade',
    });
  }


  UpdateInfo(){
    let Info={};
const BodyData =[];
    this.InfoTokenList.forEach(item => {
      Info = {};
      Info['tokenid']= item.tokenid;
      Info['CountryId']= item.CountryId;
      Info['State']= item.State;
      Info['city']= item.city;
      Info['location']= item.location;
      Info['Street']= item.Street;
      Info['LicenceType']= item.LicenceType;
      Info['MediaType']= item.MediaType;
      Info['playerType']= item.playerType;
      BodyData.push(Info);
    });
    this.loading = true;
    this.serviceLicense.UpdateTokenInfo(BodyData).pipe().subscribe(
        (data) => {
          var returnData = JSON.stringify(data);
          var obj = JSON.parse(returnData);
          if (obj.Responce == '1') {
            this.toastr.info('Saved', 'Success!');
            this.loading = false;
            this.onChangeCustomer(this.cid);
            this.modalService.dismissAll();
          }
          else{
            this.loading = false;
          }
        },
        (error) => {
          this.toastr.error(
            'Apologies for the inconvenience.The error is recorded.',
            ''
          );
          this.loading = false;
        }
      );
  }
  ExportExcelInfo() {
    if (this.cid == '0') {
      this.toastr.info('Please select a customer name');
      return;
    }
    this.uExcel = false;
    var ExportList = [];
    var ExportItem = {};
    for (var j = 0; j < this.MainTokenList.length; j++) {
      ExportItem = {};
       
        ExportItem['TokenId'] = this.MainTokenList[j].tokenid;
        ExportItem['TokenCode'] = this.MainTokenList[j].TokenNoBkp;
        ExportItem['Country'] = this.MainTokenList[j].CountryFullName;
        ExportItem['State'] = this.MainTokenList[j].State;
        ExportItem['City'] = this.MainTokenList[j].city;
        ExportItem['Street'] = this.MainTokenList[j].Street;
        ExportItem['Location'] = this.MainTokenList[j].location;

        if (this.MainTokenList[j].playerType==='Windows'){
          ExportItem['IsWindowsPlayer'] = '1';
        }
        else{
          ExportItem['IsWindowsPlayer'] = '0';
        }
        if (this.MainTokenList[j].playerType==='Android'){
          ExportItem['IsAndroidPlayer'] = '1';
        }
        else{
          ExportItem['IsAndroidPlayer'] = '0';
        }
        
        
        if (this.MainTokenList[j].MediaType==='Audio'){
          ExportItem['IsAudioPlayer'] = '1';
        }
        else{
          ExportItem['IsAudioPlayer'] = '0';
        }

        if (this.MainTokenList[j].MediaType==='Video'){
          ExportItem['IsVideoPlayer'] = '1';
        }
        else{
          ExportItem['IsVideoPlayer'] = '0';
        }

        if (this.MainTokenList[j].MediaType==='Signage'){
          ExportItem['IsSignagePlayer'] = '1';
        }
        else{
          ExportItem['IsSignagePlayer'] = '0';
        }

        if (this.MainTokenList[j].LicenceType==='Copyright'){
          ExportItem['IsCopyright'] = '1';
        }
        else{
          ExportItem['IsCopyright'] = '0';
        }
        if (this.MainTokenList[j].LicenceType==='DirectLicence'){
          ExportItem['IsDirectLicence'] = '1';
        }
        else{
          ExportItem['IsDirectLicence'] = '0';
        }
 
        if (this.MainTokenList[j].DeviceType==='Screen'){
          ExportItem['IsScreen'] = '1';
        }
        else{
          ExportItem['IsScreen'] = '0';
        }

        if ((this.MainTokenList[j].MediaType==='Signage') && (this.MainTokenList[j].DeviceType==='Sanitizer')){
          ExportItem['IsSanitizer'] = '1';
        }
        else{
          ExportItem['IsSanitizer'] = '0';
        }
 

        ExportItem['DispenserAlertEmail'] = this.MainTokenList[j].AlertEmail;

        ExportList.push(ExportItem);
      
    }
    this.excelService.exportAsExcelFile(ExportList, 'BulkPlayerInfo');
  }

  UploadExcelInfo() {
    this.uExcel = true;
  }
  CancelInfo() {
    this.uExcel = false;
  }
  onSelectedFileInfo(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.Adform.get('FilePathNew').setValue(file);
      this.InputFileName = file.name.replace('C:\\fakepath\\', '');
    } else {
      this.InputFileName = 'No file chosen...';
    }
  }
  UploadInfo() {
    if (this.Adform.get('FilePathNew').value.length == 0) {
      this.toastr.info('Please select a file');
      return;
    }
    const formData = new FormData();
    formData.append('name', 'Excel');
    formData.append('profile', this.Adform.get('FilePathNew').value);

    this.serviceLicense.UpdateTokenInfo(formData).subscribe(
      (res) => {
        this.fileUpload = res;
        var returnData = JSON.stringify(res);
        var obj = JSON.parse(returnData);
        if (obj.Responce == '1') {
          this.toastr.info(obj.message, '');
          this.modalService.dismissAll();
          this.loading = false;
          this.tokenInfoClose();
        
        }
        if (obj.Responce == '0') {
          this.toastr.error(obj.message);
          this.InputFileName = 'No file chosen...';
          this.loading = false;
        }
        this.Adform.get('FilePathNew').setValue('');
      },
      (err) => {
        this.toastr.error('p');
        this.error = err;
        this.loading = false;
      }
    );
  }







  
  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader>;
  onSort({ column, direction, arList }: SortEvent) {
    this.TokenList = arList;
    // resetting other headers
    this.headers.forEach((header) => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    // sorting countries
    if (direction === '' || column === '') {
      this.TokenList = arList;
    } else {
      this.TokenList = [...arList].sort((a, b) => {
        const res = compare(a[column], b[column]);
        return direction === 'asc' ? res : -res;
      });
    }
  }
}



























import {
  Directive,
  EventEmitter,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { TokenInfoServiceService } from '../components/token-info/token-info-service.service';
import { PlaylistLibService } from '../playlist-library/playlist-lib.service';

export type SortDirection = 'asc' | 'desc' | '';
const rotate: { [key: string]: SortDirection } = {
  asc: 'desc',
  desc: '',
  '': 'asc',
};

const compare = (v1: string | number, v2: string | number) =>
  v1 < v2 ? -1 : v1 > v2 ? 1 : 0;

export interface SortEvent {
  column: '';
  direction: SortDirection;
  arList: Array<any>;
}
