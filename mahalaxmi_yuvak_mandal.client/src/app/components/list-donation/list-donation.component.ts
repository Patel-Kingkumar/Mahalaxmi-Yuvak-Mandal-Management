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


}
