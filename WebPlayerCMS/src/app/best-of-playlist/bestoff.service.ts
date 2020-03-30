import { Injectable } from '@angular/core';
import {ConfigAPI} from '../class/ConfigAPI';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BestoffService {

  constructor(private http:HttpClient,private cApi:ConfigAPI) { }
  FillCombo(query:string){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ Query: query });
    return this.http.post(this.cApi.FillQueryCombo,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  FillBestOff(){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    return this.http.get(this.cApi.BestOf,{headers:headers})
     .pipe((data=>{return data;}))
  }
  PlaylistSong(id:string,IsBestOffPlaylist:string){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ playlistid: id ,IsBestOffPlaylist:IsBestOffPlaylist});
    return this.http.post(this.cApi.PlaylistSong,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  SaveBestPlaylist(json:JSON){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = json;
    return this.http.post(this.cApi.SaveBestPlaylist,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  AddPlaylistSong(playlistid,titleid,AddSongFrom){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ playlistid: playlistid,titleid:titleid ,AddSongFrom:AddSongFrom});
    return this.http.post(this.cApi.AddPlaylistSong,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  CommanSearch(type,text,mediaType){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ searchType: type,searchText:text,mediaType:mediaType });
    return this.http.post(this.cApi.CommanSearch,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  DeleteTitle(pid,tid,IsBestOffPlaylist){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ playlistid: pid,titleid:tid,IsBestOffPlaylist:IsBestOffPlaylist });
    return this.http.post(this.cApi.DeleteTitle,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
}
