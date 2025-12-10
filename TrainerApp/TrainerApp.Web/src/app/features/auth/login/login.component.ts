import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule], // <-- RouterModule додано!
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  email = '';
  password = '';
  error = '';
isLoading = false;
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login() {
    this.error = '';
    this.isLoading = true;
    this.authService.login({ email: this.email, password: this.password })
      .subscribe({
        next: () => {
this.isLoading = false;        },
        error: (err) => {
          this.isLoading = false;
          this.error = err.error?.message || 'Помилка входу';
        }
      });
  }

  goToRegister() {
    this.router.navigate(['/auth/register']);
  }
}
