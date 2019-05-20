import { Component } from '@angular/core';
import { App, SettingsService } from '@delon/theme';
import { Router } from '@angular/router';

@Component({
  selector: 'layout-header',
  templateUrl: './layout-header.component.html',
  styleUrls: ['./layout-header.component.scss']
})
export class LayoutHeaderComponent {
  searchToggleStatus: boolean;
  app: App;

  constructor(
    public settings: SettingsService,
    router: Router
  ) {
    this.app = settings.app;
  }

  get user() {
    return this.settings.user;
  }

  toggleCollapsedSideabar() {
    this.settings.setLayout('collapsed', !this.settings.layout.collapsed);
  }

  searchToggleChange() {
    this.searchToggleStatus = !this.searchToggleStatus;
  }
}
