import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../service/api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FooterComponent } from '../../footer/footer.component';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { NavbarComponent } from '../../navbar/navbar.component';
import { SettingsPanelComponent } from '../../settings-panel/settings-panel.component';

@Component({
  selector: 'app-manager-user',
  standalone: true,
  imports: [CommonModule, FormsModule, FooterComponent, SidebarComponent, NavbarComponent, SettingsPanelComponent],
  templateUrl: './manager-user.component.html',
  styleUrls: ['./manager-user.component.css'],
})
export class ManagerUserComponent implements OnInit {
  users: any[] = []; // To store the list of users
  newUser: any = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    role: '',
  };
  showCreateForm: boolean = false;

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.fetchUsers();
  }

  fetchUsers(): void {
    this.apiService.getAllUsers().subscribe({
      next: (response) => {
        this.users = response;
      },
      error: (error) => {
        console.error('Error fetching users:', error);
      },
    });
  }

  toggleCreateForm(): void {
    this.showCreateForm = !this.showCreateForm;
  }

  addUser(): void {
    this.apiService.createUser(this.newUser).subscribe({
      next: () => {
        alert('User created successfully');
        this.fetchUsers(); // Refresh user list
        this.resetForm();
        this.showCreateForm = false;
      },
      error: (error) => {
        console.error('Error creating user:', error);
      },
    });
  }

  cancelForm(): void {
    this.resetForm(); // Reset form data
    this.showCreateForm = false; // Hide the form
  }

  resetForm(): void {
    this.newUser = {
      firstName: '',
      lastName: '',
      email: '',
      password: '',
      confirmPassword: '',
      role: '',
    };
  }
}