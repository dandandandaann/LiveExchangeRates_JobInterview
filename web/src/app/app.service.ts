import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { ApiCurrencyResponse } from './models/api-currencyresponse.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AppService {
  private hubConnection!: signalR.HubConnection;
  private exchangeRateDataSubject = new Subject<ApiCurrencyResponse>();

  baseApiUrl = 'http://localhost:5024'
  constructor(private http: HttpClient) {
    this.startConnection();
    this.addTransferExchangeRateDataListener();
  }

  private startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl( `${this.baseApiUrl}/exchangerateshub`)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  private addTransferExchangeRateDataListener() {
    this.hubConnection.on('transferExchangeRateData', (data: ApiCurrencyResponse) => {
      this.exchangeRateDataSubject.next(data);
    });
  }

  getExchangeRateData(): Observable<ApiCurrencyResponse> {
    return this.exchangeRateDataSubject.asObservable();
  }
}