import { TestBed, inject } from '@angular/core/testing';

import { DeviceTypeService } from './device-type.service';

describe('DeviceTypeService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DeviceTypeService]
    });
  });

  it('should be created', inject([DeviceTypeService], (service: DeviceTypeService) => {
    expect(service).toBeTruthy();
  }));
});
