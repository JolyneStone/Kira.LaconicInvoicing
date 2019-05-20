import { Component, OnInit } from '@angular/core';
import { User, SettingsService } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'layout-sidebar',
  templateUrl: './layout-sidebar.component.html',
  styles: []
})
export class LayoutSidebarComponent {
    user: User;

    constructor(
      settings: SettingsService,
      public msgSrv: NzMessageService
    ) {
      this.user = settings.user || {};
    }
}
