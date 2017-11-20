import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadingTypeListComponent } from './reading-type-list.component';

describe('ReadingTypeListComponent', () => {
  let component: ReadingTypeListComponent;
  let fixture: ComponentFixture<ReadingTypeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReadingTypeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadingTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
