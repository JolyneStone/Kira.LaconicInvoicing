import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WarehouseRoutingModule } from './warehouse.routing';
import { WarehouseListComponent } from './warehouse-list/warehouse-list.component';
import { WarehouseEditComponent } from './warehouse-edit/warehouse-edit.component';
import { SharedModule } from '@shared/shared.module';
import { InboundReceiptListComponent } from './inbound-receipt-list/inbound-receipt-list.component';
import { InboundReceiptEditComponent } from './inbound-receipt-edit/inbound-receipt-edit.component';
import { OutboundReceiptListComponent } from './outbound-receipt-list/outbound-receipt-list.component';
import { OutboundReceiptEditComponent } from './outbound-receipt-edit/outbound-receipt-edit.component';
import { InventoryListComponent } from './inventory-list/inventory-list.component';
import { InventoryEditComponent } from './inventory-edit/inventory-edit.component';
import { TransferOrderListComponent } from './transfer-order-list/transfer-order-list.component';
import { TransferOrderEditComponent } from './transfer-order-edit/transfer-order-edit.component';
import { StatisticsComponent } from './statistics/statistics.component';

const COMPONENTS = [
  WarehouseListComponent,
  WarehouseEditComponent,
  InboundReceiptListComponent,
  InboundReceiptEditComponent,
  OutboundReceiptListComponent,
  OutboundReceiptEditComponent,
  InventoryListComponent,
  InventoryEditComponent,
  TransferOrderListComponent,
  TransferOrderEditComponent,
  StatisticsComponent,
];

@NgModule({
  declarations: [
    ...COMPONENTS,
  ],
  imports: [
    SharedModule,
    WarehouseRoutingModule
  ]
})
export class WarehouseModule { }
