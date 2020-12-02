import { TestBed } from '@angular/core/testing';

import { JwtauthService } from './jwtauth.service';

describe('JwtauthService', () => {
  let service: JwtauthService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JwtauthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
