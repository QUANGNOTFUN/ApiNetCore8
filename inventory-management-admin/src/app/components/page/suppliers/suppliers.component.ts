import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../service/api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FooterComponent } from '../../footer/footer.component';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { NavbarComponent } from '../../navbar/navbar.component';
import { SettingsPanelComponent } from '../../settings-panel/settings-panel.component';

@Component({
  selector: 'app-suppliers',
  standalone: true,
  imports: [CommonModule, FormsModule, FooterComponent, SidebarComponent, NavbarComponent, SettingsPanelComponent],
  templateUrl: './suppliers.component.html',
  styleUrl: './suppliers.component.css'
})
export class SuppliersComponent implements OnInit {
  suppliers: any[] = [];
  currentPage: number = 1;
  pageSize: number = 20;
  totalCount: number = 0;
  pageSizeOptions: number[] = [10, 20, 50];

  searchTerm: string = '';
  isSearching: boolean = false;

  newSupplierName: string = '';
  newContactInfo: string = '';
  showAddSupplierForm: boolean = false;

  editingSupplierId: number | null = null;

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.loadSuppliers();
  }

  loadSuppliers(): void {
    this.apiService.getAllSuppliers(this.currentPage, this.pageSize).subscribe({
      next: (response) => {
        this.suppliers = response.items;
        this.totalCount = response.totalCount;
      },
      error: (error) => {
        console.error('Lỗi khi tải danh sách nhà cung cấp:', error);
      }
    });
  }

  searchSuppliers(): void {
    if (!this.searchTerm.trim()) {
      this.loadSuppliers();
      return;
    }

    this.isSearching = true;
    this.apiService.findSuppliers(this.searchTerm, this.currentPage, this.pageSize).subscribe({
      next: (response) => {
        this.suppliers = response.items;
        this.totalCount = response.totalCount;
        this.isSearching = false;
      },
      error: (error) => {
        console.error('Lỗi khi tìm kiếm nhà cung cấp:', error);
        this.isSearching = false;
      }
    });
  }

  addSupplier(): void {
    if (!this.newSupplierName.trim() || !this.newContactInfo.trim()) {
      console.error('Tên nhà cung cấp và thông tin liên hệ là bắt buộc');
      return;
    }

    const supplierData = {
      supplierName: this.newSupplierName,
      contactInfo: this.newContactInfo,
    };

    this.apiService.addSupplier(supplierData).subscribe({
      next: (response) => {
        console.log('Thêm nhà cung cấp thành công:', response);
        this.resetForm();
        this.loadSuppliers();
      },
      error: (error) => {
        console.error('Lỗi khi thêm nhà cung cấp:', error);
      },
    });
  }

  updateSupplier(): void {
    if (!this.editingSupplierId || !this.newSupplierName.trim() || !this.newContactInfo.trim()) {
      console.error('Vui lòng nhập đầy đủ thông tin để cập nhật');
      return;
    }

    const updatedSupplierData = {
      supplierId: this.editingSupplierId,
      supplierName: this.newSupplierName,
      contactInfo: this.newContactInfo,
    };

    this.apiService.updateSupplier(this.editingSupplierId, updatedSupplierData).subscribe({
      next: (response) => {
        this.resetForm();
        this.loadSuppliers();
      },
      error: (error) => {
        console.error('Lỗi khi cập nhật nhà cung cấp:', error);
      },
    });
  }

  deleteSupplier(supplierId: number): void {
    if (!confirm('Bạn có chắc chắn muốn xóa nhà cung cấp này?')) {
      return;
    }

    this.apiService.deleteSupplier(supplierId).subscribe({
      next: () => {
        console.log(`Nhà cung cấp với ID ${supplierId} đã được xóa thành công.`);
        this.loadSuppliers(); // Tải lại danh sách sau khi xóa
      },
      error: (error) => {
        console.error('Lỗi khi xóa nhà cung cấp:', error);
      },
    });
  }


  toggleAddSupplierForm(): void {
    this.showAddSupplierForm = !this.showAddSupplierForm;

    if (this.showAddSupplierForm) {
      // Khi mở form thêm nhà cung cấp, reset trạng thái cập nhật
      this.editingSupplierId = null;
      this.newSupplierName = '';
      this.newContactInfo = '';
    } else {
      // Khi đóng form, reset hoàn toàn
      this.resetForm();
    }
  }


  editSupplier(supplier: any): void {
    this.editingSupplierId = supplier.supplierId;
    this.newSupplierName = supplier.supplierName;
    this.newContactInfo = supplier.contactInfo;
    this.showAddSupplierForm = true; // Hiển thị form chỉnh sửa
  }

  onPageChange(page: number): void {
    if (page < 1 || page > this.getTotalPages()) {
      return;
    }
    this.currentPage = page;
    this.isSearching ? this.searchSuppliers() : this.loadSuppliers();
  }

  onPageSizeChange(newPageSize: number): void {
    this.pageSize = newPageSize;
    this.currentPage = 1;
    this.isSearching ? this.searchSuppliers() : this.loadSuppliers();
  }

  getTotalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  resetForm(): void {
    this.newSupplierName = '';
    this.newContactInfo = '';
    this.showAddSupplierForm = false;
  }

}