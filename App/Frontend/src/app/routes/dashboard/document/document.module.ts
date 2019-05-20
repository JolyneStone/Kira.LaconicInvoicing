import { SharedModule } from '@shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DocumentWindowComponent } from './document-window/document-window.component';
import { DocumentRoutingModule } from './document.routing';

const COMPONENTS = [
  DocumentWindowComponent,
];

@NgModule({
  declarations: [
    ...COMPONENTS
  ],
  imports: [
    SharedModule,
    DocumentRoutingModule
  ]
})
export class DocumentModule { }
