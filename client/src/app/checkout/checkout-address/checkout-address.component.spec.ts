import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckoutAddressComponent } from './checkout-address.component';
import { ActivatedRoute } from '@angular/router';
import { AppModule } from 'src/app/app.module';

describe('CheckoutAddressComponent', () => {
  let component: CheckoutAddressComponent;
  let fixture: ComponentFixture<CheckoutAddressComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CheckoutAddressComponent],
      imports: [AppModule],
      providers: [
        { provide: ActivatedRoute, useValue: {} } // Provide a mock ActivatedRoute
      ]
    });
    fixture = TestBed.createComponent(CheckoutAddressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
