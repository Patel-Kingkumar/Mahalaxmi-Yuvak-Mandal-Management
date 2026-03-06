import { Component, ElementRef, ViewChild } from '@angular/core';
import Chart from 'chart.js/auto';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {


  @ViewChild('chartCanvas') chartRef!: ElementRef;
  @ViewChild('chartCanvasBarChart') barChartRef!: ElementRef;

  users: any[] = [];
  doughnutChart: any;
  barChart: any;

  constructor(private userService: UserService) { }

  ngAfterViewInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.userService.getAllUsers().subscribe(users => {
      this.users = users;
      console.log("users:", this.users);

      // Prepare data
      const roles = this.users.map(u => u.role);
      const roleCounts: { [key: string]: number } = {};
      roles.forEach(role => roleCounts[role] = (roleCounts[role] || 0) + 1);

      const labels = Object.keys(roleCounts); // ['User','Admin']
      const data = Object.values(roleCounts); // [2,1]

      // Doughnut chart (optional if you want to keep it)
      new Chart(this.chartRef.nativeElement, {
        type: 'doughnut',
        data: {
          labels: labels,
          datasets: [{
            data: data,
            backgroundColor: ['#3f51b5', '#ff9800'],
            borderColor: '#fff',
            borderWidth: 2
          }]
        },
        options: {
          responsive: true,
          plugins: {
            legend: { position: 'top' },
            tooltip: { enabled: true }
          }
        }
      });

      // Bar chart
      new Chart(this.barChartRef.nativeElement, {
        type: 'bar',
        data: {
          labels: labels,
          datasets: [{
            label: 'Number of Users',
            data: data,
            backgroundColor: ['#3f51b5', '#ff9800'],
            borderColor: '#000',
            borderWidth: 1
          }]
        },
        options: {
          responsive: true,
          plugins: {
            legend: { display: false },
            tooltip: { enabled: true }
          },
          scales: {
            y: {
              beginAtZero: true,
              ticks: {
                stepSize: 1
              }
            }
          }
        }
      });

    });
  }
}
