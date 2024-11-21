import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  // Base URL cho các API
  private accountApiUrl = 'http://112.197.59.27:7079/api/Account'; // URL cho Account
  private categoriesApiUrl = 'http://112.197.59.27:7079/api/Categories'; // URL cho Categories
  private productsApiUrl = 'http://112.197.59.27:7079/api/Products'; // Base URL cho API sản phẩm
  private suppliersApiUrl = 'http://112.197.59.27:7079/api/Suppliers';
  private orderApiUrl = 'http://112.197.59.27:7079/api/Orders';
  private orderDetailsApiUrl = 'http://112.197.59.27:7079/api/OrderDetails';
  private dashboardApiUrl = 'http://112.197.59.27:7079/api/Dashboards';

  constructor(private http: HttpClient) { }

  // Đăng nhập
  signIn(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.accountApiUrl}/signin`, { email, password }, { responseType: 'text' as 'json' }).pipe(
      catchError((error) => {
        console.error('Lỗi khi gọi API đăng nhập:', error);
        return throwError(error);
      })
    );
  }

  createUser(user: any): Observable<any> {
    return this.http.post(`${this.accountApiUrl}/signup`, user, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
        'Content-Type': 'application/json',
      },
    });
  }

  getAllUsers(): Observable<any> {
    return this.http.get(`${this.accountApiUrl}/get-all-users`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
      },
    });
  }

  getUserInfo(): Observable<any> {
    return this.http.get(`${this.accountApiUrl}/get-info`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
      },
    });
  }

  // Lấy danh sách tất cả category
  getAllCategories(page: number = 1, pageSize: number = 20): Observable<any> {
    return this.http.get<any>(`${this.categoriesApiUrl}/all-category?page=${page}&pageSize=${pageSize}`).pipe(
      catchError((error) => {
        console.error('Lỗi khi gọi API danh mục:', error);
        return throwError(error);
      })
    );
  }

  // Tìm kiếm category
  findCategory(name: string, page: number = 1, pageSize: number = 20): Observable<any> {
    return this.http.get<any>(`${this.categoriesApiUrl}/find-category?name=${name}&page=${page}&pageSize=${pageSize}`).pipe(
      catchError((error) => {
        console.error('Lỗi khi tìm kiếm danh mục:', error);
        return throwError(error);
      })
    );
  }

  // Thêm category
  addCategory(categoryData: any): Observable<any> {
    return this.http.post<any>(`${this.categoriesApiUrl}/add-category`, categoryData).pipe(
      catchError((error) => {
        console.error('Lỗi khi thêm danh mục:', error);
        return throwError(error);
      })
    );
  }

  updateCategory(categoryId: number, categoryData: any): Observable<any> {
    const url = `${this.categoriesApiUrl}/update-category?id=${categoryId}`;
    return this.http.put(url, categoryData, { responseType: 'text' }).pipe(
      catchError((error) => {
        console.error('Lỗi khi gọi API cập nhật danh mục:', error);
        return throwError(error);
      })
    );
  }

  deleteCategory(categoryId: number): Observable<any> {
    return this.http.delete<any>(`${this.categoriesApiUrl}/delete-category?id=${categoryId}`).pipe(
      catchError((error) => {
        console.error('Lỗi khi xóa danh mục:', error);
        return throwError(error);
      })
    );
  }

  getAllProducts(page: number = 1, pageSize: number = 20): Observable<any> {
    return this.http
      .get<any>(`${this.productsApiUrl}/all-product?page=${page}&pageSize=${pageSize}`)
      .pipe(
        catchError((error) => {
          console.error('Lỗi khi gọi API sản phẩm:', error);
          return throwError(error);
        })
      );
  }

  findProduct(name: string, page: number = 1, pageSize: number = 20): Observable<any> {
    return this.http
      .get<any>(`${this.productsApiUrl}/find-product?name=${name}&page=${page}&pageSize=${pageSize}`)
      .pipe(
        catchError((error) => {
          console.error('Lỗi khi tìm kiếm sản phẩm:', error);
          return throwError(error);
        })
      );
  }

  // Thêm sản phẩm
  addProduct(product: any): Observable<any> {
    return this.http.post<any>(`${this.productsApiUrl}/add-product`, product).pipe(
      catchError((error) => {
        console.error('Lỗi khi thêm sản phẩm:', error);
        return throwError(error);
      })
    );
  }

  // Cập nhật sản phẩm
  updateProduct(productId: number, product: any): Observable<any> {
    return this.http.put(`${this.productsApiUrl}/update-product?id=${productId}`, product, { responseType: 'text' }).pipe(
      catchError((error) => {
        console.error('Lỗi khi cập nhật sản phẩm:', error);
        return throwError(error);
      })
    );
  }

  getAllSuppliers(page: number = 1, pageSize: number = 20): Observable<any> {
    const url = `${this.suppliersApiUrl}/all-Supplier?page=${page}&pageSize=${pageSize}`;
    return this.http.get<any>(url).pipe(
      catchError((error) => {
        console.error('Lỗi khi gọi API nhà cung cấp:', error);
        return throwError(error);
      })
    );
  }

  findSuppliers(name: string, page: number = 1, pageSize: number = 20): Observable<any> {
    const url = `${this.suppliersApiUrl}/find-Supplier?name=${encodeURIComponent(name)}&page=${page}&pageSize=${pageSize}`;
    return this.http.get<any>(url).pipe(
      catchError((error) => {
        console.error('Lỗi khi gọi API tìm kiếm nhà cung cấp:', error);
        return throwError(error);
      })
    );
  }

  addSupplier(supplierData: any): Observable<any> {
    const url = `${this.suppliersApiUrl}/add-Supplier`;
    return this.http.post<any>(url, supplierData).pipe(
      catchError((error) => {
        console.error('Lỗi khi gọi API thêm nhà cung cấp:', error);
        return throwError(error);
      })
    );
  }

  updateSupplier(supplierId: number, supplierData: any): Observable<any> {
    const url = `${this.suppliersApiUrl}/update-Supplier?id=${supplierId}`;
    return this.http.put<any>(url, supplierData).pipe(
      catchError((error) => {
        console.error('Lỗi khi gọi API cập nhật nhà cung cấp:', error);
        return throwError(error);
      })
    );
  }

  deleteSupplier(supplierId: number): Observable<any> {
    const url = `${this.suppliersApiUrl}/delete-Supplier?id=${supplierId}`;
    return this.http.delete<any>(url).pipe(
      catchError((error) => {
        console.error('Lỗi khi gọi API xóa nhà cung cấp:', error);
        return throwError(error);
      })
    );
  }

  getLowStockProducts(page: number, pageSize: number) {
    const url = `${this.productsApiUrl}/low-stock?page=${page}&pageSize=${pageSize}`;
    return this.http.get(url);
  }

  addOrder(order: any): Observable<any> {
    const url = `${this.orderApiUrl}/add-order`;
    return this.http.post(url, order, {
      params: { button: 'Đặt hàng' },
      headers: { 'Content-Type': 'application/json' }
    });
  }

  getAllOrders(page: number = 1, pageSize: number = 20): Observable<any> {
    const url = `${this.orderApiUrl}/all-Orders?page=${page}&pageSize=${pageSize}`;
    return this.http.get<any>(url).pipe(
      catchError((error) => {
        console.error('Lỗi khi gọi API đơn hàng:', error);
        return throwError(error);
      })
    );
  }

  findOrdersByDate(date: string): Observable<any> {
    const url = `${this.orderApiUrl}/find-OrdersByDate?date=${date}`;
    return this.http.get<any[]>(url).pipe( // API trả về một mảng
      catchError((error) => {
        console.error('Lỗi khi gọi API lọc đơn hàng theo ngày:', error);
        return throwError(error);
      })
    );
  }

  updateOrderStatus(orderId: number, action: 'confirm' | 'cancel') {
    const url = `${this.orderApiUrl}/update-order-status?orderId=${orderId}&action=${action}`;
    return this.http.put(url, {}, { responseType: 'text' });
  }

  // API lấy dữ liệu trong ngày
  getDashboardInDay(): Observable<any> {
    return this.http.get(`${this.dashboardApiUrl}/get-dashboard-in-day`).pipe(
      catchError((error) => {
        console.error('Lỗi khi gọi API get-dashboard-in-day:', error);
        return throwError(error);
      })
    );
  }

  // API lấy dữ liệu tổng cộng
  getDashboardAll(): Observable<any> {
    return this.http.get(`${this.dashboardApiUrl}/get-dashboard-all`).pipe(
      catchError((error) => {
        console.error('Lỗi khi gọi API get-dashboard-all:', error);
        return throwError(error);
      })
    );
  }
}