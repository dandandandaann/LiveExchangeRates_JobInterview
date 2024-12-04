import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-currency-rate',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './currency-rate.component.html',
  styleUrls: ['./currency-rate.component.scss']
})
export class CurrencyRateComponent {
  @Input() currency!: string;
  @Input() rate!: number;
  @Input() trend!: string;
}