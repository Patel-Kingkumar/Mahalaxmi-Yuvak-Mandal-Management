import { TestBed } from '@angular/core/testing';

import { PlayerStsatsService } from './player-stsats.service';

describe('PlayerStsatsService', () => {
  let service: PlayerStsatsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PlayerStsatsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
