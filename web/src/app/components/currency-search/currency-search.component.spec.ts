import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrencySearchComponent } from './currency-search.component';

describe('CurrencySearchComponent', () => {
  let component: CurrencySearchComponent;
  let fixture: ComponentFixture<CurrencySearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CurrencySearchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CurrencySearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
