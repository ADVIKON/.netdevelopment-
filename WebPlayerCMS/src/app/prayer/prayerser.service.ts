import { Injectable } from '@angular/core';
import {ConfigAPI} from '../class/ConfigAPI';
import { HttpClient, HttpHeaders ,HttpEvent, HttpErrorResponse, HttpEventType} from '@angular/common/http';
import { throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class PrayerserService {
 
  constructor(private http:HttpClient,private cApi:ConfigAPI) { }
  FillClientCombo(){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ Query: "select DFClientID as id,  ClientName  as displayname from DFClients where CountryCode is not null and DFClients.IsDealer=1 order by RIGHT(ClientName, LEN(ClientName) - 3)" });
    return this.http.post(this.cApi.FillQueryCombo,params,{headers:headers})
     .pipe((data=>{return data;}))
  } 
  FillTokenInfo(cid){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ clientId: cid});
    return this.http.post(this.cApi.FillTokenInfo,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  FillTokenInfoPrayer(cid){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ clientId: cid});
    return this.http.post(this.cApi.FillTokenInfoAds,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  SavePrayer(json:JSON){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = json;
    return this.http.post(this.cApi.SavePrayer,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  FillSearchPayer(cDate,tid){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ cDate: cDate,tokenid:tid});
    return this.http.post(this.cApi.FillSearchPayer,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  DeletePrayer(id){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ id: id});
    return this.http.post(this.cApi.DeletePrayer,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
}
