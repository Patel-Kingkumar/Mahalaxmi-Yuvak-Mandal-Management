import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-user',
  standalone: false,
  templateUrl: './create-user.component.html',
  styleUrl: './create-user.component.css'
})
export class CreateUserComponent {

  createUserForm!: FormGroup;

  roles = ['Admin', 'User'];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private toastr: ToastrService
  ) {
    this.initForm();
  }

  initForm() {
    this.createUserForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      role: ['User', Validators.required],
      isActive: [true]
    });
  }

  submit() {

    if (this.createUserForm.invalid) {
      this.createUserForm.markAllAsTouched();
      return;
    }

    this.userService.createUser(this.createUserForm.value)
      .subscribe({
        next: (res) => {
          // this.toastr.success('User created successfully');
          this.createUserForm.reset({ role: 'User', isActive: true });

        },
        error: (err) => {
          // this.toastr.error('Error creating user');
        }
      });

  }

}


