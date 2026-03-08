import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListPlayerStatsComponent } from './list-player-stats.component';

describe('ListPlayerStatsComponent', () => {
  let component: ListPlayerStatsComponent;
  let fixture: ComponentFixture<ListPlayerStatsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ListPlayerStatsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListPlayerStatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
