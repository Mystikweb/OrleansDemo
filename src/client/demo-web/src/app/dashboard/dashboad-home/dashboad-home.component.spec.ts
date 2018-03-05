import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboadHomeComponent } from './dashboad-home.component';

describe('DashboadHomeComponent', () => {
  let component: DashboadHomeComponent;
  let fixture: ComponentFixture<DashboadHomeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboadHomeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboadHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
