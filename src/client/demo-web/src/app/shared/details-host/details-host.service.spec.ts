import { TestBed, inject } from '@angular/core/testing';

import { DetailsHostService } from './details-host.service';

describe('DetailsHostService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DetailsHostService]
    });
  });

  it('should be created', inject([DetailsHostService], (service: DetailsHostService) => {
    expect(service).toBeTruthy();
  }));
});
