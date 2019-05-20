import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SettingsService, TitleService, ALAIN_I18N_TOKEN } from '@delon/theme';
import { Observable } from 'rxjs';
import { User } from '@shared/osharp/osharp.model';
import { ACLService } from '@delon/acl';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { zip } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { I18NService } from '../i18n/i18n.service';
import { map, catchError } from 'rxjs/operators';

/**
 * 用于应用启动时
 * 一般用来获取应用所需要的基础数据等
 */
@Injectable()
export class StartupService {
  constructor(
    private settingSrv: SettingsService,
    private aclSrv: ACLService,
    private titleService: TitleService,
    private http: HttpClient,
    private translate: TranslateService,
    @Inject(ALAIN_I18N_TOKEN) private i18n: I18NService,
  ) { }

  load(): Promise<any> {
    return new Promise(resolve => {
      zip(
        this.http.get(`assets/tmp/i18n/${this.i18n.defaultLang}.json`),
        this.http.get('assets/osharp/app-data.json'),
      )
        .pipe(
          // 接收其他拦截器后产生的异常消息
          catchError(([langData, appData]) => {
            resolve(null);
            return [langData, appData];
          }),
        )
        .subscribe(
          ([langData, appData]) => {
            if (!appData) {
              return;
            }

            // setting language data
            this.translate.setTranslation(this.i18n.defaultLang, langData);
            this.translate.setDefaultLang(this.i18n.defaultLang);

            const curLang = localStorage.getItem('currentLanguage');
            if (curLang && curLang.match(/ch|en/)) {
              this.translate.use(curLang);
            }

            // 应用信息：包括站点名、描述、年份
            this.settingSrv.setApp(appData.app);
            // 初始化菜单
            // this.menuService.add(data.menu);
            // 设置页面标题的后缀
            this.titleService.default = '';
            this.titleService.suffix = appData.app.name;
            // 刷新用户信息
            this.refreshUser().subscribe();
          },
          () => { },
          () => {
            resolve(null);
          },
        );
    });
  }

  /** 刷新用户信息 */
  private refreshUser(): Observable<User> {
    if (this.settingSrv.user === null)
      return;
    let url = 'api/identity/profile';
    return this.http.get(url).pipe(
      map((res: any) => {
        if (!res || res == {}) {
          this.settingSrv.setUser({});
          this.aclSrv.setRole([]);
          return {};
        }
        let user: User = {
          id: res.id,
          name: res.userName,
          nickName: res.nickName,
          avatar: res.headImg,
          email: res.email,
          roles: res.roles,
          isAdmin: res.isAdmin,
          telephone: res.phoneNumber,
        };
        this.settingSrv.setUser(user);
        // 更新角色
        this.aclSrv.setRole(user.roles);
        return user;
      }),
    );
  }
}
