import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceTypeEditorComponent } from './device-type-editor.component';

describe('DeviceTypeEditorComponent', () => {
  let component: DeviceTypeEditorComponent;
  let fixture: ComponentFixture<DeviceTypeEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeviceTypeEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeviceTypeEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
