import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DonationService } from '../../services/donation.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-create-donation',
  standalone: false,
  templateUrl: './create-donation.component.html',
  styleUrl: './create-donation.component.css'
})
export class CreateDonationComponent {


  donationForm: FormGroup | any;

  users: any[] = [];

  celebrations = [
    { id: 1, name: 'Mahalaxmi Mataji Salgiri' },
    { id: 2, name: 'Janmashtami' },
    { id: 3, name: 'Ganesh Mahotsav' },
    { id: 4, name: 'Navratri Mahotsav' },
    { id: 5, name: 'Dahanu Padyatra' }
  ];

  constructor(
    private fb: FormBuilder,
    private donationService: DonationService,
    private userService: UserService
  ) { }

  ngOnInit(): void {

    this.donationForm = this.fb.group({
      userId: ['', Validators.required],
      celebrationId: ['', Validators.required],
      amount: ['', Validators.required],
      year: ['', Validators.required]
    });

    this.loadUsers();
  }

  loadUsers() {
    this.userService.getAllUsers().subscribe(res => {
      this.users = res;
    });
  }

  submit() {

    const formValue = this.donationForm.value;

    const payload = {
      userId: Number(formValue.userId),
      celebrationId: Number(formValue.celebrationId),
      amount: Number(formValue.amount),
      year: Number(formValue.year)
    };
    console.log('Payload:', payload);
    this.donationService.createDonation(payload).subscribe({
      next: (res) => {
        console.log(res);
      }
    });

  }


}
