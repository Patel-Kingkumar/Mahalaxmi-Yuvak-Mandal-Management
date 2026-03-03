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
}
