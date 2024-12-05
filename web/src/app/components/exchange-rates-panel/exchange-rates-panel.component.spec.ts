import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExchangeRatesPanelComponent } from './exchange-rates-panel.component';

describe('ExchangeRatesPanelComponent', () => {
  let component: ExchangeRatesPanelComponent;
  let fixture: ComponentFixture<ExchangeRatesPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExchangeRatesPanelComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExchangeRatesPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
