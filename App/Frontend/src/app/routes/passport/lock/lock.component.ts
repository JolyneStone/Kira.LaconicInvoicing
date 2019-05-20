import { Router } from '@angular/router';
import { Component, Inject, Injector, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SettingsService, User } from '@delon/theme';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import {
  LoginDto,
  AuthConfig,
  AjaxResultType,
} from '@shared/osharp/osharp.model';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { NzMessageService } from 'ng-zorro-antd';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { appSettings } from 'app/app.settings';

@Component({
  selector: 'passport-lock',
  templateUrl: './lock.component.html',
  styleUrls: ['./lock.component.less'],
})
export class UserLockComponent {
  //extends ComponentBase {
  f: FormGroup;
  dto: LoginDto = new LoginDto();
  canSubmit = true;
  error: string;

  constructor(
    fb: FormBuilder,
    @Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService,
    public settings: SettingsService,
    private router: Router,
    private _service: IdentityService,
    private msgSrv: NzMessageService,
    injector: Injector,
  ) {
    //super(injector);
    //super.checkAuth();

    tokenService.clear();
    this.dto.account = settings.user.email;
    this.f = fb.group({
      password: [null, Validators.required],
    });
  }

  get password() {
    return this.f.controls.password;
  }

  get avatar() {
    return appSettings.service_url + appSettings.avatarPrefix + this.settings.user.avatar;
  }

  // protected AuthConfig(): AuthConfig {
  //   return new AuthConfig('Root.Site.Identity', ['Login', 'Jwtoken']);
  // }

  submit() {
    // tslint:disable-next-line:forin
    for (const i in this.f.controls) {
      this.f.controls[i].markAsDirty();
      this.f.controls[i].updateValueAndValidity();
    }
    if (this.f.valid) {
      this.canSubmit = false;
      this.dto.password = this.password.value;
      this._service
        .login(this.dto)
        .then(result => {
          if (result.type === AjaxResultType.success) {
            this.msgSrv.success('用户登录成功');
            setTimeout(() => {
              this.router.navigate(['/dashboard']);
            }, 50);
            return;
          }
          this.canSubmit = true;
          this.error = `登录失败：${result.content}`;
        })
        .catch(e => {
          this.canSubmit = true;
          this.error = `发生错误：${e.statusText}`;
        });
    }
  }
}
