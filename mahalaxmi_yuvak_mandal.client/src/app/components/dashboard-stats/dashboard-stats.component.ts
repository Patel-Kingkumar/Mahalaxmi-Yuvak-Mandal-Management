import { Component } from '@angular/core';
import { DashboardService } from '../../services/dashboard.service';

@Component({
  selector: 'app-dashboard-stats',
  standalone: false,
  templateUrl: './dashboard-stats.component.html',
  styleUrl: './dashboard-stats.component.css'
})
export class DashboardStatsComponent {

  stats: any;

  constructor(private dashboardService: DashboardService) {
    this.loadStats();
  }

  loadStats() {
    this.dashboardService.getDashboardStats().subscribe({
      next: (res) => {
        this.stats = res;
        console.log("Dashboard Stats: ", this.stats);
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

}
