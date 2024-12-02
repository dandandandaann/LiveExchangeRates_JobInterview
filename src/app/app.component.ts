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
    this.apiService.getCurrencies().pipe(map(x => {return x /* map incoming data for display */})) // TODO: move this data mapping to the backend
    .subscribe(
      (response) => {
        console.log(response);
      }
    );
  }
}
