import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../service/api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FooterComponent } from '../../footer/footer.component';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { NavbarComponent } from '../../navbar/navbar.component';
import { SettingsPanelComponent } from '../../settings-panel/settings-panel.component';

@Component({
  selector: 'app-category',
  standalone: true,
  imports: [CommonModule, FormsModule, FooterComponent, SidebarComponent, NavbarComponent, SettingsPanelComponent],
  templateUrl: './category.component.html',
  styleUrl: './category.component.css'
})
export class CategoryComponent implements OnInit {
  categories: any[] = [];
  suppliers: any[] = [];
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 20;
  searchTerm: string = '';

  pageSizeOptions: number[] = [20, 50, 100];

  newCategoryName: string = '';
  newCategoryDescription: string = '';
  selectedSuppliers: any[] = [];
  showAddCategoryForm: boolean = false;

  editingCategoryId: number | null = null;

  supplierSearchTerm: string = '';

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.loadCategories();
    this.loadSuppliers();
  }

  loadCategories(): void {
    this.apiService.getAllCategories(this.currentPage, this.pageSize).subscribe(
      (response) => {
        this.categories = response.items;
        this.totalCount = response.totalCount;
      },
      (error) => {
        console.error('Lỗi khi tải danh mục:', error);
      }
    );
  }

  loadSuppliers(): void {
    this.apiService.getAllSuppliers().subscribe(
      (response) => {
        this.suppliers = response.items;
      },
      (error) => {
        console.error('Lỗi khi tải nhà cung cấp:', error);
      }
    );
  }

  searchSuppliers(): void {
    if (this.supplierSearchTerm.trim()) {
      this.apiService.findSuppliers(this.supplierSearchTerm).subscribe(
        (response) => {
          this.suppliers = response.items; // Cập nhật danh sách nhà cung cấp
        },
        (error) => {
          console.error('Lỗi khi tìm kiếm nhà cung cấp:', error);
        }
      );
    } else {
      this.loadSuppliers(); // Nếu không có từ khóa, tải tất cả nhà cung cấp
    }
  }

  searchCategories(): void {
    if (this.searchTerm.trim()) {
      this.apiService.findCategory(this.searchTerm, this.currentPage, this.pageSize).subscribe(
        (response) => {
          this.categories = response.items;
          this.totalCount = response.totalCount;
        },
        (error) => {
          console.error('Lỗi khi tìm kiếm danh mục:', error);
        }
      );
    } else {
      this.loadCategories();
    }
  }

  onPageChange(page: number): void {
    const totalPages = Math.ceil(this.totalCount / this.pageSize);
    if (page > 0 && page <= totalPages) {
      this.currentPage = page;
      this.searchCategories();
    }
  }

  getTotalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }


  // Mở hoặc đóng form thêm danh mục
  toggleAddCategoryForm(): void {
    this.showAddCategoryForm = !this.showAddCategoryForm;
  }

  addCategory(): void {
    if (this.newCategoryName.trim() && this.newCategoryDescription.trim() && this.selectedSuppliers.length > 0) {
      const categoryData = {
        categoryName: this.newCategoryName,
        description: this.newCategoryDescription,
        supplier: this.selectedSuppliers.map((supplierId) => ({
          supplierId: supplierId,
          supplierName: this.suppliers.find((s) => s.supplierId === supplierId)?.supplierName || '',
        })),
      };

      this.apiService.addCategory(categoryData).subscribe(
        () => {
          console.log('Danh mục đã được thêm thành công');
          this.resetForm();
          this.loadCategories();
        },
        (error) => {
          console.error('Lỗi khi thêm danh mục:', error);
        }
      );
    } else {
      console.error('Vui lòng điền đầy đủ thông tin và chọn nhà cung cấp');
    }
  }

  updateCategory(): void {
    if (!this.editingCategoryId) {
      console.error('Không có ID danh mục để cập nhật');
      return;
    }

    const updatedCategoryData = {
      categoryName: this.newCategoryName,
      description: this.newCategoryDescription,
      supplierIdNew: this.selectedSuppliers,
    };

    this.apiService.updateCategory(this.editingCategoryId, updatedCategoryData)
      .subscribe({
        next: (response) => {
          console.log('Cập nhật danh mục thành công:', response);
          this.resetForm();
          this.loadCategories(); // Tải lại danh sách danh mục
        },
        error: (error) => {
          console.error('Lỗi khi cập nhật danh mục:', error);
        },
      });
  }


  resetForm(): void {
    this.newCategoryName = '';
    this.newCategoryDescription = '';
    this.selectedSuppliers = [];
    this.showAddCategoryForm = false;
    this.editingCategoryId = null;
  }

  editCategory(category: any): void {
    this.editingCategoryId = category.categoryId;
    this.newCategoryName = category.categoryName;
    this.newCategoryDescription = category.description;

    // Đặt các nhà cung cấp đã chọn
    this.selectedSuppliers = category.supplier.map((s: any) => s.supplierId);
    this.showAddCategoryForm = true;
  }

  deleteCategory(categoryId: number): void {
    const confirmDelete = window.confirm('Bạn có chắc chắn muốn xóa danh mục này không?');
    if (confirmDelete) {
      this.apiService.deleteCategory(categoryId).subscribe(
        () => {
          console.log('Danh mục đã được xóa thành công');
          this.loadCategories(); // Tải lại danh sách danh mục sau khi xóa
        },
        (error) => {
          console.error('Lỗi khi xóa danh mục:', error);
        }
      );
    }
  }

  onPageSizeChange(newPageSize: number): void {
    this.pageSize = newPageSize;
    this.currentPage = 1; // Reset về trang đầu tiên
    this.loadCategories();
  }
}