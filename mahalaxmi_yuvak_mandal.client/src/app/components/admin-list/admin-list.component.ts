import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-list',
  standalone: false,
  templateUrl: './admin-list.component.html',
  styleUrl: './admin-list.component.css'
})
export class AdminListComponent implements OnInit {


  users: any[] = [];
  userRole: string = '';
  constructor(
    private userService: UserService,
    private authUserService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.getUsers();
  }

  ngOnInit(): void {
    this.userRole = sessionStorage.getItem('role') || '';
  }

  getUsers() {
    this.userService.getAllUsers().subscribe(users => {
      this.users = users.filter(user => user.role === 'Admin');
    });
  }

  downloadPdfReport(): void {
    this.authUserService.downloadAdminsPdf().subscribe({
      next: (data: Blob) => {
        // Create a local URL for the binary data
        const blob = new Blob([data], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);

        // Create a temporary hidden link and click it
        const link = document.createElement('a');
        link.href = url;
        link.download = `Admins_List_${new Date().toLocaleDateString()}.pdf`;
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
        this.toastr.success('User deleted successfully');

        // refresh list
        this.getUsers();
      },
      error: (err) => {
        this.toastr.error('Error deleting user');
      }
    });

  }

  editUser(user: any) {
    this.router.navigate(['/edit-user', user.id]);
  }
}
