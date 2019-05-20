import { NgModule } from '@angular/core';
import { PurchaseRoutingModule } from './purchase.routing';
import { SharedModule } from '@shared/shared.module';
import { VendorListComponent } from './vendor-list/vendor-list.component';
import { VendorEditComponent } from './vendor-edit/vendor-edit.component';
import { MaterialListComponent } from './material-list/material-list.component';
import { MaterialEditComponent } from './material-edit/material-edit.component';
import { PurchaseOrderListComponent } from './purchase-order-list/purchase-order-list.component';
import { PurchaseOrderEditComponent } from './purchase-order-edit/purchase-order-edit.component';
import { StatisticsComponent } from './statistics/statistics.component';

const COMPONENTS = [
  VendorListComponent,
  VendorEditComponent,
  MaterialListComponent,
  MaterialEditComponent,
  PurchaseOrderListComponent,
  PurchaseOrderEditComponent,
  StatisticsComponent,
];

@NgModule({
  declarations: [
    ...COMPONENTS,
  ],
  imports: [
    SharedModule,
    PurchaseRoutingModule
  ]
})
export class PurchaseModule { }
