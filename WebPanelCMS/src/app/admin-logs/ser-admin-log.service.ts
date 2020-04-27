import { Injectable } from '@angular/core';
import {ConfigAPI} from '../class/ConfigAPI';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SerAdminLogService {

  constructor(private http:HttpClient,private cApi:ConfigAPI) { }
  FillCombo(query:string){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ Query: query });
    return this.http.post(this.cApi.FillQueryCombo,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  FillAdminLogs(cid:string){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ clientId: cid});
    return this.http.post(this.cApi.FillAdminLogs,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  GetGenreList(mediatype:string,mediaStyle:string){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ mediatype: mediatype,mediaStyle:mediaStyle, ClientId:localStorage.getItem('dfClientId')});
    return this.http.post(this.cApi.GetGenreList,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  NewSavePlaylist(json){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify(json);
    return this.http.post(this.cApi.NewSavePlaylist,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
}
