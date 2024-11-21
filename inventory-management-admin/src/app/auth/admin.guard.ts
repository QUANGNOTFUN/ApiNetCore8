import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ApiService } from '../service/api.service';

@Injectable({
  providedIn: 'root',
})
export class AdminGuard implements CanActivate {
  constructor(private apiService: ApiService, private router: Router) { }

  canActivate(): Promise<boolean> {
    return new Promise((resolve) => {
      this.apiService.getUserInfo().subscribe({
        next: (response) => {
          if (response.roles.includes('Administrator')) {
            resolve(true);
          } else {
            this.router.navigate(['/']); // Redirect nếu không phải Admin
            resolve(false);
          }
        },
        error: () => {
          this.router.navigate(['/signin']); // Redirect về login nếu lỗi
          resolve(false);
        },
      });
    });
  }
}
