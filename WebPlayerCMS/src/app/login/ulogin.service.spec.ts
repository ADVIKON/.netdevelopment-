import { TestBed } from '@angular/core/testing';

import { UloginService } from './ulogin.service';

describe('UloginService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UloginService = TestBed.get(UloginService);
    expect(service).toBeTruthy();
  });
});
