import { Component } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../shared/services/auth.service';
import { ToastService } from '../shared/services/toast-service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {

  loginData = {
    userName: '',
    password: ''
  };

  constructor(
    private authService: AuthService,
    private router: Router,
    private toastService: ToastService
  ) {}

  login(): void {

    if (
      !this.loginData.userName.trim() ||
      !this.loginData.password.trim()
    ) {
      this.toastService.show(
        'Please enter username and password',
        'error'
      );

      return;
    }

    this.authService
      .login(this.loginData)
      .subscribe({
        next: () => {
          this.toastService.show(
            'Login successful',
            'success'
          );

          this.router.navigate([
            '/dashboard'
          ]);
        },

        error: (err) => {
          console.error(
            'Login failed',
            err
          );

          this.toastService.show(
            'Invalid username or password',
            'error'
          );
        }
      });
  }
}