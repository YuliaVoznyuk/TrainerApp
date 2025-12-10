// src/app/core/services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, tap } from 'rxjs';

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  birthdate: string; // "2000-01-01"
  role: 'Trainer' | 'Client';
  heightCm?: number;
  currentWeightKg?: number;

  // поля для тренера
  bio?: string;
  avatarUrl?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = 'https://localhost:7001/api/auth'; // ← твій бекенд
  private tokenKey = 'jwt_token';
  
  private currentUserSubject = new BehaviorSubject<any>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {
    this.loadToken();
  }

  register(data: RegisterRequest) {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  login(credentials: LoginRequest) {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        this.setToken(response.token);
        this.router.navigate(['/dashboard']);
      })
    );
  }

 private setToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
    this.decodeAndSetUser(token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private loadToken() {
    const token = this.getToken();
    if (token) {
      const payload = JSON.parse(atob(token.split('.')[1]));
      this.currentUserSubject.next(payload);
    }
  }
private decodeAndSetUser(token: string) {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      this.currentUserSubject.next(payload);
    } catch (e) {
      this.logout();
    }
  }
  logout() {
    localStorage.removeItem(this.tokenKey);
    this.currentUserSubject.next(null);
    this.router.navigate(['/auth/login']);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getUserRole(): string | null {
    const user = this.currentUserSubject.value;
    if (!user) return null;

    // У твоєму JWT роль лежить саме так (перевір через jwt.io)
    return user['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] 
           ?? user['role'] 
           ?? null;
  }

  getUserName(): string {
    const user = this.currentUserSubject.value;
    return user?.name || user?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || 'Користувач';
  }
}