import { Injectable } from '@angular/core';
import {ConfigAPI} from '../class/ConfigAPI';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class CustomerRegService {

  constructor(private http: HttpClient, private cApi: ConfigAPI) { }
  FillCombo(query: string){
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    var params = JSON.stringify({ Query: query });
    return this.http.post(this.cApi.FillQueryCombo, params, {headers: headers})
     .pipe((data => {return data; }))
  }
  FillCustomer(){
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    var params = JSON.stringify({ DBType: localStorage.getItem('DBType') });
    return this.http.post(this.cApi.FillCustomer, params, {headers: headers})
     .pipe((data => {return data; }))
  }
  SaveCustomer(json: JSON){
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    var params = json;
    return this.http.post(this.cApi.SaveCustomer, params, {headers: headers})
     .pipe((data => {return data; }))
  }
  EditClickCustomer(cid: string){
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    var params = JSON.stringify({clientId: cid});
    return this.http.post(this.cApi.EditClickCustomer, params, {headers: headers})
     .pipe((data => {return data; }))
  }
  DeleteCustomer(cid: string){
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    var params = JSON.stringify({clientId: cid});
    return this.http.post(this.cApi.DeleteCustomer, params, {headers: headers})
     .pipe((data => {return data; }))
  }
  SendMail(cid){
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    var params = JSON.stringify({clientId: cid, DBType: localStorage.getItem('DBType')});
    return this.http.post(this.cApi.SendMail, params, {headers: headers})
     .pipe((data => {return data; }))
  }
  CitySateNewModify(id, name, type, stateid, CountryId){
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    var params = JSON.stringify({id: id, name: name, type: type, stateid: stateid, CountryId: CountryId});
    return this.http.post(this.cApi.CitySateNewModify, params, {headers: headers})
     .pipe((data => {return data; }))
  }
}