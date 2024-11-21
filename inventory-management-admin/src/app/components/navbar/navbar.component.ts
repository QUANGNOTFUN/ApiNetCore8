import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  constructor(private router: Router) { }

  logout(): void {
    // Xóa accessToken trong localStorage
    localStorage.removeItem('accessToken');

    // Điều hướng người dùng đến trang đăng nhập (hoặc trang khác tùy ý)
    this.router.navigate(['/signin']);
  }

}
