import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppService } from './app.service';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { map } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, HttpClientModule], // TODO: try to remove HttpClientModule, it might not be needed
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'ionic-currencyexchange';

  constructor(private apiService: AppService) {

  }

  ngOnInit(): void {
    this.apiService.getCurrencies()
    .subscribe(
      (response) => {
        console.log(response);
      }
    );
  }
}
