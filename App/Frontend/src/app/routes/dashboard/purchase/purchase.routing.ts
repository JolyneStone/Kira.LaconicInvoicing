import { StatisticsComponent } from './statistics/statistics.component';
import { PurchaseOrderEditComponent } from './purchase-order-edit/purchase-order-edit.component';
import { PurchaseOrderListComponent } from './purchase-order-list/purchase-order-list.component';
import { MaterialListComponent } from './material-list/material-list.component';
import { VendorListComponent } from './vendor-list/vendor-list.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'vendorlist', pathMatch: 'full' },
  { path: 'vendorlist', component: VendorListComponent, data: { title: '供应商管理', reuse: true } },
  { path: 'materiallist', component: MaterialListComponent, data: { title: '原料管理', reuse: true } },
  { path: 'purchaseorderlist', component: PurchaseOrderListComponent, data: { title: '采购单管理', reuse: true } },
  { path: 'purchaseorder-add', component: PurchaseOrderEditComponent, data: { title: '添加采购单', reuse: true } },
  { path: 'purchaseorder-edit/:id', component: PurchaseOrderEditComponent, data: { title: '编辑采购单', reuse: true } },
  { path: 'statistics', component: StatisticsComponent, data: { title: '统计分析', reuse: true } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PurchaseRoutingModule { }
