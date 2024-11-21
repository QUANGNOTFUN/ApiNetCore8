import { Component } from '@angular/core';
import { ApiService } from '../../../service/api.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sign-in',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent {
  email: string = '';
  password: string = '';
  errorMessage: string | null = null;

  constructor(private apiService: ApiService, private router: Router) { }

  onSubmit(isValid: boolean): void {
    if (!isValid) {
      this.errorMessage = 'Vui lòng kiểm tra lại thông tin';
      return;
    }

    this.apiService.signIn(this.email, this.password).subscribe({
      next: (response) => {
        if (response && response.trim()) {
          localStorage.setItem('accessToken', response.trim());
          this.router.navigate(['/dashboard']);
        } else {
          this.errorMessage = 'Không có token trong phản hồi';
        }
      },
      error: (error) => {
        console.error('Đăng nhập thất bại:', error);
        this.errorMessage = 'Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin.';
      }
    });
  }


  navigateToSignup(): void {
    this.router.navigate(['/signup']);
  }

  navigateToHome(): void {
    this.router.navigate(['/']);
  }
}