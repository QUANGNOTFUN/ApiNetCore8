import { Routes } from '@angular/router';
import { authGuard } from './auth.guard';
import { SignInComponent } from './components/page/sign-in/sign-in.component';
import { DashboardComponent } from './components/page/dashboard/dashboard.component';
import { CategoryComponent } from './components/page/category/category.component';
import { ProductComponent } from './components/page/product/product.component';
import { SuppliersComponent } from './components/page/suppliers/suppliers.component';
import { ProductLowStockComponent } from './components/page/product-low-stock/product-low-stock.component';
import { OrdersComponent } from './components/page/orders/orders.component';
import { AdminGuard } from './auth/admin.guard';
import { ManagerUserComponent } from './components/page/manager-user/manager-user.component';

export const routes: Routes = [
    { path: '', component: DashboardComponent, canActivate: [authGuard] },
    { path: 'signin', component: SignInComponent },
    { path: 'category', component: CategoryComponent, canActivate: [authGuard] },
    { path: 'product', component: ProductComponent, canActivate: [authGuard] },
    { path: 'suppliers', component: SuppliersComponent, canActivate: [authGuard] },
    { path: 'lowstock', component: ProductLowStockComponent, canActivate: [authGuard] },
    { path: 'orders', component: OrdersComponent, canActivate: [authGuard] },
    { path: 'manager-user', component: ManagerUserComponent, canActivate: [AdminGuard] },
    { path: '**', pathMatch: 'full', component: DashboardComponent, canActivate: [authGuard] }
];
