import { Component, Input, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CurrencySearchComponent } from "../currency-search/currency-search.component";
import { CurrencyRateComponent } from "../currency-rate/currency-rate.component";
import { ApiCurrencyResponse } from '../../models/api-currencyresponse.model';

@Component({
  selector: 'app-exchange-rates-panel',
  standalone: true,
  imports: [CommonModule, CurrencySearchComponent, CurrencyRateComponent],
  templateUrl: './exchange-rates-panel.component.html',
  styleUrls: ['./exchange-rates-panel.component.scss']
})
export class ExchangeRatesPanelComponent {
  @Input() inputExchangeRates!: ApiCurrencyResponse;

  filteredCurrencies: string[] = [];
  previousExchangeRates!: { currency: string; currencyPair: string; value: number; trend: any; }[];
  outputExchangeRates!: { currency: string; currencyPair: string; value: number; trend: any; }[];

  ngOnChanges(_changes: SimpleChanges) {
    if (this.inputExchangeRates) {
      this.updateCurrencyRates();
    }
  }

  onSearchCurrencies(currencies: string[]): void {
    this.filteredCurrencies = currencies;
    this.updateCurrencyRates();
  }

  private updateCurrencyRates(): void {
    const newRates = Object.entries(this.inputExchangeRates.rates)
      .map(([currency, value]) => {
        const previousRate = this.previousExchangeRates?.find((rate: any) => rate.currency === currency);
        const trend = previousRate ?
          (value > previousRate.value ? '⇧' : (value < previousRate.value ? '⇩' : previousRate.trend)) : '';
        return { currency: currency, currencyPair: this.inputExchangeRates.currencyPair, value, trend };
      });


    this.previousExchangeRates = newRates;

    if (this.filteredCurrencies.length) {
      this.outputExchangeRates = newRates
        .filter(x => this.filteredCurrencies.length === 0 || this.filteredCurrencies.includes(x.currency));
    }
    else {
      this.outputExchangeRates = newRates
        .sort(() => Math.random() - 0.5)
        .slice(0, 5);
    }
  }
}
