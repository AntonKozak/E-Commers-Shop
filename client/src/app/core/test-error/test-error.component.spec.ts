import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestErrorComponent } from './test-error.component';
import { ActivatedRoute } from '@angular/router';
import { AppModule } from 'src/app/app.module';

describe('TestErrorComponent', () => {
  let component: TestErrorComponent;
  let fixture: ComponentFixture<TestErrorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TestErrorComponent],
      imports: [AppModule],
      providers: [ { provide: ActivatedRoute, useValue: {} } ]
    });
    fixture = TestBed.createComponent(TestErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
