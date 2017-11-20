import { TestBed, inject } from '@angular/core/testing';

import { ReadingTypeService } from './reading-type.service';

describe('ReadingTypeService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ReadingTypeService]
    });
  });

  it('should be created', inject([ReadingTypeService], (service: ReadingTypeService) => {
    expect(service).toBeTruthy();
  }));
});
