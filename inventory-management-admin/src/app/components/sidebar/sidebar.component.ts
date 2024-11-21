import { Component, OnInit } from '@angular/core';
import { NgIf } from '@angular/common';
import { ApiService } from '../../service/api.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [NgIf],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent implements OnInit {
  fullName: string = '';
  email: string = '';
  isAdmin: boolean = false;

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.getUserInfo();
  }

  getUserInfo(): void {
    this.apiService.getUserInfo().subscribe({
      next: (response) => {
        this.fullName = `${response.firstName} ${response.lastName}`.trim();
        this.email = response.email;
        this.isAdmin = response.roles.includes('Administrator'); // Kiểm tra quyền admin
      },
      error: (error) => {
        console.error('Failed to fetch user info:', error);
      },
    });
  }
}
