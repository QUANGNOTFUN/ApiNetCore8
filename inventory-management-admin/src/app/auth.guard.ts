import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const token = localStorage.getItem('accessToken');
  const router = inject(Router); // Inject Router để điều hướng

  if (token) {
    // Người dùng đã đăng nhập
    return true;
  } else {
    // Chưa đăng nhập -> điều hướng về trang signin
    router.navigate(['/signin']);
    return false;
  }
};