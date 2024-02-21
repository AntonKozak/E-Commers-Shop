import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShopComponent } from './shop.component';
import { AppModule } from '../app.module';
import { ShopService } from './shop.service';

describe('ShopComponent', () => {
  let component: ShopComponent;
  let fixture: ComponentFixture<ShopComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ShopComponent],
      imports: [AppModule] 
    });

    fixture = TestBed.createComponent(ShopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
