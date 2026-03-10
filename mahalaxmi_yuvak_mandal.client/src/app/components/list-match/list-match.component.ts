import { Component } from '@angular/core';
import { MatchService } from '../../services/match.service';

@Component({
  selector: 'app-list-match',
  standalone: false,
  templateUrl: './list-match.component.html',
  styleUrl: './list-match.component.css'
})
export class ListMatchComponent {

  matches: any[] = [];
  userRole: string = '';

  constructor(private matchService: MatchService) { }

  ngOnInit() {
    this.userRole = sessionStorage.getItem('role') || '';
    this.getMatches();
  }

  getMatches() {
    this.matchService.getAllMatches().subscribe((res: any) => {
      this.matches = res;
    });
  }

  deleteMatch(id: number) {
    if (confirm('Are you sure you want to delete this match?')) {
      this.matchService.deleteMatch(id).subscribe(() => {
        this.getMatches();
      });
    }

  }

  downloadPdfReport() {
    this.matchService.downloadMatchesPdf().subscribe({
      next: (data: Blob) => {
        // Create a local URL for the binary data
        const blob = new Blob([data], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);

        // Create a temporary hidden link and click it
        const link = document.createElement('a');
        link.href = url;
        link.download = `Matches_List_${new Date().toLocaleDateString()}.pdf`;
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
