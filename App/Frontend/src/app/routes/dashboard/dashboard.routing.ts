import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard.component';

// TODO: import components
// import { DemoComponent } from './demo/demo.component';

const routes: Routes = [
    { path: '', component: DashboardComponent, pathMatch: 'full' },
    { path: 'purchase', loadChildren: './purchase/purchase.module#PurchaseModule' },
    { path: 'warehouse', loadChildren: './warehouse/warehouse.module#WarehouseModule' },
    { path: 'sale', loadChildren: './sale/sale.module#SaleModule' },
    { path: 'print', loadChildren: './print/print.module#PrintModule' },
    { path: 'document', loadChildren: './document/document.module#DocumentModule' },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class DashboardRoutingModule { }
