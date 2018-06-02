import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailsHostComponent } from './details-host.component';

describe('DetailsHostComponent', () => {
  let component: DetailsHostComponent;
  let fixture: ComponentFixture<DetailsHostComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetailsHostComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailsHostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
