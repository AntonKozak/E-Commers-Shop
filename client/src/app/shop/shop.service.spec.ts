import { TestBed } from '@angular/core/testing';

import { ShopService } from './shop.service';
import { AppModule } from '../app.module';

describe('ShopService', () => {
  let service: ShopService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [AppModule],
    });
    service = TestBed.inject(ShopService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
