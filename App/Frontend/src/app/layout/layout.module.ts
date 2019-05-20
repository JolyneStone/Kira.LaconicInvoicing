
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MenuService } from '@delon/theme';
import { SharedModule } from '@shared/shared.module';
import { LayoutModule as CdkLayoutModule } from '@angular/cdk/layout';
import { LayoutDefaultComponent } from './default/default.component';
import { UserMenuComponent } from './default/components/user-menu/user-menu.component';
import { FooterComponent } from './default/components/footer/footer.component';

import { LayoutHeaderComponent } from './default/components/layout-header/layout-header.component';
import { LayoutSidebarComponent } from './default/components/layout-sidebar/layout-sidebar.component';

import { SettingDrawerComponent } from './default/components/setting-drawer/setting-drawer.component';
import { SettingDrawerItemComponent } from './default/components/setting-drawer/setting-drawer-item.component';

const SETTINGDRAWER = [SettingDrawerComponent, SettingDrawerItemComponent];

const COMPONENTS = [
  LayoutHeaderComponent,
  LayoutSidebarComponent,
  LayoutDefaultComponent,
  UserMenuComponent,
  FooterComponent,
  ...SETTINGDRAWER
];

// passport
import { LayoutPassportComponent } from './passport/passport.component';
const PASSPORT = [LayoutPassportComponent];


@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    CdkLayoutModule,
  ],
  entryComponents: SETTINGDRAWER,
  providers: [
  ],
  declarations: [
    ...COMPONENTS,
    ...PASSPORT
  ],
  exports: [
    ...COMPONENTS,
    ...PASSPORT
  ]
})
export class LayoutModule {
 }
