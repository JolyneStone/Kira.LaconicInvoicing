import { Component, OnInit, Injector, AfterViewInit } from '@angular/core';
import { ResetEmailDto } from '@shared/business/app.model';
import { VerifyCode, AdResult, AuthConfig, SendMailDto } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { OsharpService, ComponentBase } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { I18NService } from '@core/i18n/i18n.service';

@Component({
  selector: 'app-identity-reset-email',
  templateUrl: `./reset-email.component.html`
})
export class ResetEmailComponent extends ComponentBase implements AfterViewInit {
  title: string;
  dto: ResetEmailDto = new ResetEmailDto();
  code: VerifyCode = new VerifyCode();
  result: AdResult = new AdResult();
  canSubmit = true;
  canSend = false;

  constructor(
    public router: Router,
    public osharp: OsharpService,
    private identity: IdentityService,
    private i18nService: I18NService,
    injector: Injector
  ) {
    super(injector);
    this.checkAuth().then(() => {
      this.canSend = this.auth.SendResetMail;
    });
    this.title = this.i18nService.fanyi('app.account.identity.reset-email');
  }

  ngAfterViewInit() {
    this.refreshVerifyCode();
  }

  protected AuthConfig() {
    return new AuthConfig("Root.Site.Identity", ["SendResetMail"]);
  }

  refreshVerifyCode() {
    this.osharp.refreshVerifyCode().subscribe(vc => {
      this.code = vc;
    });
  }

  submitForm() {
    this.dto.verifyCode = this.code.code;
    this.dto.verifyCodeId = this.code.id;
    this.canSubmit = false;
    this.identity.sendResetMail(this.dto).then(res => {
      res.show = true;
      this.result = res;
      this.canSubmit = true;
    });
  }
}
