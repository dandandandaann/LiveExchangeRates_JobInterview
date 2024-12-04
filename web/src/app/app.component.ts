import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppService } from './app.service';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { CurrencyRateComponent } from "./components/currency-rate/currency-rate.component";
import { CurrencySearchComponent } from "./components/currency-search/currency-search.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, HttpClientModule, CurrencyRateComponent, CurrencySearchComponent], // TODO: try to remove HttpClientModule, it might not be needed
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  intervalId: any;
  title = 'ionic-currencyexchange';

  constructor(private apiService: AppService) {}

  exchangeRates: any = null;
  previousExchangeRates: any = null;
  filteredCurrencies: string[] = [];

  ngOnInit(): void {
    this.fetchCurrencyRates();
    this.intervalId = setInterval(() => {
      this.fetchCurrencyRates();
    }, 2000);
  }

  ngOnDestroy(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  onSearchCurrencies(currencies: string[]): void {
    this.filteredCurrencies = currencies;
    this.fetchCurrencyRates();
  }

  private fetchCurrencyRates(): void {
    this.apiService.getCurrencies().subscribe(
      (response) => {
        const newRates = Object.entries(response.rates)
          .filter(([currency]) => this.filteredCurrencies.length === 0 || this.filteredCurrencies.includes(currency))
          .slice(0, 5)
          .map(([currency, value]) => {
            const previousRate = this.previousExchangeRates?.find((rate: any) => rate.currency === `${response.currencyPair} / ${currency}`);
            const trend = previousRate ? 
              (value > previousRate.value ? '⇧' : (value < previousRate.value ? '⇩' : previousRate.trend)) : '';
            return { currency: `${response.currencyPair} / ${currency}`, value, trend };
          });

        this.previousExchangeRates = newRates;
        this.exchangeRates = newRates;
      },
      (error) => {
        document.write(`Error: ${error.message || 'An unknown error occurred'}<br/>`);
      }
    );
  }
}
