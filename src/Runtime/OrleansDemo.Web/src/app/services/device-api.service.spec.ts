import { TestBed, inject } from '@angular/core/testing';

import { DeviceApiService } from './device-api.service';

describe('DeviceApiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DeviceApiService]
    });
  });

  it('should be created', inject([DeviceApiService], (service: DeviceApiService) => {
    expect(service).toBeTruthy();
  }));
});
