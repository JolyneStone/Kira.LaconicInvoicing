import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { environment } from '@env/environment';
// single pages
import { CallbackComponent } from './callback/callback.component';
import { Exception403Component } from './exception/403.component';
import { Exception404Component } from './exception/404.component';
import { Exception500Component } from './exception/500.component';
import { HomeComponent } from './home/home.component';
import { LayoutDefaultComponent } from '../layout/default/default.component';
import { LayoutPassportComponent } from '../layout/passport/passport.component';
import { UserLoginComponent } from './passport/login/login.component';
import { UserRegisterComponent } from './passport/register/register.component';
import { UserRegisterResultComponent } from './passport/register-result/register-result.component';
import { UserLockComponent } from './passport/lock/lock.component';
import { LoadDefaultMenuGuard } from '@shared/business/gurads/load.default-menu.guard/load-default-menu-guard';
import { LoadProfileMenuGuard } from '@shared/business/gurads/load.profile-menu.guard/load-profile-menu-guard';
import { LoadAdminMenuGuard } from '@shared/business/gurads/load.admin-menu.guard/load-admin-menu-guard';

const routes: Routes = [
  {
    path: '',
    component: LayoutDefaultComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', canActivate: [LoadDefaultMenuGuard], loadChildren: './dashboard/dashboard.module#DashboardModule', data: { title: '仪表盘' } },
      { path: 'profile', canActivate: [LoadProfileMenuGuard], loadChildren: './profile/profile.module#ProfileModule'},
    ],
  },
  {
    path: 'identity',
    loadChildren: './identity/identity.module#IdentityModule',
  },
  { path: 'admin', canActivate: [LoadAdminMenuGuard], loadChildren: './admin/admin.module#AdminModule' },
  // passport
  {
    path: 'passport',
    component: LayoutPassportComponent,
    children: [
      {
        path: 'login',
        component: UserLoginComponent,
        data: { title: '登录', titleI18n: 'app.login.login' },
      },
      {
        path: 'register',
        component: UserRegisterComponent,
        data: { title: '注册', titleI18n: 'app.register.register' },
      },
      {
        path: 'register-result',
        component: UserRegisterResultComponent,
        data: { title: '注册结果', titleI18n: 'app.register.register' },
      },
      {
        path: 'lock',
        component: UserLockComponent,
        data: { title: '锁屏', titleI18n: 'app.lock' },
      },
    ],
  },
  // 单页不包裹Layout
  { path: 'callback/:type', component: CallbackComponent },
  {
    path: '403',
    component: Exception403Component,
    data: { title: '无权访问该页面' },
  },
  {
    path: '404',
    component: Exception404Component,
    data: { title: '页面不存在' },
  },
  {
    path: '500',
    component: Exception500Component,
    data: { title: '服务器内部错误' },
  },
  { path: '**', redirectTo: 'dashboard' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes,
    {
       useHash: environment.useHash,
       scrollPositionRestoration: 'top'
    })],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
