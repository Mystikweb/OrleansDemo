import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceTypeDialogComponent } from './device-type-dialog.component';

describe('DeviceTypeDialogComponent', () => {
  let component: DeviceTypeDialogComponent;
  let fixture: ComponentFixture<DeviceTypeDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeviceTypeDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeviceTypeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
