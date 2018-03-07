import { TestBed, inject } from '@angular/core/testing';

import { GrainService } from './grain.service';

describe('GrainService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GrainService]
    });
  });

  it('should be created', inject([GrainService], (service: GrainService) => {
    expect(service).toBeTruthy();
  }));
});
