import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { ToastsManager } from 'ng6-toastr';

import { IPlayService } from '../instant-play/i-play.service';

@Component({
  selector: 'app-instant-play',
  templateUrl: './instant-play.component.html',
  styleUrls: ['./instant-play.component.css']
})
export class InstantPlayComponent implements OnInit {
  ActivePlaylist = [];
  PlayerList = [];
  DownloadedSongsList = [];
  AdsList = [];
  SongsList = [];
  SearchText = "";
  cmbAlbum;
  chkSearchRadio: string = "title";
  chkMediaRadio: string = "Audio";
  AlbumList = [];
  FCMID;
  IsVideoToken;
  Search: boolean = true;
  public loading = false;
  IsAdminLogin: boolean = false;
  CustomerList = [];
  SelectedClientId = "0";
  IsCL:boolean=false;
  IsRF:boolean=false;
  tid;
  constructor(public toastr: ToastsManager, vcr: ViewContainerRef, private ipService: IPlayService) {
    this.toastr.setRootViewContainerRef(vcr);
  }

  ngOnInit() {
    if(localStorage.getItem('IsRf')=="1"){
      this.IsRF=true;
      this.IsCL=false;
    }
    else{
      this.IsRF=false;
      this.IsCL=true;
    
    }
    if (localStorage.getItem('dfClientId') == "6") {
      this.IsAdminLogin = true;
      this.SelectedClientId = "0";
      this.FillClientList();
    }
    else {
      this.FillPlayer(localStorage.getItem('dfClientId'));
      this.SelectedClientId = localStorage.getItem('dfClientId');
    }

  }
  FillClientList() {
    this.loading = true;
    var str = "";
    str = "select DFClientID as id,  ClientName as displayname from DFClients where CountryCode is not null and DFClients.IsDealer=1 order by RIGHT(ClientName, LEN(ClientName) - 3)";
    this.ipService.FillCombo(str).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.CustomerList = JSON.parse(returnData);
        this.loading = false;
        // this.FillPlayer(localStorage.getItem('dfClientId'));
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  FillPlayer(id) {
    this.loading = true;
    this.ipService.FillPlayer(id).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.PlayerList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  FillAds(tid) {
    this.loading = true;
    this.ipService.FillAds(tid).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.AdsList = JSON.parse(returnData);

        this.loading = false;
        this.SearchContent();
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onChangePlayer(tid) {
    this.DownloadedSongsList = [];
    this.GetFCMID(tid);
    this.tid=tid;
  }
  GetFCMID(tid) {
    this.loading = true;
    this.ipService.GetFCMID(tid).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);

        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.FCMID = obj.FcmId;
          this.IsVideoToken = obj.IsVideoToken;
        }
        else {
          this.FCMID = "";
          this.IsVideoToken = "";
        }
        this.loading = false;
        this.FillActivePlaylist(tid);
      },
        error => {
          this.FCMID = "";
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  FillActivePlaylist(tid) {
    this.loading = true;
    this.ipService.FillActivePlaylist(tid, this.SelectedClientId).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);

        this.ActivePlaylist = JSON.parse(returnData);
        
        this.loading = false;
        this.FillAds(tid);
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onChangePlaylist(pId) {
    this.FillPlaylistSongs(pId);
  }
  FillPlaylistSongs(fileid) {
    this.loading = true;
    this.ipService.PlaylistSong(fileid, "No").pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);

        var obj = JSON.parse(returnData);
        this.DownloadedSongsList = obj;
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }

  FillSearch() {
    if (this.SearchText == "") {
      this.FillSongList();
      return;
    }
    this.loading = true;
    this.ipService.CommanSearch(this.chkSearchRadio, this.SearchText, this.chkMediaRadio).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        this.SongsList = obj;
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  FillSongList() {
    this.loading = true;
    this.ipService.FillSongList(this.chkMediaRadio).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        this.SongsList = obj;
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  SearchContent() {
    if (this.chkSearchRadio == "album") {
      this.FillAlbum();
      this.Search = false;
    }

    else {
      this.Search = true;
      this.FillSearch();
    }

  }
  FillAlbum() {
    this.loading = true;
    var qry = "spSearch_Album_Copyright '" + this.SearchText + "' , " + localStorage.getItem('IsRf');
    this.ipService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.AlbumList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  FillGenre() {
    this.loading = true;
    var qry = "select tbGenre.GenreId as Id, genre as DisplayName  from tbGenre ";
    qry=qry+" inner join Titles tit on tit.genreId= tbGenre.genreId ";
    qry=qry+" where tit.IsRoyaltyFree = "+localStorage.getItem('IsRf')+" ";
    qry=qry+" group by tbGenre.GenreId,genre ";
    qry=qry+" order by genre "; 
    this.ipService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.AlbumList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }

  FillCategory() {
    this.loading = true;
    var qry = "select  acategory as DisplayName, acategory as Id from titles ";
    qry = qry + " where acategory is not null and IsRoyaltyFree = "+localStorage.getItem('IsRf')+" ";
    if (this.chkMediaRadio == "Audio") {
      qry = qry + " and acategory not like '%video%' and acategory not like '%MP4%' ";

    }
    else if (this.chkMediaRadio == "Video") {
      qry = qry + " and ( acategory like '%video%' or acategory like '%mp4%' ) ";
    }
    else {
      qry = qry + " and ( acategory like '%Image%') ";
    }
    qry = qry + " group by acategory order by acategory ";

    this.ipService.FillCombo(qry).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.AlbumList = JSON.parse(returnData);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onChangeAlbum(id) {
    this.SearchText = id;
    this.FillSearch();
  }
  SearchRadioClick(e) {
    this.chkSearchRadio = e;
    this.SearchText = "";
    this.Search = true;

    if (this.chkSearchRadio == "Genre") {
      this.FillGenre();
      this.Search = false;
    }
    if (this.chkSearchRadio == "Category") {
      this.FillCategory();
      this.Search = false;
    }

  }
  MediaRadioClick(e) {
     
    this.SearchText = "";
    this.Search = true;
    if (e=="CL") {
      localStorage.setItem('IsRf','0');
    }
    else if  (e=="RF"){
      localStorage.setItem('IsRf','1');
    }
    else{
      this.chkMediaRadio = e;
    }
    this.FillSongList();
  }
  keyDownFunction(event) {
    if (event.keyCode == 13) {
      this.SearchContent();
    }
  }


  /// Instant Play=========================================
  InstantPlay(Reqid, Reqtype, Requrl, ArId, AlId, titleName, aname) {
    var ReqDeviceToken = this.FCMID;
    var vToken = this.IsVideoToken;
    var parm = JSON.stringify({
      id: Reqid,
      type: Reqtype,
      url: Requrl,
      DeviceToken: ReqDeviceToken,
      title: titleName,
      artistid: ArId,
      albumid: AlId,
      artistname: aname,
      IsVideoToken: vToken,
      tid:this.tid
    })

    this.ipService.SendNoti(parm).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);

        var obj = JSON.parse(returnData);
        this.loading = false;
        if (obj.Response == "1") {
          this.toastr.info("Request is assigned", '');
        }
        else {
          this.toastr.error("Request is not complete.Please try again later.", '');
        }
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  onClickInstant(id, Arid, Alid, mType, tName, aName, ActionType) {
    if (this.FCMID == "") {
      this.toastr.error("Player version is not compatible. Please update player version.");
     return;
    }
    var url = "";
        url = "http://134.119.178.26/mp3files/";
    if (mType == "Audio") {
      url = url + id + ".mp3";
    }
    if (mType == "Video") {
      url = url + id + ".mp4";
    }
 
     
    this.InstantPlay(id, ActionType, url, Arid, Alid, tName, aName);
  }
  onChangeCustomer(deviceValue) {
     
    this.SelectedClientId = deviceValue;
    this.DownloadedSongsList = [];
    this.ActivePlaylist =[];
    this.FillPlayer(deviceValue);

  }

}
