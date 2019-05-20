import { Component, OnInit, } from '@angular/core';
import { ConfirmEmailDto, AjaxResult, AjaxResultType, AdResult } from '@shared/osharp/osharp.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { _HttpClient } from '@delon/theme';

@Component({
  selector: 'app-identity-confirm-email',
  template: `
  <result type="{{result.type}}" [title]="result.title" description="{{result.description}}">
    <button nz-button [nzType]="'primary'" (click)="router.navigate(['identity/login'])">{{ 'app.login.login' | translate }}</button>
    <button nz-button (click)="router.navigate(['dashboard'])">{{ 'app.back-home' | translate }}</button>
  </result>
  `,
})
export class ConfirmEmailComponent implements OnInit {

  dto: ConfirmEmailDto = new ConfirmEmailDto();
  result: AdResult = new AdResult();

  constructor(
    private http: _HttpClient,
    public router: Router,
    private osharp: OsharpService,
    private identity: IdentityService
  ) { }

  ngOnInit(): void {
    this.getUrlParams();
    this.result.title = '正在激活邮箱……';
    this.confirmEmail();
  }

  private getUrlParams() {
    const url = window.location.hash;
    this.dto.userId = this.osharp.getHashURLSearchParams(url, 'userId');
    this.dto.code = this.osharp.getHashURLSearchParams(url, 'code');
  }

  private confirmEmail() {
    this.identity.confirmEmail(this.dto).then(res => {
      this.result = res;
    });
  }
}
