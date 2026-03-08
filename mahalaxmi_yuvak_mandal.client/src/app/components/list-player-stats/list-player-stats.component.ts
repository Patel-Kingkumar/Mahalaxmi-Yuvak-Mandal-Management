import { Component } from '@angular/core';
import { PlayerStsatsService } from '../../services/player-stsats.service';

@Component({
  selector: 'app-list-player-stats',
  standalone: false,
  templateUrl: './list-player-stats.component.html',
  styleUrls: ['./list-player-stats.component.css']
})
export class ListPlayerStatsComponent {

  stats: any[] = [];

  constructor(private statsService: PlayerStsatsService) {
    this.getStats();
  }

  getStats() {

    this.statsService.getAllPlayerStats()
      .subscribe({
        next: (res: any) => {
          this.stats = res;
        },
        error: (err) => {
          console.log(err);
        }
      })

  }

}
