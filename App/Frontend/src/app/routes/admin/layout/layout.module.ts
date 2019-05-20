import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminLayoutComponent } from './layout.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { SharedModule } from '@shared/shared.module';
import { HeaderComponent } from './header/header.component';
import { HeaderUserComponent } from './user/user.component';

const COMPONENTS = [
  AdminLayoutComponent,
  SidebarComponent,
  HeaderComponent,
  HeaderUserComponent
];

@NgModule({
  declarations: [
    ...COMPONENTS
  ],
  imports: [
    CommonModule,
    SharedModule,
  ],
  exports: [],
  providers: [],
})
export class AdminLayoutModule { }
