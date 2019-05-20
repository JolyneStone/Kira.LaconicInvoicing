import { StatisticsComponent } from './../warehouse/statistics/statistics.component';
import { InventoryListComponent } from './inventory-list/inventory-list.component';
import { TransferOrderEditComponent } from './transfer-order-edit/transfer-order-edit.component';
import { TransferOrderListComponent } from './transfer-order-list/transfer-order-list.component';
import { OutboundReceiptListComponent } from './outbound-receipt-list/outbound-receipt-list.component';
import { InboundReceiptEditComponent } from './inbound-receipt-edit/inbound-receipt-edit.component';
import { InboundReceiptListComponent } from './inbound-receipt-list/inbound-receipt-list.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WarehouseListComponent } from './warehouse-list/warehouse-list.component';
import { OutboundReceiptEditComponent } from './outbound-receipt-edit/outbound-receipt-edit.component';

const routes: Routes = [
  { path: '', redirectTo: 'warehouselist', pathMatch: 'full' },
  { path: 'warehouselist', component: WarehouseListComponent, data: { title: '仓库管理', reuse: true } },
  { path: 'inboundreceiptlist', component: InboundReceiptListComponent, data: { title: '入库单管理', reuse: true } },
  { path: 'inboundreceipt-add', component: InboundReceiptEditComponent, data: { title: '添加入库单', reuse: true}},
  { path: 'inboundreceipt-edit/:id', component: InboundReceiptEditComponent, data: { title: '编辑入库单', reuse: true}},
  { path: 'outboundreceiptlist', component: OutboundReceiptListComponent, data: { title: '出库单管理', reuse: true } },
  { path: 'outboundreceipt-add', component: OutboundReceiptEditComponent, data: { title: '添加出库单', reuse: true}},
  { path: 'outboundreceipt-edit/:id', component: OutboundReceiptEditComponent, data: { title: '编辑出库单', reuse: true}},
  { path: 'transferorderlist', component: TransferOrderListComponent, data: { title: '调拨单管理', reuse: true } },
  { path: 'transferorder-add', component: TransferOrderEditComponent, data: { title: '添加调拨单', reuse: true}},
  { path: 'transferorder-edit/:id', component: TransferOrderEditComponent, data: { title: '编辑调拨单', reuse: true}},
  { path: 'inventorylist/:id', component: InventoryListComponent, data: { title: '库存管理', reuse: true } },
  { path: 'statistics', component: StatisticsComponent, data: { title: '统计分析', reuse: true } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WarehouseRoutingModule { }
