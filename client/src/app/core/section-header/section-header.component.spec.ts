import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionHeaderComponent } from './section-header.component';
import { AppModule } from 'src/app/app.module';

describe('SectionHeaderComponent', () => {
  let component: SectionHeaderComponent;
  let fixture: ComponentFixture<SectionHeaderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SectionHeaderComponent],
      imports: [AppModule]
    });
    fixture = TestBed.createComponent(SectionHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
