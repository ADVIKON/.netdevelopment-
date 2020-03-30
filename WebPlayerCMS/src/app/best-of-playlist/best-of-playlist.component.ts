import { Component, OnInit, ViewContainerRef, ViewChildren, QueryList, ElementRef, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng6-toastr';
import { BestoffService } from '../best-of-playlist/bestoff.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-best-of-playlist',
  templateUrl: './best-of-playlist.component.html',
  styleUrls: ['./best-of-playlist.component.css']
})
export class BestOfPlaylistComponent implements OnInit {
  PlaylistSongsList = [];
  PlaylistList = [];
  PlaylistSelected = [];
  SongsList = [];
  playlistform: FormGroup;
  SongsSelected = [];
  submittedPlaylistform = false;
  public loading = false;
  tid;
  Search: boolean = true;
  SearchText;
  cmbAlbum;
  chkSearchRadio: string = "title";
  chkMediaRadio:string="Audio";
  AlbumList = [];
  @ViewChildren("checkboxes") checkboxes: QueryList<ElementRef>;
  constructor(private formBuilder: FormBuilder, public toastr: ToastsManager, vcr: ViewContainerRef, private bService: BestoffService, config: NgbModalConfig, private modalService: NgbModal) {
    this.toastr.setRootViewContainerRef(vcr);
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    this.playlistform = this.formBuilder.group({
      plName: ["", Validators.required],
      id: [""]
    });
    this.PlaylistSongsList = [];
    this.PlaylistList = [];
    this.SongsList = [];
    this.FillBestOff();
  }
  SelectPlaylist(fileid, event) {
    this.PlaylistSelected = [];
    this.PlaylistSelected.push(fileid);
    this.loading = true;
    this.bService.PlaylistSong(fileid,"Yes").pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        this.PlaylistSongsList = obj;
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  get f() { return this.playlistform.controls; }
  onSubmitPlaylist() {
    this.submittedPlaylistform = true;
    if (this.playlistform.invalid) {
      return;
    }

    this.loading = true;
    this.bService.SaveBestPlaylist(this.playlistform.value).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Saved", 'Success!');
          this.loading = false;
          this.playlistform = this.formBuilder.group({
            plName: [" ", Validators.required],
            id: [""]
          });
          this.FillBestOff();
          this.PlaylistSongsList = [];
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
  SelectSongList(fileid, event) {
    if (event.target.checked) {
      this.SongsSelected.push(fileid);
    }
    else {
      const index: number = this.SongsSelected.indexOf(fileid);
      if (index !== -1) {
        this.SongsSelected.splice(index, 1);
      }
    }
    //    this.toastr.info(JSON.stringify(this.SongsSelected), 'Success!');
  }
  AddSong() {

    if (this.PlaylistSelected.length == 0) {
      this.toastr.error("Please select a palylist", '');
      return;
    }
    if (this.SongsSelected.length == 0) {
      this.toastr.error("Select atleast one song", '');
      return;
    }

    this.loading = true;
    this.bService.AddPlaylistSong(this.PlaylistSelected, this.SongsSelected,"BestOf").pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        this.loading = false;
        if (obj.Responce == "1") {
          this.SelectPlaylist(this.PlaylistSelected[0], "");
        }
        else {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        }
        this.checkboxes.forEach((element) => {
          element.nativeElement.checked = false;
        });
        this.SongsSelected = [];
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }
  MediaRadioClick(e) {
    this.chkMediaRadio = e;
  }

  FillBestOff() {
    this.loading = true;
    this.bService.FillBestOff().pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        this.PlaylistList = obj.lstBestPlaylist;
        this.SongsList = obj.lstSong;
        this.loading = false;
        this.SelectPlaylist(this.PlaylistList[0].id, "");
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded ,support team will get back to you soon.", '');
          this.loading = false;
        })
  }

  onPlaylistClick(id, pname) {
    this.playlistform = this.formBuilder.group({
      plName: [pname, Validators.required],
      id: [id]
    });
  }

  openTitleDeleteModal(mContent, id) {
    this.tid = id;
    this.modalService.open(mContent, { centered: true });
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
  SearchRadioClick(e) {
    this.chkSearchRadio = e;
    this.SearchText = "";
    this.Search = true;
  }
  FillAlbum() {
    this.loading = true;
    var qry = "spSearch_Album_Copyright '" + this.SearchText + "'";
    this.bService.FillCombo(qry).pipe()
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
  FillSearch() {
    this.loading = true;
    this.bService.CommanSearch(this.chkSearchRadio, this.SearchText, this.chkMediaRadio).pipe()
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
  DeleteTitle() {
    this.loading = true;
    this.bService.DeleteTitle(this.PlaylistSelected[0], this.tid,"Yes").pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "1") {
          this.toastr.info("Deleted", 'Success!');
          this.loading = false;
          this.SelectPlaylist(this.PlaylistSelected[0], "");
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
}
