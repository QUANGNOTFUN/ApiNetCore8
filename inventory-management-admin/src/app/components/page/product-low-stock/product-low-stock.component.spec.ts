import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductLowStockComponent } from './product-low-stock.component';

describe('ProductLowStockComponent', () => {
  let component: ProductLowStockComponent;
  let fixture: ComponentFixture<ProductLowStockComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductLowStockComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductLowStockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
