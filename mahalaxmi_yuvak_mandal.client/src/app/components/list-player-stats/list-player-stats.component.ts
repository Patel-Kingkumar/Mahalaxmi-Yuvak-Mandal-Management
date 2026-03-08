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

  downloadPdfReport() {
    this.statsService.downloadPlayerStatsPdf().subscribe({
      next: (data: Blob) => {
        // Create a local URL for the binary data
        const blob = new Blob([data], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);

        // Create a temporary hidden link and click it
        const link = document.createElement('a');
        link.href = url;
        link.download = `Player_Stats_${new Date().toLocaleDateString()}.pdf`;
        link.click();

        // Cleanup
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        console.error('Error downloading the PDF:', error);
        // Optional: Add a toast notification here
      }
    });
  }

}
