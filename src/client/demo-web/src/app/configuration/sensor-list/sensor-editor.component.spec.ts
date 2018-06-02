import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SensorEditorComponent } from './sensor-editor.component';

describe('SensorEditorComponent', () => {
  let component: SensorEditorComponent;
  let fixture: ComponentFixture<SensorEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SensorEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SensorEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
