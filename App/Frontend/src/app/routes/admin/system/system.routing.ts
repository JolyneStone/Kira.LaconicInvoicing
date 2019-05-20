import { PrintComponent } from './print/print.component';
import { NoticeComponent } from './notice/notice.component';
import { NgModule, } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SettingsComponent } from './settings/settings.component';
import { AuditOperationComponent } from './audit-operation/audit-operation.component';
import { AuditEntityComponent } from './audit-entity/audit-entity.component';
import { PackComponent } from './pack/pack.component';
import { BaseDataComponent } from './base-data/base-data.component';
import { PrintEditComponent } from './print-edit/print-edit.component';
import { DocumentComponent } from './document/document.component';
import { DocumentEditComponent } from './document-edit/document-edit.component';

const routes: Routes = [
  { path: 'settings', component: SettingsComponent, data: { title: '系统设置', reuse: true } },
  { path: 'audit-operation', component: AuditOperationComponent, data: { title: '操作审计', reuse: true } },
  { path: 'audit-entity', component: AuditEntityComponent, data: { title: '数据审计', reuse: true } },
  { path: 'pack', component: PackComponent, data: { title: '模块包', reuse: true } },
  { path: 'base-data', component: BaseDataComponent, data: { title: '数据字典', reuse: true } },
  { path: 'notice', component: NoticeComponent, data: { title: '公告发布', reuse: true } },
  { path: 'print', component: PrintComponent, data: { title: '套打模板', reuse: true } },
  { path: 'print-add', component: PrintEditComponent, data: { title: '添加套打模板', reuse: true } },
  { path: 'print-edit/:id', component: PrintEditComponent, data: { title: '套打模板编辑', reuse: true } },
  { path: 'document', component: DocumentComponent, data: { title: '文档模板', reuse: true } },
  { path: 'document-add', component: DocumentEditComponent, data: { title: '添加文档模板', reuse: true } },
  { path: 'document-edit/:id', component: DocumentEditComponent, data: { title: '文档模板编辑', reuse: true } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemRoutingModule { }
