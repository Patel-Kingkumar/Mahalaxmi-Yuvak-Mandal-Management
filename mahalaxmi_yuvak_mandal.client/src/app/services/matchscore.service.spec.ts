import { TestBed } from '@angular/core/testing';

import { MatchscoreService } from './matchscore.service';

describe('MatchscoreService', () => {
  let service: MatchscoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MatchscoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
