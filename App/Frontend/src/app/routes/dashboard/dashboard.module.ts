import { PurchaseModule } from './purchase/purchase.module';
import { NgModule, } from '@angular/core';
import { DashboardComponent } from './dashboard.component';
import { SharedModule } from '@shared/shared.module';

import '@antv/g2';
import '@antv/g2-plugin-slider';
import '@antv/data-set';
import { DashboardRoutingModule } from './dashboard.routing';
import { DocumentModule } from './document/document.module';


@NgModule({
  declarations: [
    DashboardComponent,
  ],
  imports: [
    SharedModule,
    DashboardRoutingModule,
  ],
  providers: [
  ]
})
export class DashboardModule { }
