import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DocumentWindowComponent } from './document-window/document-window.component';

const routes: Routes = [
  { path: '', redirectTo: 'document-window', pathMatch: 'full' },
  { path: 'document-window', component: DocumentWindowComponent, data: { title: '表单导出', reuse: true } }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DocumentRoutingModule { }
