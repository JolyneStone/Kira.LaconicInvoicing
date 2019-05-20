import { NgModule, ModuleWithProviders, LOCALE_ID } from '@angular/core';
import { CommonModule, registerLocaleData } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
// delon
import { AlainThemeModule, ALAIN_I18N_TOKEN } from '@delon/theme';
import { DelonABCModule } from '@delon/abc';
import { DelonACLModule } from '@delon/acl';
import { DelonFormModule } from '@delon/form';
import { DelonChartModule } from '@delon/chart';

// region: third libs
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { CountdownModule } from 'ngx-countdown';

import { OsharpModule } from '@shared/osharp/osharp.module';
import { MaterialModule } from '@shared/material.module';

// i18n
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';

import { HeaderStorageComponent } from './business/components/storage.component';
import { HeaderNotifyComponent } from './business/components/notify.component';
import { HeaderIconComponent } from './business/components/icon.component';
import { HeaderFullScreenComponent } from './business/components/fullscreen.component';
import { LogoComponent } from './business/components/logo.component';
import { LoadAdminMenuGuard } from './business/gurads/load.admin-menu.guard/load-admin-menu-guard';
import { LoadProfileMenuGuard } from './business/gurads/load.profile-menu.guard/load-profile-menu-guard';
import { LoadDefaultMenuGuard } from './business/gurads/load.default-menu.guard/load-default-menu-guard';
import { BusinessModule } from './business/business.module';
import { BrowserModule } from '@angular/platform-browser';

const THIRDMODULES = [
  NgZorroAntdModule,
  CountdownModule,
  OsharpModule,
  BusinessModule,
];

// endregion

// region: your componets & directives
const COMPONENTS = [
];
const DIRECTIVES = [];

const GUARDS = [
];

// endregion

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    ReactiveFormsModule,
    AlainThemeModule.forChild(),
    DelonABCModule,
    DelonACLModule,
    DelonFormModule,
    DelonChartModule,
    MaterialModule,
    // third libs
    ...THIRDMODULES,
  ],
  declarations: [
    // your components
    ...COMPONENTS,
    ...DIRECTIVES
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    AlainThemeModule,
    DelonABCModule,
    DelonACLModule,
    DelonFormModule,
    DelonChartModule,
    MaterialModule,
    // i18n
    TranslateModule,
    // third libs
    ...THIRDMODULES,
    // your components
    ...COMPONENTS,
    ...DIRECTIVES
  ],
  providers: [
    ...GUARDS
  ]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule
    };
  }
}
