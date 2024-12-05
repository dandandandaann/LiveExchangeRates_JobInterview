import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppService } from './app.service';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ExchangeRatesPanelComponent } from "./components/exchange-rates-panel/exchange-rates-panel.component";
import { ApiCurrencyResponse } from './models/api-currencyresponse.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, HttpClientModule, ExchangeRatesPanelComponent], // TODO: try to remove HttpClientModule, it might not be needed
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  public exchangeRates!: ApiCurrencyResponse;
  intervalId: any;
  title = 'ionic-currencyexchange';

  constructor(private appService: AppService) {}

  ngOnInit(): void {
    this.appService.getExchangeRateData().subscribe(
      (data: ApiCurrencyResponse) => {
        this.exchangeRates = data;
      },
      (error) => {
        console.error('Error receiving exchange rate data:', error);
      }
    );
    this.intervalId = setInterval(() => {
      this.appService.getExchangeRateData().subscribe(
        (data: ApiCurrencyResponse) => {
          this.exchangeRates = data;
        },
        (error) => {
          console.error('Error receiving exchange rate data:', error);
        }
      );
    }, 10000);
  }

  ngOnDestroy(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }
}
