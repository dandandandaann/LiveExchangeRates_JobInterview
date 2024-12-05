import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-currency-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './currency-search.component.html',
  styleUrls: ['./currency-search.component.scss']
})
export class CurrencySearchComponent {
  @Output() searchCurrencies = new EventEmitter<string[]>();
  currencies: string[] = [];
  inputValue: string = '';

  onInputChange(event: any): void {
    const value = event.target.value;
    if (value.includes(',')) {
      this.addCurrencies(value);
    }
  }

  onKeyup(event: KeyboardEvent): void {
    if (event.key === 'Enter') {
      this.addCurrencies(this.inputValue);
    } else if (event.key === 'Backspace' && this.inputValue === '') {
      this.removeLastCurrency();
    }
  }

  addCurrencies(value: string): void {
    const newCurrencies = value.split(',')
      .map(currency => currency.trim().toUpperCase())
      .filter(currency => currency && !this.currencies.includes(currency));
    this.currencies = [...this.currencies, ...newCurrencies];
    this.inputValue = '';
    this.searchCurrencies.emit(this.currencies);
  }

  removeCurrency(currency: string): void {
    this.currencies = this.currencies.filter(c => c !== currency);
    this.searchCurrencies.emit(this.currencies);
  }

  removeLastCurrency(): void {
    this.currencies.pop();
    this.searchCurrencies.emit(this.currencies);
  }
}
