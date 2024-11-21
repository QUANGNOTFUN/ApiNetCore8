import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../service/api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FooterComponent } from '../../footer/footer.component';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { NavbarComponent } from '../../navbar/navbar.component';
import { SettingsPanelComponent } from '../../settings-panel/settings-panel.component';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule, FormsModule, FooterComponent, SidebarComponent, NavbarComponent, SettingsPanelComponent],
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent implements OnInit {
  products: any[] = [];
  categories: any[] = [];
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 20;
  pageSizeOptions: number[] = [20, 50, 100];
  searchTerm: string = '';

  showAddProductForm: boolean = false;
  newProductName: string = '';
  newProductDescription: string = '';
  newCostPrice: number | null = null;
  newSellPrice: number | null = null;
  newStockQuantity: number | null = null;
  newReorderLevel: number | null = null;
  newCategoryID: number | null = null;

  editingProductId: number | null = null;

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.loadProducts();
    this.loadCategories();
  }

  // Load danh sách danh mục từ API
  loadCategories(): void {
    this.apiService.getAllCategories(1, 500).subscribe(
      (response) => {
        this.categories = response.items; // Lưu danh mục vào biến
      },
      (error) => {
        console.error('Lỗi khi tải danh mục:', error);
      }
    );
  }

  // Load tất cả sản phẩm
  loadProducts(): void {
    this.apiService.getAllProducts(this.currentPage, this.pageSize).subscribe(
      (response) => {
        this.products = response.items;
        this.totalCount = response.totalCount;
      },
      (error) => {
        console.error('Lỗi khi tải sản phẩm:', error);
      }
    );
  }

  // Xử lý tìm kiếm sản phẩm
  searchProducts(): void {
    if (this.searchTerm.trim()) {
      this.apiService.findProduct(this.searchTerm, this.currentPage, this.pageSize).subscribe(
        (response) => {
          this.products = response.items;
          this.totalCount = response.totalCount;
        },
        (error) => {
          console.error('Lỗi khi tìm kiếm sản phẩm:', error);
        }
      );
    } else {
      this.loadProducts(); // Nếu không nhập từ khóa, tải tất cả sản phẩm
    }
  }

  // Phân trang
  onPageChange(page: number): void {
    const totalPages = Math.ceil(this.totalCount / this.pageSize);
    if (page > 0 && page <= totalPages) {
      this.currentPage = page;
      this.searchProducts(); // Cập nhật theo tìm kiếm hoặc tất cả sản phẩm
    }
  }

  onPageSizeChange(newPageSize: number): void {
    this.pageSize = newPageSize;
    this.currentPage = 1;
    this.searchProducts();
  }

  getTotalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  // Thêm và cập nhật sản phẩm (chưa triển khai đầy đủ trong đoạn này)
  toggleAddProductForm(): void {
    this.showAddProductForm = !this.showAddProductForm;
  }

  addOrUpdateProduct(): void {
    const product = {
      productID: this.editingProductId || 0,
      productName: this.newProductName,
      description: this.newProductDescription,
      costPrice: this.newCostPrice,
      sellPrice: this.newSellPrice,
      stockQuantity: this.newStockQuantity,
      reorderLevel: this.newReorderLevel,
      categoryID: this.newCategoryID,
    };

    if (this.editingProductId) {
      // Cập nhật sản phẩm
      this.apiService.updateProduct(this.editingProductId, product).subscribe(
        () => {
          this.resetForm();
          this.loadProducts();
        },
        (error) => {
          console.error('Lỗi khi cập nhật sản phẩm:', error);
        }
      );
    } else {
      // Thêm sản phẩm
      this.apiService.addProduct(product).subscribe(
        () => {
          this.resetForm();
          this.loadProducts();
        },
        (error) => {
          console.error('Lỗi khi thêm sản phẩm:', error);
        }
      );
    }
  }

  editProduct(product: any): void {
    this.editingProductId = product.productID;
    this.newProductName = product.productName;
    this.newProductDescription = product.description;
    this.newCostPrice = product.costPrice;
    this.newSellPrice = product.sellPrice;
    this.newStockQuantity = product.stockQuantity;
    this.newReorderLevel = product.reorderLevel;
    this.newCategoryID = product.categoryID;
    this.showAddProductForm = true;
  }

  resetForm(): void {
    this.newProductName = '';
    this.newProductDescription = '';
    this.newCostPrice = null;
    this.newSellPrice = null;
    this.newStockQuantity = 0;
    this.newReorderLevel = 0;
    this.newCategoryID = 0;
    this.editingProductId = null;
    this.showAddProductForm = false;
  }
}