import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-edit-user',
  standalone: false,
  templateUrl: './edit-user.component.html',
  styleUrl: './edit-user.component.css'
})
export class EditUserComponent {

  editForm: FormGroup | any;
  userId!: number;

  roles = ['Admin', 'User'];

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router
  ) { }

  ngOnInit() {

    this.userId = this.route.snapshot.params['id'];

    this.editForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      role: ['', Validators.required],
      isActive: [true]
    });

    this.loadUser();
  }

  loadUser() {

    this.userService.getUserById(this.userId).subscribe(res => {

      this.editForm.patchValue({
        fullName: res.fullName,
        email: res.email,
        role: res.role,
        isActive: res.isActive
      });

    });

  }

  updateUser() {

    if (this.editForm.invalid) return;

    const formData = {
      ...this.editForm.value,
      isActive: this.editForm.value.isActive === true || this.editForm.value.isActive === 'true'
    };

    this.userService.updateUser(this.userId, formData)
      .subscribe({
        next: () => {
          alert("User updated successfully");
          this.router.navigate(['/list-users']);
        },
        error: (err) => {
          console.log(err);
        }
      });

  }


}
