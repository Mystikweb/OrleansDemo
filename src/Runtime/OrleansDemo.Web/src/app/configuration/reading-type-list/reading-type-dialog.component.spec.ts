import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadingTypeDialogComponent } from './reading-type-dialog.component';

describe('ReadingTypeDialogComponent', () => {
  let component: ReadingTypeDialogComponent;
  let fixture: ComponentFixture<ReadingTypeDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReadingTypeDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadingTypeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
