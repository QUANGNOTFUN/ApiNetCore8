import { Component, OnInit } from '@angular/core';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { NavbarComponent } from '../../navbar/navbar.component';
import { SettingsPanelComponent } from '../../settings-panel/settings-panel.component';
import { MainPanelComponent } from '../../main-panel/main-panel.component';
import { ApiService } from '../../../service/api.service';
import { FooterComponent } from '../../footer/footer.component';
import { NgChartsModule } from 'ng2-charts';
import { ChartConfiguration, ChartType, ChartData } from 'chart.js';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    SidebarComponent,
    NavbarComponent,
    SettingsPanelComponent,
    FooterComponent,
    NgChartsModule,
    CommonModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  dashboardInDay: any = null;
  dashboardAll: any = null;

  // Biểu đồ tròn: Số lượng sản phẩm nhập
  pieChartDataProducts: ChartData<'pie', number[], string | string[]> = {
    labels: ['Today', 'Total'],
    datasets: [
      {
        data: [0, 0], // Số liệu ban đầu
        backgroundColor: ['#4CAF50', '#FFC107'], // Màu sắc
      },
    ],
  };
  pieChartOptionsProducts: ChartConfiguration['options'] = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
      },
    },
  };
  pieChartType: ChartType = 'pie';

  // Biểu đồ tròn: Tổng giá trị nhập
  pieChartDataPrice: ChartData<'pie', number[], string | string[]> = {
    labels: ['Today', 'Total'],
    datasets: [
      {
        data: [0, 0], // Số liệu ban đầu
        backgroundColor: ['#4CAF50', '#FFC107'], // Màu sắc
      },
    ],
  };
  pieChartOptionsPrice: ChartConfiguration['options'] = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
      },
    },
  };

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    // Gọi API lấy dữ liệu trong ngày
    this.apiService.getDashboardInDay().subscribe({
      next: (data) => {
        this.dashboardInDay = data;

        // Cập nhật dữ liệu biểu đồ số lượng sản phẩm nhập
        if (this.dashboardAll) {
          this.updatePieCharts();
        }
      },
      error: (err) => {
        console.error('Lỗi khi gọi API get-dashboard-in-day:', err);
      },
    });

    // Gọi API lấy dữ liệu tổng cộng
    this.apiService.getDashboardAll().subscribe({
      next: (data) => {
        this.dashboardAll = data;

        // Cập nhật dữ liệu biểu đồ khi cả hai API đã trả về
        if (this.dashboardInDay) {
          this.updatePieCharts();
        }
      },
      error: (err) => {
        console.error('Lỗi khi gọi API get-dashboard-all:', err);
      },
    });
  }

  // Cập nhật dữ liệu cho các biểu đồ
  updatePieCharts(): void {
    if (!this.dashboardInDay || !this.dashboardAll) {
      console.error('Thiếu dữ liệu từ API để cập nhật biểu đồ.');
      return;
    }

    // Cập nhật dữ liệu cho biểu đồ sản phẩm
    this.pieChartDataProducts.datasets[0].data = [
      this.dashboardInDay.quantityProductIn, // Số lượng sản phẩm nhập trong ngày
      this.dashboardAll.quantityProductIn,  // Tổng số lượng sản phẩm nhập
    ];
    this.pieChartDataProducts.labels = ['Today', 'Total']; // Đặt nhãn là Today và Total

    // Cập nhật dữ liệu cho biểu đồ tổng giá trị
    this.pieChartDataPrice.datasets[0].data = [
      this.dashboardInDay.totalPriceIn,  // Tổng giá trị nhập trong ngày
      this.dashboardAll.totalPriceIn,   // Tổng giá trị nhập
    ];
    this.pieChartDataPrice.labels = ['Today', 'Total'];
  }

}