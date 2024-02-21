import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductItemComponent } from './product-item.component';
import { ShopModule } from '../shop.module';
import { AppModule } from 'src/app/app.module';

describe('ProductItemComponent', () => {
  let component: ProductItemComponent;
  let fixture: ComponentFixture<ProductItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProductItemComponent],
      imports:[AppModule]
    });
    fixture = TestBed.createComponent(ProductItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
