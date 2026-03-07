import { Component, OnInit } from '@angular/core';
import { DonationService } from '../../services/donation.service';

@Component({
  selector: 'app-list-donation',
  standalone: false,
  templateUrl: './list-donation.component.html',
  styleUrl: './list-donation.component.css'
})
export class ListDonationComponent implements OnInit {


  donations: any[] = [];
  userRole: string = '';
  userId: any = 0;

  constructor(private donationService: DonationService) {
    this.getDonations();
  }


  ngOnInit(): void {
    this.userRole = sessionStorage.getItem('role') || '';
    this.userId = sessionStorage.getItem('userId') || 0;
    console.log('User Role:', this.userRole);
    console.log('User ID:', this.userId);
  }



  getDonations() {

    this.userRole = sessionStorage.getItem('role') || '';
    this.userId = Number(sessionStorage.getItem('userId'));

    // Admin → all donations
    if (this.userRole === 'Admin') {

      this.donationService.getDonations().subscribe({
        next: (res) => {
          this.donations = res.map((d: any) => ({
            ...d,
            donationDate: new Date(d.donationDate)
          }));
        }
      });

    }

    else {

      this.donationService.getDonations(this.userId).subscribe({
        next: (res) => {
          this.donations = res.map((d: any) => ({
            ...d,
            donationDate: new Date(d.donationDate)
          }));
        }
      });

    }

  }

  downloadPdfReport() {
    const userId = Number(sessionStorage.getItem('userId'));
    const role = sessionStorage.getItem('role') || 'User';

    if (!userId) {
      alert("Session expired. Please login again.");
      return;
    }

    this.donationService.downloadDonationReport(userId, role).subscribe({
      next: (data: Blob) => {
        // Create a local URL for the binary data
        const blob = new Blob([data], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);

        // Create a temporary hidden link and click it
        const link = document.createElement('a');
        link.href = url;
        link.download = `Users_List_${new Date().toLocaleDateString()}.pdf`;
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
