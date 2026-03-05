import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-user',
  standalone: false,
  templateUrl: './list-user.component.html',
  styleUrl: './list-user.component.css'
})
export class ListUserComponent {

  users: any[] = [];

  constructor(private userService: UserService, private router: Router) {
    this.getUsers();
  }

  getUsers() {
    this.userService.getAllUsers().subscribe(users => {
      this.users = users;
    });
  }

  downloadPdfReport(): void {
    this.userService.downloadUsersPdf().subscribe({
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

  deleteUser(id: number) {

    if (!confirm('Are you sure you want to delete this user?')) {
      return;
    }

    this.userService.deleteUser(id).subscribe({
      next: () => {
        alert("User deleted successfully");

        // refresh list
        this.getUsers();
      },
      error: (err) => {
        alert(err.error);
      }
    });

  }

  editUser(user: any) {
    this.router.navigate(['/edit-user', user.id]);
  }

}
