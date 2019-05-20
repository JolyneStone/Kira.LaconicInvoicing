import { SaleRoutingModule } from './sale.routing';
import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { CustomerEditComponent } from './customer-edit/customer-edit.component';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { SaleOrderListComponent } from './sale-order-list/sale-order-list.component';
import { SaleOrderEditComponent } from './sale-order-edit/sale-order-edit.component';
import { StatisticsComponent } from './statistics/statistics.component';

const COMPONENTS = [
  CustomerListComponent,
  CustomerEditComponent,
  ProductListComponent,
  ProductEditComponent,
  SaleOrderListComponent,
  SaleOrderEditComponent,
  StatisticsComponent,
];

@NgModule({
  declarations: [
    ...COMPONENTS,
  ],
  imports: [
    SharedModule,
    SaleRoutingModule
  ]
})
export class SaleModule { }
