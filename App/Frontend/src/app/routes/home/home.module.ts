import { NgModule, } from '@angular/core';
import { HomeComponent } from './home.component';
import { HomeRoutingModule, } from './home.routing';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [
    HomeComponent,
  ],
  imports: [
    SharedModule,
    HomeRoutingModule
  ],
  providers: [
  ]
})
export class HomeModule { }
