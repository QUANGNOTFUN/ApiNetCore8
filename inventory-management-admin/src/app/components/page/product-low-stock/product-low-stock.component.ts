import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../service/api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FooterComponent } from '../../footer/footer.component';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { NavbarComponent } from '../../navbar/navbar.component';
import { SettingsPanelComponent } from '../../settings-panel/settings-panel.component';

@Component({
  selector: 'app-product-low-stock',
  standalone: true,
  imports: [CommonModule, FormsModule, FooterComponent, SidebarComponent, NavbarComponent, SettingsPanelComponent],
  templateUrl: './product-low-stock.component.html',
  styleUrl: './product-low-stock.component.css'
})
export class ProductLowStockComponent implements OnInit {
  products: any[] = [];
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 20;
  pageSizeOptions: number[] = [20, 50, 100];

  selectedProduct: any = null;
  isPopupOpen: boolean = false;
  orderQuantity: number = 0;
  selectedSupplierId: number | null = null;

  isMultiOrderPopupOpen: boolean = false;
  selectedProducts: any[] = [];
  multiOrderDetails: { productId: number; quantity: number; supplierId: number }[] = [];

  suppliers: any[] = [];

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.loadLowStockProducts();
    this.loadSuppliers();
  }

  // Load danh sách sản phẩm tồn kho thấp từ API
  loadLowStockProducts(): void {
    this.apiService.getLowStockProducts(this.currentPage, this.pageSize).subscribe(
      (response: any) => {
        this.products = response.items;
        this.totalCount = response.totalCount || 0;
      },
      (error) => {
        console.error('Lỗi khi tải sản phẩm tồn kho thấp:', error);
      }
    );
  }

  loadSuppliers(): void {
    this.apiService.getAllSuppliers(1, 500).subscribe(
      (response: any) => {
        this.suppliers = response.items;
      },
      (error) => {
        console.error('Lỗi khi tải danh sách nhà cung cấp:', error);
      }
    );
  }

  getSupplierNameById(supplierId: number): string {
    const supplier = this.suppliers.find((sup) => sup.supplierId === supplierId);
    return supplier ? supplier.supplierName : `Supplier ${supplierId}`;
  }

  openOrderPopup(product: any): void {
    this.selectedProduct = product;
    this.orderQuantity = 0;
    this.selectedSupplierId = product.supplierIds.length > 0 ? Number(product.supplierIds[0]) : null;
    this.isPopupOpen = true;
  }

  closePopup(): void {
    this.isPopupOpen = false;
    this.selectedProduct = null;
    this.orderQuantity = 0;
    this.selectedSupplierId = null;
  }

  confirmOrder(): void {
    if (!this.orderQuantity || this.orderQuantity <= 0) {
      alert('Số lượng phải lớn hơn 0');
      return;
    }

    const supplierId = Number(this.selectedSupplierId);
    if (!supplierId) {
      alert('Vui lòng chọn nhà cung cấp hợp lệ');
      return;
    }

    const orderData = {
      addOrderDetails: [
        {
          productId: this.selectedProduct.productID,
          supplierId: supplierId,
          quantity: this.orderQuantity,
        },
      ],
    };

    this.apiService.addOrder(orderData).subscribe({
      next: () => {
        alert('Đặt hàng thành công!');
        this.closePopup();
      },
      error: (error) => {
        alert('Có lỗi xảy ra khi đặt hàng.');
        console.error(error);
      },
    });
  }

  onPageChange(page: number): void {
    const totalPages = this.getTotalPages();
    if (totalPages === 0) {
      return; // Nếu không có trang nào, không cho phép chuyển trang
    }
    if (page > 0 && page <= totalPages) {
      this.currentPage = page;
      this.loadLowStockProducts();
    }
  }

  onPageSizeChange(newPageSize: number): void {
    this.pageSize = newPageSize;
    this.currentPage = 1;
    this.loadLowStockProducts();
  }

  getTotalPages(): number {
    const totalPages = Math.ceil(this.totalCount / this.pageSize);
    return totalPages > 0 ? totalPages : 1; // Đảm bảo luôn trả về ít nhất 1
  }

  // getTotalPages(): number {
  //   return Math.ceil(this.totalCount / this.pageSize);
  // }

  openMultiOrderPopup(): void {
    this.isMultiOrderPopupOpen = true;
    this.selectedProducts = this.products.map((product) => ({
      productId: product.productID,
      quantity: 0,
      supplierId: product.supplierIds.length > 0 ? product.supplierIds[0] : null,
      isSelected: false, // Thêm isSelected để theo dõi checkbox
    }));
  }

  closeMultiOrderPopup(): void {
    this.isMultiOrderPopupOpen = false;
    this.selectedProducts = [];
  }

  confirmMultiOrder(): void {
    const validOrders = this.selectedProducts
      .filter((item) => item.isSelected && item.quantity > 0 && item.supplierId);

    if (validOrders.length === 0) {
      alert('Vui lòng chọn sản phẩm và nhập số lượng hợp lệ.');
      return;
    }

    const orderData = {
      addOrderDetails: validOrders.map((item) => ({
        productId: item.productId,
        supplierId: item.supplierId,
        quantity: item.quantity,
      }))
    };

    this.apiService.addOrder(orderData).subscribe({
      next: () => {
        alert('Đặt hàng thành công!');
        this.closeMultiOrderPopup();
      },
      error: (error) => {
        alert('Có lỗi xảy ra khi đặt hàng.');
        console.error(error);
      },
    });
  }
  getProductNameById(productId: number): string {
    const product = this.products.find((prod) => prod.productID === productId);
    return product ? product.productName : `Product ${productId}`;
  }

  getSuppliersByProductId(productId: number): number[] {
    const product = this.products.find((prod) => prod.productID === productId);
    return product ? product.supplierIds : [];
  }

}