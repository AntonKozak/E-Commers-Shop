import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckoutDeliveryComponent } from './checkout-delivery.component';
import { CheckoutModule } from '../checkout.module';
import { AppModule } from 'src/app/app.module';

describe('CheckoutDeliveryComponent', () => {
  let component: CheckoutDeliveryComponent;
  let fixture: ComponentFixture<CheckoutDeliveryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CheckoutDeliveryComponent],
      imports: [AppModule]
    });
    fixture = TestBed.createComponent(CheckoutDeliveryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
