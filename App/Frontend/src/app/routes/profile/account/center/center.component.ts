import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  ViewChild,
  ElementRef,
  OnDestroy,
  AfterViewInit,
  Injector,
} from '@angular/core';
import { Router, ActivationEnd } from '@angular/router';
import { filter, catchError } from 'rxjs/operators';
import { _HttpClient, SettingsService } from '@delon/theme';
import { zip, Subscription } from 'rxjs';
import { appSettings } from 'app/app.settings';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { Inject } from '@angular/compiler/src/core';
import { AuthConfig } from '@shared/osharp/osharp.model';
import { UserDetail } from '@shared/business/app.model';

@Component({
  selector: 'app-account-center',
  templateUrl: './center.component.html',
  styleUrls: ['./center.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileAccountCenterComponent extends ComponentBase implements OnInit, OnDestroy {
  private router$: Subscription;
  avatar: string;
  userDetail: UserDetail;
  notice: any;

  constructor(
    private router: Router,
    private http: _HttpClient,
    private cdr: ChangeDetectorRef,
    public setting: SettingsService,
    injector: Injector
  ) {
    super(injector);
    super.checkAuth();
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.UserCenter.UserCenter.Profile', [
      'Info',
    ]);
  }

  private setActive() {
    const key = this.router.url.substr(this.router.url.lastIndexOf('/') + 1);
  }

  ngOnInit(): void {
    zip(
      this.http.get('api/usercenter/profile/info'),
      this.http.get('/api/notice')
    )
    .pipe(
      catchError(([userDetail, notice]) => {
        return [userDetail, notice];
      }),
    )
    .subscribe(
      ([userDetail, notice]: any) => {
        this.userDetail = userDetail;
        this.avatar = appSettings.service_url + appSettings.avatarPrefix + this.userDetail.avatar;
        if (userDetail.profile) {
          const profileObj = JSON.parse(userDetail.profile);
          this.userDetail.personalProfile = profileObj.personalProfile;
          this.userDetail.country = profileObj.country;
          this.userDetail.province = profileObj.province;
          this.userDetail.city = profileObj.city;
          this.userDetail.address = profileObj.address;
          this.userDetail.department = profileObj.department;
        }
        
        // this.notice = notice;

        zip(
          this.http.get(`/geo/${this.userDetail.province}`),
          this.http.get(`/geo/${this.userDetail.city}`),
        )
        .pipe(
          catchError(([province, city]) => {
            return [province, city];
          }),
        )
        .subscribe(
          ([province, city]: any) => {
            if (province) {
              this.userDetail.province = province.name;
            }
            if (city) {
              this.userDetail.city = city.name;
            }
            this.cdr.detectChanges();
          }
        );
      },
    );
    this.router$ = this.router.events
      .pipe(filter(e => e instanceof ActivationEnd))
      .subscribe(() => this.setActive());
    this.setActive();
  }

  taging = false;
  tagValue = '';
  @ViewChild('tagInput')
  private tagInput: ElementRef;
  tagShowIpt() {
    this.taging = true;
    this.cdr.detectChanges();
    (this.tagInput.nativeElement as HTMLInputElement).focus();
  }

  tagBlur() {
    const { userDetail, cdr, tagValue } = this;
    // if (
    //   tagValue &&
    //   user.tags.filter(tag => tag.label === tagValue).length === 0
    // ) {
    //   user.tags.push({ label: tagValue });
    // }
    this.tagValue = '';
    this.taging = false;
    cdr.detectChanges();
  }

  tagEnter(e: KeyboardEvent) {
    if (e.keyCode === 13) this.tagBlur();
  }

  ngOnDestroy() {
    this.router$.unsubscribe();
  }
}
