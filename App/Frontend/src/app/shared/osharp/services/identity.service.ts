import { Injectable, Inject } from '@angular/core';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { HttpClient } from '@angular/common/http';
import { SettingsService } from '@delon/theme';
import { ACLService } from '@delon/acl';
import {
  LoginDto,
  AjaxResult,
  AjaxResultType,
  RegisterDto,
  ConfirmEmailDto,
  User,
  SendMailDto,
  AdResult,
  ResetPasswordDto,
} from '@shared/osharp/osharp.model';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { Observable } from '../../../../../node_modules/rxjs';
import { map } from 'rxjs/operators';
import { ResetEmailDto } from '@shared/business/app.model';

@Injectable()
export class IdentityService {
  constructor(
    private http: HttpClient,
    private osharp: OsharpService,
    @Inject(DA_SERVICE_TOKEN) private tokenSrv: ITokenService,
    private settingSrv: SettingsService,
    private aclSrv: ACLService,
  ) {}

  login(dto: LoginDto): Promise<AjaxResult> {
    // this.tokenSrv.set({
    //   token: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI2IiwidW5pcXVlX25hbWUiOiJBZG1pbiIsIm5iZiI6MTU1NDAzOTU4MywiZXhwIjoxNTU0NjQ0MzgzLCJpYXQiOjE1NTQwMzk1ODMsImlzcyI6IktpcmEuTGFjb25pY0ludm9pY2luZyBzZXJ2ZXIiLCJhdWQiOiJLaXJhLkxhY29uaWNJbnZvaWNpbmcgY2xpZW50In0.vlFCSpgyTGn0gdvQSlve2foDiIX931OIN5yjZeV9CF8'
    // });
    const url = 'api/identity/jwtoken';
    return this.http
      .post<AjaxResult>(url, dto)
      .pipe(
        map(result => {
          if (result.type === AjaxResultType.success) {
            // 设置Token
            this.tokenSrv.set({ token: result.data });
            // 刷新用户信息
            this.refreshUser().subscribe();
          }
          return result;
        }),
      )
      .toPromise();
  }

  logout() {
    const url = 'api/identity/logout';
    return this.http
      .post<AjaxResult>(url, {})
      .pipe(
        map(res => {
          if (res.type === AjaxResultType.success) {
            // 清除Token
            this.tokenSrv.clear();
            this.settingSrv.setUser({});
            // 刷新用户信息
            this.refreshUser().subscribe();
          }
          return res;
        }),
      )
      .toPromise();
  }

  register(dto: RegisterDto): Promise<AdResult> {
    let url = 'api/identity/register';
    return this.http
      .post<AjaxResult>(url, dto)
      .pipe(
        map(res => {
          let result = new AdResult();
          if (res.type == AjaxResultType.success) {
            result.type = 'success';
            result.title = '新用户注册成功';
            result.description = `你的账户：${dto.userName}[${
              dto.nickName
            }] 注册成功，请及时登录邮箱 ${dto.email} 接收邮件激活账户。`;
            return result;
          }
          result.type = 'error';
          result.title = '用户注册失败';
          result.description = res.content;
          return result;
        }),
      )
      .toPromise();
  }

  /** 刷新用户信息 */
  refreshUser(): Observable<User> {
    let url = 'api/identity/profile';
    return this.http.get(url).pipe(
      map((res: any) => {
        if (!res || res == {}) {
          this.settingSrv.setUser({});
          this.aclSrv.setRole([]);
          // 更新权限
          this.osharp.refreshAuthInfo();
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
        };

        this.settingSrv.setUser(user);
        // 更新角色
        this.aclSrv.setRole(user.roles);
        // 更新权限
        this.osharp.refreshAuthInfo();
        return user;
      }),
    );
  }

  sendConfirmMail(dto: SendMailDto): Promise<AdResult> {
    let url = 'api/identity/SendConfirmMail';
    return this.http
      .post<AjaxResult>(url, dto)
      .pipe(
        map(res => {
          let result = new AdResult();
          if (res.type != AjaxResultType.success) {
            result.type = 'error';
            result.title = '重发激活邮件失败';
            result.description = res.content;
            return result;
          }
          result.type = 'success';
          result.title = '重发激活邮件成功';
          result.description = `邮箱激活邮件发送成功，请登录邮箱“${
            dto.email
          }”收取邮件进行后续步骤`;
          return result;
        }),
      )
      .toPromise();
  }

  sendResetMail(dto: ResetEmailDto): Promise<AdResult> {
    let url = 'api/identity/SendResetMail';
    return this.http
      .post<AjaxResult>(url, dto)
      .pipe(
        map(res => {
          let result = new AdResult();
          if (res.type != AjaxResultType.success) {
            result.type = 'error';
            result.title = '发送重置邮件失败';
            result.description = res.content;
            return result;
          }
          result.type = 'success';
          result.title = '发送重置邮件成功';
          result.description = `重置邮件发送成功，请登录邮箱“${
            dto.newEmail
          }”收取邮件进行后续步骤`;
          return result;
        }),
      )
      .toPromise();
  }

  confirmEmail(dto: ConfirmEmailDto): Promise<AdResult> {
    
    let url = 'api/identity/ConfirmEmail';
    return this.http
      .post<AjaxResult>(url, dto)
      .pipe(
        map(res => {
          let result = new AdResult();
          if (res.type != AjaxResultType.success) {
            result.type = 'error';
            result.title = '邮箱激活失败';
            if (res.type == AjaxResultType.info) {
              result.type = 'minus-circle-o';
            }
            result.title = '邮箱激活取消';
            result.description = res.content;
            return result;
          }
          result.type = 'success';
          result.title = '邮箱激活成功';
          result.description = res.content;
          return result;
        }),
      )
      .toPromise();
  }

  sendResetPasswordMail(dto: SendMailDto): Promise<AdResult> {
    let url = 'api/identity/SendResetPasswordMail';
    return this.http
      .post<AjaxResult>(url, dto)
      .pipe(
        map(res => {
          let result = new AdResult();
          if (res.type != AjaxResultType.success) {
            result.type = 'error';
            result.title = '重置密码邮件发送失败';
            result.description = res.content;
            return result;
          }
          result.type = 'success';
          result.title = '重置密码邮件发送成功';
          result.description = `重置密码邮件发送成功，请登录邮箱“${
            dto.email
          }”收取邮件进行后续步骤`;
          return result;
        }),
      )
      .toPromise();
  }

  resetPassword(dto: ResetPasswordDto): Promise<AdResult> {
    const url = 'api/identity/ResetPassword';
    return this.http
      .post<AjaxResult>(url, dto)
      .pipe(
        map(res => {
          let result = new AdResult();
          if (res.type != AjaxResultType.success) {
            result.type = 'error';
            result.title = '登录密码重置失败';
            result.description = res.content;
            return result;
          }
          result.type = 'success';
          result.title = '登录密码重置成功';
          result.description = '登录密码重置成功，请使用新密码登录系统。';
          return result;
        }),
      )
      .toPromise();
  }
}
