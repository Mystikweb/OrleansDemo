import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadingTypeEditorComponent } from './reading-type-editor.component';

describe('ReadingTypeEditorComponent', () => {
  let component: ReadingTypeEditorComponent;
  let fixture: ComponentFixture<ReadingTypeEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReadingTypeEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadingTypeEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
