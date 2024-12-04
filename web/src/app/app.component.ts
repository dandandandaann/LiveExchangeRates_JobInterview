import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppService } from './app.service';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { CurrencyRateComponent } from "./components/currency-rate/currency-rate.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, HttpClientModule, CurrencyRateComponent], // TODO: try to remove HttpClientModule, it might not be needed
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'ionic-currencyexchange';

  constructor(private apiService: AppService) {

  }
  exchangeRates: any = null;
  ngOnInit(): void {
    this.apiService.getCurrencies()
      .subscribe(
        (response) => {
          this.exchangeRates = Object.entries(response.rates)
            // .sort(() => Math.random() - 0.5)
            .slice(0, 3)
            .map(([currency, value]) => ({currency: `${response.currencyPair} / ${currency}`, value}))

          return response;
        }
      );
  }
}
