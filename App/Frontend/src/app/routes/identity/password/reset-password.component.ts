import { Component, Injector } from '@angular/core';
import { ResetPasswordDto, AdResult, AuthConfig } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { OsharpService } from '@shared/osharp/services/osharp.service';

@Component({
  selector: 'app-identity-reset-password',
  templateUrl: `./reset-password.component.html`
})
export class ResetPasswordComponent {

  dto: ResetPasswordDto = new ResetPasswordDto();
  result: AdResult = new AdResult();
  canSubmit = true;
  private osharp: OsharpService;

  constructor(
    public router: Router,
    private identity: IdentityService,
    injector: Injector
  ) {
    // super(injector);
    // this.checkAuth();
    this.osharp = injector.get(OsharpService);
    this.getUrlParams();
  }

  // protected AuthConfig(): AuthConfig {
  //   return new AuthConfig("Root.Site.Identity", ["ResetPassword"]);
  // }

  private getUrlParams() {
    const url = window.location.hash;
    this.dto.userId = this.osharp.getHashURLSearchParams(url, "userId");
    this.dto.token = this.osharp.getHashURLSearchParams(url, "token");
  }

  submitForm() {
    this.canSubmit = false;
    this.identity.resetPassword(this.dto).then(res => {
      res.show = true;
      this.result = res;
      this.canSubmit = true;
    });
  }
}
