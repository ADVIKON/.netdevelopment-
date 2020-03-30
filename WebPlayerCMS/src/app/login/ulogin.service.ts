import { Injectable } from '@angular/core';
import {ConfigAPI} from '../class/ConfigAPI';
import { HttpClient, HttpHeaders,HttpErrorResponse   } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/merge';
import 'rxjs/add/operator/map';
@Injectable({
  providedIn: 'root'
})
export class UloginService {

  constructor(private http:HttpClient,private cApi:ConfigAPI) { }
  uLogin(json:JSON){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = json;
    return this.http.post(this.cApi.uLogin,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
    
}
