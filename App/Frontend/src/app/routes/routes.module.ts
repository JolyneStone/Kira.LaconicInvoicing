import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';
import { AppRoutingModule } from './app.routing';
// single pages
import { CallbackComponent } from './callback/callback.component';
import { Exception403Component } from './exception/403.component';
import { Exception404Component } from './exception/404.component';
import { Exception500Component } from './exception/500.component';
import { UserLockComponent } from './passport/lock/lock.component';
import { UserLoginComponent } from './passport/login/login.component';
import { UserRegisterComponent } from './passport/register/register.component';
import { UserRegisterResultComponent } from './passport/register-result/register-result.component';

const COMPONENTS = [
  // single pages
  CallbackComponent,
  UserLockComponent,
  UserLoginComponent,
  UserRegisterComponent,
  UserRegisterResultComponent,
  Exception403Component,
  Exception404Component,
  Exception500Component,
];
const COMPONENTS_NOROUNT = [];

@NgModule({
  imports: [
    SharedModule,
    AppRoutingModule
  ],
  declarations: [
    ...COMPONENTS,
    ...COMPONENTS_NOROUNT
  ],
  entryComponents: COMPONENTS_NOROUNT
})
export class RoutesModule {}
