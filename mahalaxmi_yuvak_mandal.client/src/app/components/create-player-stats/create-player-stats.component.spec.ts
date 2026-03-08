import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePlayerStatsComponent } from './create-player-stats.component';

describe('CreatePlayerStatsComponent', () => {
  let component: CreatePlayerStatsComponent;
  let fixture: ComponentFixture<CreatePlayerStatsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreatePlayerStatsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatePlayerStatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
