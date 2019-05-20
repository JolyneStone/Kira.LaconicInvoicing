/**
 * 进一步对基础模块的导入提炼
 * 有关模块注册指导原则请参考：https://github.com/cipchk/ng-alain/issues/180
 */
import {
  NgModule,
  Optional,
  SkipSelf,
  ModuleWithProviders,
} from '@angular/core';
import { RouteReuseStrategy } from '@angular/router';
import { throwIfAlreadyLoaded } from '@core/module-import-guard';

import { NgZorroAntdModule } from 'ng-zorro-antd';
import { AlainThemeModule } from '@delon/theme';
import { DelonABCModule, ReuseTabService, ReuseTabStrategy } from '@delon/abc';
import { DelonAuthModule } from '@delon/auth';
import { DelonACLModule } from '@delon/acl';
import { DelonCacheModule } from '@delon/cache';
import { DelonUtilModule } from '@delon/util';

// region: global config functions

import { PageHeaderConfig } from '@delon/abc';
export function pageHeaderConfig(): PageHeaderConfig {
  return Object.assign(new PageHeaderConfig(), { home_i18n: 'dashboard' });
}

import { DelonAuthConfig } from '@delon/auth';
export function delonAuthConfig(): DelonAuthConfig {
  return Object.assign(new DelonAuthConfig(), <DelonAuthConfig>{
    login_url: 'identity/login',
    ignores: [
      /assets/,
      /login/,
      /jwtoken/,
      /register/,
      /oAuth2/,
      /oAuth2Callback/,
      /logout/,
      /ConfirmEmail/,
      /sendConfirmMail/,
      /SendResetPasswordMail/,
      /verifycode/,
      /CheckEmailExists/,
      /CheckVerifyCode/,
      /ResetPassword/,
      /CheckUserNameExists/,
      /CheckNickNameExists/,
      /getauthinfo/,
      /CheckUrlAuth/
    ]
  });
}

// endregion

// #region mock
import { DelonMockModule } from '@delon/mock';
import * as MOCKDATA from '../../_mock';
import { environment } from '@env/environment';
const MOCK_MODULES = !environment.production
  ? [DelonMockModule.forRoot({ data: MOCKDATA })]
  : [];
// #endregion

@NgModule({
  imports: [
    NgZorroAntdModule,
    AlainThemeModule.forRoot(),
    DelonABCModule,
    DelonAuthModule,
    DelonACLModule,
    DelonCacheModule,
    DelonUtilModule,
    ...MOCK_MODULES,
  ],
})
export class DelonModule {
  constructor(
    @Optional()
    @SkipSelf()
    parentModule: DelonModule,
  ) {
    throwIfAlreadyLoaded(parentModule, 'DelonModule');
  }

  static forRoot(): ModuleWithProviders {
    return {
      ngModule: DelonModule,
      providers: [
        // TIPS：若不需要路由复用需要移除以下代码及模板`<reuse-tab></reuse-tab>`
        {
          provide: RouteReuseStrategy,
          useClass: ReuseTabStrategy,
          deps: [ReuseTabService],
        },
        // TIPS：@delon/abc 有大量的全局配置信息，例如设置所有 `simple-table` 的页码默认为 `20` 行
        // { provide: SimpleTableConfig, useFactory: simpleTableConfig }
        { provide: PageHeaderConfig, useFactory: pageHeaderConfig },
        { provide: DelonAuthConfig, useFactory: delonAuthConfig },
      ],
    };
  }
}
