import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PrintWindowComponent } from './print-window/print-window.component';

const routes: Routes = [
  { path: '', redirectTo: 'print-window', pathMatch: 'full' },
  { path: 'print-window', component: PrintWindowComponent, data: { title: '表单打印', reuse: true } }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PrintRoutingModule {}
