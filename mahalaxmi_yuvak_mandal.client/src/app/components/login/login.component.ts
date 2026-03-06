import { UserService } from './../../services/user.service';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms'; // ✅ Add this

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  loginForm: FormGroup | any;
  forgotForm: FormGroup;
  otpForm: FormGroup;

  showForgot = false;
  showOTP = false;
  message = '';


  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      roleName: ['User', Validators.required]
    });

    this.forgotForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });

    this.otpForm = this.fb.group({
      email: ['', Validators.required],
      otp: ['', Validators.required],
      newPassword: ['', Validators.required]
    });
  }

  login() {
    if (this.loginForm.invalid) return;

    this.auth.login(this.loginForm.value).subscribe({
      next: res => {
        console.log("res  : ", res);
        // ✅ Store JWT Token
        sessionStorage.setItem('token', res.token);

        // ✅ Store user role
        sessionStorage.setItem('role', res.role);
        this.message = 'Login successful!';
        this.router.navigate(['/dashboard']);   // ✅ Redirect here
      },
      error: err => {
        this.message = 'Invalid credentials or role.';
      }
    });
  }

  forgotPassword() {
    this.showForgot = true;
  }

  sendOTP() {
    if (this.forgotForm.invalid) return;

    const email = this.forgotForm.value.email;
    this.auth.sendOTP(email).subscribe({
      next: res => {
        this.showOTP = true;
        this.otpForm.patchValue({ email });
        this.message = 'OTP sent to your email!';
      },
      error: err => this.message = 'Email not found.'
    });
  }

  resetPassword() {
    if (this.otpForm.invalid) return;

    this.auth.resetPassword(this.otpForm.value).subscribe({
      next: res => {
        this.message = 'Password reset successful! Login now.';
        this.showForgot = false;
        this.showOTP = false;
        this.loginForm.patchValue({ email: this.otpForm.value.email });
      },
      error: err => this.message = 'OTP invalid or expired.'
    });
  }
}
