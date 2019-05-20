import { Component, Injector, AfterViewInit } from '@angular/core';
import { SendMailDto, AdResult, AuthConfig, VerifyCode } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { OsharpService, ComponentBase } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { I18NService } from '@core/i18n/i18n.service';

@Component({
  selector: 'app-identity-forgot-password',
  templateUrl: `../shared/send-mail.html`,
})
export class ForgotPasswordComponent implements AfterViewInit {

  title: string;
  dto: SendMailDto = new SendMailDto();
  code: VerifyCode = new VerifyCode();
  result: AdResult = new AdResult();
  canSubmit = true;
  canSend = false;

  constructor(
    public router: Router,
    public osharp: OsharpService,
    private identity: IdentityService,
    private i18NService: I18NService,
    injector: Injector
  ) {
    // super(injector);
    // this.checkAuth().then(() => {
    //   this.canSend = this.auth.SendResetPasswordMail;
    // });
    this.canSend = true;
    this.title = this.i18NService.fanyi("app.account.security.send-reset-password-email");
  }

  ngAfterViewInit() {
    this.refreshVerifyCode();
  }

  // protected AuthConfig(): AuthConfig {
  //   return new AuthConfig('Root.Site.Identity', ['SendResetPasswordMail']);
  // }

  refreshVerifyCode() {
    this.osharp.refreshVerifyCode().subscribe(vc => {
      this.code = vc;
    });
  }

  submitForm() {
    this.dto.verifyCode = this.code.code;
    this.dto.verifyCodeId = this.code.id;
    this.canSubmit = false;
    this.identity.sendResetPasswordMail(this.dto).then(res => {
      res.show = true;
      this.result = res;
      this.canSubmit = true;
    });
  }
}
