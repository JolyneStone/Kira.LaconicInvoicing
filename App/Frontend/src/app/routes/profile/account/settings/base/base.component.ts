import {
  Component,
  ChangeDetectionStrategy,
  OnInit,
  ChangeDetectorRef,
  Injector,
  ViewChild
} from '@angular/core';
import { _HttpClient, SettingsService } from '@delon/theme';
import { zip } from 'rxjs';
import {
  NzMessageService,
  UploadFile,
  UploadXHRArgs,
  NzButtonComponent,
} from 'ng-zorro-antd';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { UserDetail } from '@shared/business/app.model';
import { AuthConfig, User } from '@shared/osharp/osharp.model';
import { DomSanitizer } from '@angular/platform-browser';
import { I18NService } from '@core/i18n/i18n.service';
import { appSettings } from 'app/app.settings';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-account-settings-base',
  templateUrl: './base.component.html',
  styleUrls: ['./base.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileAccountSettingsBaseComponent extends ComponentBase
  implements OnInit {
  userLoading = true;
  userDetail: UserDetail;
  fileAccept = ['image/png', 'image/jpeg', 'image/gif', 'image/bmp'];
  avatarUrl: any;
  isUpdateAvatar: boolean = false;
  @ViewChild('fileInput')
  private fileInput: NzButtonComponent;

  constructor(
    private sanitizer: DomSanitizer,
    private http: _HttpClient,
    private cdr: ChangeDetectorRef,
    private msg: NzMessageService,
    private settingSrv: SettingsService,
    private i18NService: I18NService,
    injector: Injector,
  ) {
    super(injector);
    super.checkAuth();
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.UserCenter.UserCenter.Profile', [
      'Info',
      'UpdateInfo',
      'UpdateAvatar',
    ]);
  }

  ngOnInit(): void {
    zip(
      this.http.get('api/usercenter/profile/info'),
      this.http.get('/geo/province'),
    )
    .pipe(
      catchError(([userDetail, province]) => {
        return [userDetail, province];
      }),
    )
    .subscribe(([userDetail, province]: any) => {
      this.userLoading = false;
      this.userDetail = userDetail;
      if (userDetail.profile) {
        const profileObj = JSON.parse(userDetail.profile);
        this.userDetail.personalProfile = profileObj.personalProfile;
        this.userDetail.country = profileObj.country;
        this.userDetail.province = profileObj.province;
        this.userDetail.city = profileObj.city;
        this.userDetail.address = profileObj.address;
        this.userDetail.department = profileObj.department;
      }
      this.provinces = province;
      this.avatarUrl = appSettings.service_url + appSettings.avatarPrefix + userDetail.avatar;
      this.choProvince(userDetail.province, false);
      this.cdr.detectChanges();
    });
  }

  // #region geo

  provinces: any[] = [];
  cities: any[] = [];

  choProvince(pid: string, cleanCity = true) {
    this.http.get(`/geo/citiesByProvince/${pid}`).subscribe((res: any) => {
      this.cities = res;
      if (cleanCity) this.userDetail.city = '';
      this.cdr.detectChanges();
    });
  }

  // #endregion

  save() {
    //(this.userDetail as any).avatarFile = this.avatarFile;
    this.http
      .post('api/usercenter/profile/updateinfo', this.userDetail)
      .subscribe(
        (res: any) => {
          const user: User = {
            id: this.userDetail.userId,
            nickName: this.userDetail.nickName,
            telephone: this.userDetail.telephone,
            isAdmin: this.settingSrv.user.isAdmin,
            name: this.settingSrv.user.name,
            email: this.settingSrv.user.email,
            roles: this.settingSrv.user.roles,
            avatar: this.userDetail.avatar,
          };

          if (this.isUpdateAvatar) {
            const formData = new FormData();
            formData.append('avatarFile', (this.fileInput.el as any).files[0]);
            this.http
              .post('api/usercenter/profile/updateAvatar', formData)
              .subscribe(
                (res1: any) => {
                  if (res1.data) {
                    user.avatar = res1.data;
                    this.settingSrv.setUser(user);
                    this.msg.success(
                      this.i18NService.fanyi('app.account.settings.save-success'),
                    );
                  }
                },
                (res1: any) => {
                  this.msg.success(res1.content);
                },
              );
          } else {

            this.settingSrv.setUser(user);
            this.msg.success(
              this.i18NService.fanyi('app.account.settings.save-success'),
            );
          }
        },
        (res: any) => {
          this.msg.success(res.content);
        },
      );
    return false;
  }

  // handlePreview = (file: UploadFile) => {
  //   this.userDetail.avatar = file.url || file.thumbUrl;
  //   this.avatarUrl = this.userDetail.avatar;
  // };

  public onChangeSelectFile(event) {
    const file = event.target.files[0];
    const imgUrl = window.URL.createObjectURL(file);
    const sanitizerUrl = this.sanitizer.bypassSecurityTrustUrl(imgUrl);

    this.isUpdateAvatar = true;
    this.avatarUrl = sanitizerUrl;
  }

  changeAvatar = (item: UploadXHRArgs) => {
    // 构建一个 FormData 对象，用于存储文件或其他参数
    const formData = new FormData();
    // tslint:disable-next-line:no-any
    formData.append('avatarFile', item.file as any);
    return this.http.post(item.action!, formData).subscribe((res: any) => {
      this.osharp.ajaxResult(
        res,
        (result: any) => {
          this.avatarUrl = result.data;
        },
        (result: any) => {
          // 处理失败
          result.content || this.msg.info(result.content);
        },
      );
    });
  };

  // changeAvatar = (item: UploadXHRArgs) => {
  //   // 构建一个 FormData 对象，用于存储文件或其他参数
  //   const formData = new FormData();
  //   // tslint:disable-next-line:no-any
  //   formData.append('avatarFile', item.file as any);
  //   const req = new HttpRequest('POST', item.action!, formData, {
  //     reportProgress: true,
  //     withCredentials: true
  //   });
  //   // 始终返回一个 `Subscription` 对象，nz-upload 会在适当时机自动取消订阅
  //   return this.httpClient.request(req).subscribe(
  //     (event: HttpEvent<{}>) => {
  //       if (event.type === HttpEventType.UploadProgress) {
  //         if (event.total! > 0) {
  //           // tslint:disable-next-line:no-any
  //           (event as any).percent = (event.loaded / event.total!) * 100;
  //         }
  //         // 处理上传进度条，必须指定 `percent` 属性来表示进度
  //         item.onProgress!(event, item.file!);
  //       } else if (event instanceof HttpResponse) {
  //         // 处理成功
  //         item.onSuccess!(event.body, item.file!, event);
  //       }
  //     },
  //     err => {
  //       // 处理失败
  //       item.onError!(err, item.file!);
  //     }
  //   );
  // };
}
