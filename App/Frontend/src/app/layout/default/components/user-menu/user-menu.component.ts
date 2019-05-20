import { NzMessageService } from 'ng-zorro-antd';
import { AjaxResultType } from './../../../../shared/osharp/osharp.model';
import { OsharpService } from './../../../../shared/osharp/services/osharp.service';
import { Component, OnInit, AfterViewInit } from '@angular/core';
import { SettingsService, MenuService, User } from '@delon/theme';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { Router } from '@angular/router';
import { appSettings } from 'app/app.settings';

@Component({
  selector: 'layout-user-menu',
  templateUrl: './user-menu.component.html',
  styleUrls: ['./user-menu.component.scss'],
})
export class UserMenuComponent {
  constructor(
    public identity: IdentityService,
    public osharp: OsharpService,
    public settings: SettingsService,
    public router: Router,
    private msgSrv: NzMessageService,
    private menuService: MenuService,
  ) {}

  get inAdminModule() {
    return this.router.url.startsWith('/admin/');
  }

  get avatar() {
    return appSettings.service_url + appSettings.avatarPrefix + this.settings.user.avatar;
  }

  logout() {
    this.identity.logout().then(res => {
      if (res.type === AjaxResultType.success) {
        this.msgSrv.success('用户退出成功');

        // let url = this.router.url;
        // if (url.startsWith('/admin/')) {
        //   url = '/dashboard';
        // }
        setTimeout(() => {
          this.router.navigateByUrl('/identity/login');
        }, 100);
        return;
      }
      this.msgSrv.error(`用户登出失败：${res.content}`);
    });
  }
}
