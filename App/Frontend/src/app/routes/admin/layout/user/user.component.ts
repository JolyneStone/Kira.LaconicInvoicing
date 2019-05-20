import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { SettingsService, MenuService, User } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';
import { AjaxResultType } from '@shared/osharp/osharp.model';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { appSettings } from 'app/app.settings';

@Component({
  selector: 'header-user',
  templateUrl: './user.component.html',
})
export class HeaderUserComponent {
  constructor(
    public settings: SettingsService,
    public osharp: OsharpService,
    private msgSrv: NzMessageService,
    private identity: IdentityService,
    public router: Router,
    private menuService: MenuService,
  ) {
  }

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

        let url = this.router.url;
        if (url.startsWith('/admin/')) {
          url = '/dashboard';
        }
        setTimeout(() => {
          this.router.navigateByUrl(url);
        }, 100);
        return;
      }
      this.msgSrv.error(`用户登出失败：${res.content}`);
    });
  }
}
