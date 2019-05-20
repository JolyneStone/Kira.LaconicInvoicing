import { Component, Injector } from '@angular/core';
import { IdentityService, } from '@shared/osharp/services/identity.service';
import { LoginDto, AjaxResultType, AuthConfig } from '@shared/osharp/osharp.model';
import { NzMessageService } from 'ng-zorro-antd';
import { Router } from '@angular/router';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { SocialOpenType } from '@delon/auth';

@Component({
  selector: 'app-identity-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less']
})
export class LoginComponent {//extends ComponentBase {

  dto: LoginDto = new LoginDto();
  canSubmit = true;
  resendConfirmMail = false;
  error: string;

  constructor(
    private msgSrv: NzMessageService,
    private _service: IdentityService,
    private router: Router,
    injector: Injector
  ) {
    //super(injector);
    //super.checkAuth();
  }

  // protected AuthConfig(): AuthConfig {
  //   return new AuthConfig('Root.Site.Identity', ["Login", "Jwtoken", "Register", "SendResetPasswordMail", "SendConfirmMail"]);
  // }

  submitForm() {
    this.canSubmit = false;
    this._service.login(this.dto).then(result => {
      if (result.type === AjaxResultType.success) {
        this.msgSrv.success('用户登录成功');
        setTimeout(() => {
          this.router.navigate(['dashboard']);
        }, 50);
        return;
      }
      this.canSubmit = true;
      this.error = `登录失败：${result.content}`;
      this.resendConfirmMail = result.content.indexOf("邮箱未验证") > -1;
    }).catch(e => {
      this.canSubmit = true;
      this.error = `发生错误：${e.statusText}`;
    });
  }

  // open(type: string, openType: SocialOpenType = 'href') {
  //   let callback = `/#/callback/${type}`;
  //   let url = `api/identity/OAuth2?provider=${type}&returnUrl=${this.osharp.urlEncode(callback)}`;

  //   switch (type) {
  //     case 'QQ':
  //       url = 'api/identity/OAuth2?provider=';
  //       break;
  //     case 'auth0':
  //       url = `//cipchk.auth0.com/login?client=8gcNydIDzGBYxzqV0Vm1CX_RXH-wsWo5&redirect_uri=${decodeURIComponent(
  //         callback,
  //       )}`;
  //       break;
  //     case 'github':
  //       url = `//github.com/login/oauth/authorize?client_id=9d6baae4b04a23fcafa2&response_type=code&redirect_uri=${decodeURIComponent(
  //         callback,
  //       )}`;
  //       break;
  //     case 'weibo':
  //       url = `https://api.weibo.com/oauth2/authorize?client_id=1239507802&response_type=code&redirect_uri=${decodeURIComponent(
  //         callback,
  //       )}`;
  //       break;
  //   }
  //   if (openType === 'window') {
  //     this.socialService
  //       .login(url, '/', {
  //         type: 'window',
  //       })
  //       .subscribe(res => {
  //         if (res) {
  //           this.settingsService.setUser(res);
  //           this.router.navigateByUrl('/');
  //         }
  //       });
  //   } else {
  //     this.socialService.login(url, '/', {
  //       type: 'href',
  //     });
  //   }
  // }
}
