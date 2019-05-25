import { Component, HostBinding, ViewChild, ViewContainerRef, OnInit, AfterViewInit, OnDestroy, ElementRef, Renderer2, Inject, ComponentFactoryResolver } from '@angular/core';
import { Router, RouteConfigLoadStart, NavigationError, NavigationEnd } from '@angular/router';
import { ScrollService, SettingsService, App } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';
import { Subject } from 'rxjs';
import { environment } from '@env/environment';
import { SettingDrawerComponent } from './components/setting-drawer/setting-drawer.component';
import { takeUntil } from 'rxjs/operators';
import { DOCUMENT } from '@angular/common';
import { updateHostClass } from '@delon/util';

@Component({
  selector: 'layout-default', templateUrl: './default.component.html', styleUrls: ['./default.component.scss']
}

) export class LayoutDefaultComponent implements OnInit, AfterViewInit, OnDestroy  {
  private unsubscribe$ = new Subject<void>();
  @ViewChild('settingHost', { read: ViewContainerRef })
  private settingHost: ViewContainerRef;
  isFetching = false;
  // mainNavs = [
  //   { text: "首页", icon: "home", link: '/#/home' },
  //   // { text: "特性", icon: "ac_unit", link: '/#/', disabled: true },
  //   // { text: "组件", icon: "widgets", link: '/#/', disabled: true },
  // ];
  // mainActions = [];
 
  constructor(
    router: Router,
    scroll: ScrollService,
    msgSrv: NzMessageService,
    private settings: SettingsService,
    private resolver: ComponentFactoryResolver,
    private el: ElementRef,
    private renderer: Renderer2,
    @Inject(DOCUMENT) private doc: any,
  ) {
    router.events.pipe(takeUntil(this.unsubscribe$)).subscribe(evt => {
      if (!this.isFetching && evt instanceof RouteConfigLoadStart) {
        this.isFetching = true;
      }
      if (evt instanceof NavigationError) {
        this.isFetching = false;
        msgSrv.error(`无法加载${evt.url}路由`, { nzDuration: 1000 * 3 });
        return;
      }
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      setTimeout(() => {
        scroll.scrollToTop();
        this.isFetching = false;
      }, 100);
    });
  }
  
  private setClass() {
    const { el, doc, renderer, settings } = this;
    const layout = settings.layout;
    updateHostClass(
      el.nativeElement,
      renderer,
      {
        ['alain-default']: true,
        [`alain-default__fixed`]: layout.fixed,
        [`alain-default__collapsed`]: layout.collapsed,
      },
    );

    doc.body.classList[layout.colorWeak ? 'add' : 'remove']('color-weak');
  }

  ngAfterViewInit(): void {
    // Setting componet for only developer
    // if (!environment.production) {
    //   setTimeout(() => {
    //     const settingFactory = this.resolver.resolveComponentFactory(SettingDrawerComponent);
    //     this.settingHost.createComponent(settingFactory);
    //   }, 22);
    // }
  }

  ngOnInit() {
    const { settings, unsubscribe$ } = this;
    settings.notify.pipe(takeUntil(unsubscribe$)).subscribe(() => this.setClass());
    this.setClass();
  }

  ngOnDestroy() {
    const { unsubscribe$ } = this;
    unsubscribe$.next();
    unsubscribe$.complete();
  }
}
