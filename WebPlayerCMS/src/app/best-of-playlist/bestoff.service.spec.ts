import { TestBed } from '@angular/core/testing';

import { BestoffService } from './bestoff.service';

describe('BestoffService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BestoffService = TestBed.get(BestoffService);
    expect(service).toBeTruthy();
  });
});
