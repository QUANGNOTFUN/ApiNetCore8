import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../service/api.service';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FooterComponent } from '../../footer/footer.component';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { NavbarComponent } from '../../navbar/navbar.component';
import { SettingsPanelComponent } from '../../settings-panel/settings-panel.component';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, FormsModule, FooterComponent, SidebarComponent, NavbarComponent, SettingsPanelComponent],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent implements OnInit {
  orders: any[] = [];
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 20;
  pageSizeOptions: number[] = [20, 50, 100];

  selectedDate: string = '';

  suppliers: any = {};

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.loadSuppliers();
    this.loadOrders();
  }

  loadSuppliers(): void {
    this.apiService.getAllSuppliers(1, 500).subscribe({
      next: (response: any) => {
        this.suppliers = response.items.reduce((acc: any, supplier: any) => {
          acc[supplier.supplierId] = supplier.supplierName; // Map supplierId với supplierName
          return acc;
        }, {});
      },
      error: (error) => {
        console.error('Lỗi khi tải dữ liệu nhà cung cấp:', error);
      },
    });
  }

  getSupplierName(supplierId: string): string {
    return this.suppliers[supplierId] || 'Unknown Supplier';
  }

  loadOrders(): void {
    this.apiService.getAllOrders(this.currentPage, this.pageSize).subscribe({
      next: (response: any) => {
        this.orders = response.items.sort((a: any, b: any) => {
          const dateA = new Date(a.orderDate).getTime();
          const dateB = new Date(b.orderDate).getTime();
          return dateB - dateA;
        });
        this.totalCount = response.totalCount || 0;
      },
      error: (error) => {
        console.error('Lỗi khi tải dữ liệu đơn hàng:', error);
      },
    });
  }

  filterOrdersByDate(): void {
    if (!this.selectedDate) {
      console.error('Please select a date to filter orders.');
      return;
    }

    this.apiService.findOrdersByDate(this.selectedDate).subscribe({
      next: (response: any) => {
        console.log('Dữ liệu trả về từ API:', response);
        // Gán trực tiếp mảng từ API vào orders
        this.orders = response || [];
      },
      error: (error) => {
        console.error('Lỗi khi lọc đơn hàng theo ngày:', error);
        this.orders = []; // Nếu lỗi, đặt danh sách đơn hàng trống
      },
    });
  }

  openActionPopup(order: any, action: 'confirm' | 'cancel'): void {
    const actionText = action === 'confirm' ? 'Confirm' : 'Cancel';
    Swal.fire({
      title: `Are you sure you want to ${actionText} this order?`,
      icon: action === 'confirm' ? 'success' : 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
    }).then((result) => {
      if (result.isConfirmed) {
        this.updateOrderStatus(order.orderId, action);
      }
    });
  }

  updateOrderStatus(orderId: number, action: 'confirm' | 'cancel'): void {
    this.apiService.updateOrderStatus(orderId, action).subscribe({
      next: (response: string) => {
        Swal.fire('Success', `Order has been ${action}ed successfully. "${response}"`, 'success');
        this.loadOrders(); // Tải lại dữ liệu
      },
      error: (error) => {
        console.error(`Error updating order status:`, error);
        Swal.fire('Error', 'Something went wrong. Please try again later.', 'error');
      },
    });
  }


  onPageChange(page: number): void {
    const totalPages = Math.ceil(this.totalCount / this.pageSize);
    if (page > 0 && page <= totalPages) {
      this.currentPage = page;
      this.loadOrders();
    }
  }

  onPageSizeChange(newPageSize: number): void {
    this.pageSize = newPageSize;
    this.currentPage = 1;
    this.loadOrders();
  }

  getTotalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }
}