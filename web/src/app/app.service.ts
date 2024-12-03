import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiCurrencyResponse } from './models/api-currencyresponse.model';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class AppService {

  baseApiUrl = 'https://api.fxratesapi.com/';
  constructor(private http: HttpClient) {

  }

   getCurrencies(): Observable<ApiCurrencyResponse> {
    return this.http.get<ApiCurrencyResponse>(`${this.baseApiUrl}/latest`);
   }
}