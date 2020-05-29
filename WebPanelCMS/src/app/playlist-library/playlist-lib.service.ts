import { Injectable } from '@angular/core';
import {ConfigAPI} from '../class/ConfigAPI';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PlaylistLibService {

  constructor(private http:HttpClient,private cApi:ConfigAPI) { }
  FillCombo(query:string){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ Query: query });
    return this.http.post(this.cApi.FillQueryCombo,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  Playlist(id:string){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ Id: id });
    return this.http.post(this.cApi.Playlist,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  PlaylistSong(id:string,IsBestOffPlaylist:string){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ playlistid: id,IsBestOffPlaylist:IsBestOffPlaylist });
    return this.http.post(this.cApi.PlaylistSong,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  CommanSearch(type,text,mediaType,IsExplicit){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ searchType: type,searchText:text,mediaType:mediaType, IsRf:localStorage.getItem('IsRf'), ClientId:localStorage.getItem('dfClientId'),IsExplicit:IsExplicit });
    return this.http.post(this.cApi.CommanSearch,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  DeleteTitle(pid,tid,IsBestOffPlaylist){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ playlistid: pid,titleid:tid ,IsBestOffPlaylist:IsBestOffPlaylist});
    return this.http.post(this.cApi.DeleteTitle,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  SavePlaylist(json:JSON){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = json;
    return this.http.post(this.cApi.SavePlaylist,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  SavePlaylistFromBestOf(id,plName,formatid,isBestOff){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ id: id,plName:plName ,formatid:formatid,isBestOff:isBestOff});
    return this.http.post(this.cApi.SavePlaylistFromBestOf,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  AddPlaylistSong(playlistid,titleid,AddSongFrom){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ playlistid: playlistid,titleid:titleid ,AddSongFrom:AddSongFrom});
    return this.http.post(this.cApi.AddPlaylistSong,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  FillSongList(mediaType,IsExplicit){
    var params = JSON.stringify({ searchType: "",searchText:"",mediaType:mediaType , IsRf:localStorage.getItem('IsRf'), ClientId:localStorage.getItem('dfClientId'),IsExplicit:IsExplicit });
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    return this.http.post(this.cApi.SongList,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  DeletePlaylist(pid,forceDelete){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ playlistid: pid,titleid:"" ,IsBestOffPlaylist:"",IsForceDelete:forceDelete});
    return this.http.post(this.cApi.DeletePlaylist,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  SaveFormat(id,fname){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ id: id,formatname:fname, dfclientId: localStorage.getItem('dfClientId') });
    return this.http.post(this.cApi.SaveFormat,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  SettingPlaylist(pid, chkMute, chkFixed){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ playlistid: pid,chkMute:chkMute,chkFixed:chkFixed});
    return this.http.post(this.cApi.SettingPlaylist,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  UpdatePlaylistSRNo(pid, json){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ playlistid: pid,lstTitleSR:json});
    console.log(params);
    return this.http.post(this.cApi.UpdatePlaylistSRNo,params,{headers:headers})
     .pipe((data=>{return data;}))
  }

  
SaveModifyLogs(tokenid:string, ModifyData:string){
  var UserId= localStorage.getItem('UserId');
  var dfclientid =localStorage.getItem('dfClientId');
  var IPAddress =localStorage.getItem('ipAddress');
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ dfclientid: dfclientid,IPAddress:IPAddress,ModifyData:ModifyData,UserId:UserId,EffectToken:tokenid });
    return this.http.post(this.cApi.SaveModifyLogs,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  DeleteFormat(id,IsForceDelete){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ formatId: id,IsForceDelete:IsForceDelete});
    return this.http.post(this.cApi.DeleteFormat,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  FillTokenInfo_formatANDplaylist(formatId,playlistId){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ formatId: formatId ,playlistId: playlistId});
    return this.http.post(this.cApi.FillTokenInfo_formatANDplaylist,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  CopyFormat(FormatId,CopyFormatId){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ FormatId: FormatId,CopyFormatId:CopyFormatId, dfclientId: localStorage.getItem('dfClientId') });
    return this.http.post(this.cApi.CopyFormat,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  DeleteTitlePercentage(pid,tPercentage){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ playlistid: pid,titlepercentage:tPercentage});
    
    return this.http.post(this.cApi.DeleteTitlePercentage,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
}
