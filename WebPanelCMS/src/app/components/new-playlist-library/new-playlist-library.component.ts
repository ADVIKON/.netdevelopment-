import { Component, OnInit, ViewContainerRef } from '@angular/core';

import { ToastrService } from 'ngx-toastr';
import * as $ from 'jquery';
import * as CanvasJs from 'src/assets/canvasjs.min.js'
import { SerAdminLogService } from 'src/app/admin-logs/ser-admin-log.service';
@Component({
  selector: 'app-new-playlist-library',
  templateUrl: './new-playlist-library.component.html',
  styleUrls: ['./new-playlist-library.component.css']
})
export class NewPlaylistLibraryComponent implements OnInit {
  //"./node_modules/jquery/dist/jquery.min.js",  
  data = [];
  JsonList = [];
  JsonItem = {};
  GraphList = [];
  public loading = false;
  mediatype = "Audio";
  mediaStyle = "Copyright";
  page: number = 1;
  pageSize: number = 20;
  constructor(private adminService: SerAdminLogService, public toastr: ToastrService, vcr: ViewContainerRef) {
  
  }

  ngOnInit() {
    $("#rdoAudio").prop("checked", true);
    this.mediatype = "Audio";
    this.JsonList=[];
    this.NewGenreList=[];
    this.GraphList=[];
    this.GetGenreList();
    


  }
  Chart() {
    let chart = new CanvasJs.Chart("chartContainer", {
      animationEnabled: true,
      exportEnabled: false,
      legend: {
        horizontalAlign: "right",
        verticalAlign: "center"
      },
      data: [{
        type: "pie",
        showInLegend: true,
        toolTipContent: "{name} (#percent%)",
        indexLabel: "(#percent%)",
        legendText: "{name}",
        indexLabelPlacement: "inside",
        dataPoints: this.GraphList
      }]
    });

    chart.render();
  }

  MediaType(mType) {
    this.mediatype = mType;
    this.GetGenreList();
  }
  MediaStyle(mSyle) {
    this.mediaStyle = mSyle;
    this.GetGenreList();
  }


  GetGenreList() {
   // this.JsonList = [];
   // this.GraphList = [];
    this.loading = true;

    this.adminService.GetGenreList(this.mediatype, this.mediaStyle).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        this.data = JSON.parse(returnData);
        this.loadPage(0);
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  };

  SavePlaylist() {


    var mediaStyle, mediatype;

    if ($("#txtPlaylistName").val() == "") {
      this.toastr.info('Playlist name cannot be blank')
      return;
    }
    if ($("#txtSongs").val() == "") {
      this.toastr.info('Number of songs cannot be blank')
      return;
    }

    if (this.NewGenreList.length == 0) {
      this.toastr.info('Select atleast one genre')
      return;
    }


    if ($("#rdoCopyright").prop("checked")) {
      mediaStyle = $("#rdoCopyright").val();
    }
    if ($("#rdoDirect").prop("checked")) {
      mediaStyle = $("#rdoDirect").val();
    }

    if ($("#rdoVideo").prop("checked")) {
      mediatype = $("#rdoVideo").val();
    }
    if ($("#rdoAudio").prop("checked")) {
      mediatype = $("#rdoAudio").val();
    }

    this.JsonItem["plName"] = $("#txtPlaylistName").val();
    this.JsonItem["MediaType"] = mediatype;
    this.JsonItem["MediaStyle"] = mediaStyle;
    this.JsonItem["TotalSongs"] = $("#txtSongs").val();
    this.JsonItem["formatid"] = localStorage.getItem("FormatID");

    this.JsonItem["lstGenrePer"] = this.NewGenreList;

    this.JsonList.push(this.JsonItem);

    this.loading = true;

    this.adminService.NewSavePlaylist(this.JsonList).pipe()
      .subscribe(data => {
        var returnData = JSON.stringify(data);
        var obj = JSON.parse(returnData);
        if (obj.Responce == "2") {
          this.toastr.info("Songs are not found with current pattern.", '');
        }
        if (obj.Responce == "1") {
          this.toastr.info("Playlist is Saved", '');
          $("#txtPlaylistName").val('');
          $("#txtSongs").val('');
          this.GetGenreList();
        }
        if (obj.Responce == "0") {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
        }
        this.loading = false;
      },
        error => {
          this.toastr.error("Apologies for the inconvenience.The error is recorded.", '');
          this.loading = false;
        })
  }
  NewGenreList = [];
  onChangeRange(e, gid, gname) {
    var GenreItem = {}
    var GraphItem = {}
    var k = $("#chkGenre" + gid).prop("checked");
    if (k == false) {
      if (e != 0) {
        k = true;
        $("#chkGenre" + gid).prop("checked", true)
      }
    }
    if (k == true) {
      if (e != 0) {


        GenreItem["GenreId"] = gid;
        GenreItem["GenrePercentage"] = e;

        this.removeDuplicateRecord(GenreItem);
        this.NewGenreList.push(GenreItem);

        GraphItem["y"] = e * 100;
        GraphItem["name"] = gname;
        this.GraphRemoveDuplicateRecord(GraphItem);
        this.GraphList.push(GraphItem);
      }
      else {
        $("#chkGenre" + gid).prop("checked", false)
        GenreItem["GenreId"] = gid;
        GenreItem["GenrePercentage"] = "0";
        this.removeDuplicateRecord(GenreItem);

        GraphItem["y"] = "0";
        GraphItem["name"] = gname;
        this.GraphRemoveDuplicateRecord(GraphItem);
      }
      this.Chart();
    }

  }

  onChangeCheckBox(e, gid, gname) {
    var GenreItem = {}
    var GraphItem = {}
    if (e.target.checked == true) {
      var k = $("#" + gid).val();
      if (k != 0) {
        GenreItem["GenreId"] = gid;
        GenreItem["GenrePercentage"] = k;
        this.removeDuplicateRecord(GenreItem);
        this.NewGenreList.push(GenreItem);

        GraphItem["y"] = e * 100;
        GraphItem["name"] = gname;
        this.GraphRemoveDuplicateRecord(GraphItem);
        this.GraphList.push(GraphItem);

      }
    }
    else {
      $("#" + gid).val('0');
      GenreItem["GenreId"] = gid;
      GenreItem["GenrePercentage"] = "0";
      this.removeDuplicateRecord(GenreItem);

      GraphItem["y"] = "0";
      GraphItem["name"] = gname;
      this.GraphRemoveDuplicateRecord(GraphItem);
    }
    this.Chart();
  }


  removeDuplicateRecord = (array): void => {
    this.NewGenreList = this.NewGenreList.filter(order => order.GenreId !== array.GenreId);
  }
  GraphRemoveDuplicateRecord = (array): void => {
    this.GraphList = this.GraphList.filter(order => order.name !== array.name);
  }

  loadPage(page) {
    for (var i = 0; i < this.data.length; i++) {
      var ismatch = false;
      for (var j = 0; j < this.NewGenreList.length; j++) {
        
        if (this.data[i].genreid == this.NewGenreList[j].GenreId) {
          ismatch = true;
          this.data[i].iChecked = true;
          this.data[i].GenrePercentage = this.NewGenreList[j].GenrePercentage;
          break;
        }
      }
      if (!ismatch) {
        this.data[i].iChecked = false;
        this.data[i].GenrePercentage=0;
      } 
    }
  };
  ClearList(){
    $("#rdoAudio").prop("checked", true);
    this.mediatype = "Audio";
    this.JsonList=[];
    this.NewGenreList=[];
    this.GraphList=[];
    this.GetGenreList();
    this.Chart();
  }
}
