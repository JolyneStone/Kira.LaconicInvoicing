import { PrintWindowComponent } from './print-window/print-window.component';
import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { PrintRoutingModule } from './print.routing';

const COMPONENTS = [
  PrintWindowComponent,
];

@NgModule({
  declarations: [
    ...COMPONENTS,
  ],
  imports: [
    SharedModule,
    PrintRoutingModule
  ]
})
export class PrintModule { }