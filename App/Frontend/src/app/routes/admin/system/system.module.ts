import { PrintComponent } from './print/print.component';
import { NgModule, Component, } from '@angular/core';
import { SystemRoutingModule, } from './system.routing';
import { SharedModule } from '@shared/shared.module';
import { SettingsComponent } from './settings/settings.component';
import { AuditOperationComponent } from './audit-operation/audit-operation.component';
import { AuditEntityComponent } from './audit-entity/audit-entity.component';
import { PackComponent } from './pack/pack.component';
import { BaseDataComponent } from './base-data/base-data.component';
import { BaseDataEditComponent } from './base-data-edit/base-data-edit.component';
import { NoticeComponent } from './notice/notice.component';
import { PrintEditComponent } from './print-edit/print-edit.component';
import { DocumentComponent } from './document/document.component';
import { DocumentEditComponent } from './document-edit/document-edit.component';

const COMPONENTS = [
  SettingsComponent,
  AuditOperationComponent,
  AuditEntityComponent,
  PackComponent,
  BaseDataComponent,
  BaseDataEditComponent,
  NoticeComponent,
  PrintComponent,
  PrintEditComponent,
  DocumentComponent,
  DocumentEditComponent,
];

@NgModule({
  declarations: [
    ...COMPONENTS,
  ],
  imports: [
    SystemRoutingModule,
    SharedModule
  ],
  providers: [
  ],
  entryComponents: [
    BaseDataEditComponent
  ]
})
export class SystemModule { }
