import { StatisticsComponent } from './statistics/statistics.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { ProductListComponent } from './product-list/product-list.component';
import { SaleOrderListComponent } from './sale-order-list/sale-order-list.component';
import { SaleOrderEditComponent } from './sale-order-edit/sale-order-edit.component';

const routes: Routes = [
  { path: '', redirectTo: 'customerlist', pathMatch: 'full' },
  { path: 'customerlist', component: CustomerListComponent, data: { title: '客户管理', reuse: true } },
  { path: 'productlist', component: ProductListComponent, data: { title: '产品管理', reuse: true } },
  { path: 'saleorderlist', component: SaleOrderListComponent, data: { title: '销售单管理', reuse: true } },
  { path: 'saleorder-add', component: SaleOrderEditComponent, data: { title: '添加销售单', reuse: true } },
  { path: 'saleorder-edit/:id', component: SaleOrderEditComponent, data: { title: '编辑销售单', reuse: true } },
  { path: 'statistics', component: StatisticsComponent, data: { title: '统计分析', reuse: true } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SaleRoutingModule { }
