// src/app/features/auth/register/register.component.ts
import { Component } from '@angular/core';
import { AuthService, RegisterRequest } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  // Крок реєстрації
  step: 'role' | 'details' = 'role';

  // Обрана роль (null — поки не обрано)
  role: 'Client' | 'Trainer' | null = null;

  // Єдиний об'єкт форми (відповідає RegisterDto на бекенді)
  form: RegisterRequest = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    birthdate: '',           // буде заповнено через <input type="date">
    role: 'Client',          // буде перезаписано при виборі ролі
    heightCm: undefined,
    currentWeightKg: undefined,
    bio: undefined,
    avatarUrl: undefined
  };

  confirmPassword = '';
  error = '';
  success = false;
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  // Викликається при виборі ролі
  selectRole(role: 'Client' | 'Trainer') {
    this.role = role;
    this.form.role = role;
    this.step = 'details';
  }

  // Повернення до вибору ролі
  goBack() {
    this.step = 'role';
    this.role = null;
  }

  // Реєстрація
  register() {
    this.error = '';

    if (this.form.password !== this.confirmPassword) {
      this.error = 'Паролі не співпадають';
      return;
    }

    if (!this.form.birthdate) {
      this.error = 'Вкажіть дату народження';
      return;
    }

    this.isLoading = true;

    // Очищаємо поля, які не потрібні для обраної ролі
    const data: RegisterRequest = { ...this.form };
    if (data.role === 'Client') {
      data.bio = undefined;
      data.avatarUrl = undefined;
    } else {
      data.heightCm = undefined;
      data.currentWeightKg = undefined;
    }

    this.authService.register(data).subscribe({
      next: () => {
        this.success = true;
        this.isLoading = false;
        setTimeout(() => this.router.navigate(['/auth/login']), 2000);
      },
      error: (err) => {
        this.isLoading = false;
        this.error = err.error?.message || 'Помилка реєстрації';
      }
    });
  }

  goToLogin() {
    this.router.navigate(['/auth/login']);
  }
}