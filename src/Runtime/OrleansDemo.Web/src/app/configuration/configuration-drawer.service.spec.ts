import { TestBed, inject } from '@angular/core/testing';

import { ConfigurationDrawerService } from './configuration-drawer.service';

describe('ConfigurationDrawerService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ConfigurationDrawerService]
    });
  });

  it('should be created', inject([ConfigurationDrawerService], (service: ConfigurationDrawerService) => {
    expect(service).toBeTruthy();
  }));
});
