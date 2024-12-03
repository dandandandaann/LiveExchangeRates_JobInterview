import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-currency-rate',
  imports: [],
  templateUrl: './currency-rate.component.html',
  styleUrl: './currency-rate.component.scss'
})
export class CurrencyRateComponent {
  @Input() currency!: string;
  @Input() rate!: number;
}