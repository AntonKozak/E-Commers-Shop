import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderDetailComponent } from './order-detail.component';
import { AppModule } from 'src/app/app.module';

describe('OrderDetailComponent', () => {
  let component: OrderDetailComponent;
  let fixture: ComponentFixture<OrderDetailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OrderDetailComponent],
      imports: [AppModule]
    });
    fixture = TestBed.createComponent(OrderDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
