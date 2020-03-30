import { Injectable } from '@angular/core';
import {ConfigAPI} from '../class/ConfigAPI';
import { HttpClient, HttpHeaders } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class CustomerRegService {

  constructor(private http:HttpClient,private cApi:ConfigAPI) { }
  FillCombo(query:string){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({ Query: query });
    return this.http.post(this.cApi.FillQueryCombo,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  FillCustomer(){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    return this.http.get(this.cApi.FillCustomer,{headers:headers})
     .pipe((data=>{return data;}))
  }
  SaveCustomer(json:JSON){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = json;
    return this.http.post(this.cApi.SaveCustomer,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  EditClickCustomer(cid:string){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({clientId:cid});
    return this.http.post(this.cApi.EditClickCustomer,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
  DeleteCustomer(cid:string){
    let headers = new HttpHeaders({ 'Content-Type':'application/json' });
    var params = JSON.stringify({clientId:cid});
    return this.http.post(this.cApi.DeleteCustomer,params,{headers:headers})
     .pipe((data=>{return data;}))
  }
}
