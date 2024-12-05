import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppService } from './app.service';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ExchangeRatesPanelComponent } from "./components/exchange-rates-panel/exchange-rates-panel.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, HttpClientModule, ExchangeRatesPanelComponent], // TODO: try to remove HttpClientModule, it might not be needed
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  intervalId: any;
  title = 'ionic-currencyexchange';

  constructor(private apiService: AppService) {}

  exchangeRates: any = null;
  previousExchangeRates: any = null;

  ngOnInit(): void {
    this.fetchCurrencyRates();
    this.intervalId = setInterval(() => {
      this.fetchCurrencyRates();
    }, 10000);
  }

  ngOnDestroy(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  private fetchCurrencyRates(): void {
    this.apiService.getCurrencies().subscribe(
      (response) => {
        this.exchangeRates = response;
      },
      (error) => {
        document.write(`Error: ${error.message || 'An unknown error occurred'}<br/>`);
      }
    );
  }
}
