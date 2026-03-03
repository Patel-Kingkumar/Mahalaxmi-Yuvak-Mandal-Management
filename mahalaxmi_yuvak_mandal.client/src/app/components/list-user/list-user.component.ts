import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-list-user',
  standalone: false,
  templateUrl: './list-user.component.html',
  styleUrl: './list-user.component.css'
})
export class ListUserComponent {

  users: any[] = [];

  constructor(private userService: UserService) {
    this.getUsers();
  }

  getUsers() {
    this.userService.getAllUsers().subscribe(users => {
      console.log("Users:", users);
      this.users = users;
    });
  }

  editUser(user: any) {
    console.log("Edit user:", user);
    // Navigate to edit page or open modal
  }

  deleteUser(userId: number) {
    if (confirm("Are you sure you want to delete this user?")) {
      console.log("Delete user with ID:", userId);
      // Call your delete API here
    }
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

}
