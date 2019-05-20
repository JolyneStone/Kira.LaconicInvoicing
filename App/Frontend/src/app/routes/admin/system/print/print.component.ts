import { PrintManagementService } from './../../../../shared/business/services/print.management.service';
import { AjaxResultType } from './../../../../shared/osharp/osharp.model';
import { AuthConfig } from '@shared/osharp/osharp.model';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { Component, OnInit, Injector } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';
import { TemplateType, PrintTemplateDto } from '@shared/business/app.model';
import { Router } from '@angular/router';
import { appSettings } from '../../../../app.settings';

@Component({
  selector: 'app-print',
  templateUrl: './print.component.html',
  styles: []
})
export class PrintComponent extends ComponentBase implements OnInit {

  constructor(
    private http: _HttpClient,
    private msr: NzMessageService,
    private router: Router,
    private printManagementService: PrintManagementService,
    injector: Injector
  ) {
    super(injector);
    this.appSettings = appSettings;
  }

  appSettings: any;
  printTemplateDtoList: PrintTemplateDto[];
  loading = false;
  templateType: typeof TemplateType = TemplateType;

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.Print.Print.PrintManagement', [
      'Download',
      'Add',
      'Update',
      'Delete',
      'GetAll',
    ]);
  }

  async ngOnInit() {
    this.printTemplateDtoList = [];
    await super.checkAuth();
    this.getAll();
  }

  getAll() {
    this.printManagementService.getAll()
      .subscribe(res => {
        if (res.type === AjaxResultType.success) {
          this.printTemplateDtoList = res.data;
        } else if (res.content) {
          this.msr.error(res.content);
        }
      });
  }

  refresh() {
    this.getAll();
  }

  add() {
    this.router.navigateByUrl('/admin/system/print-add');
  }

  edit(id: string) {
    this.router.navigate(['admin', 'system', 'print-edit', id]);
  }

  delete(id: string) {
    this.printManagementService.delete(id)
      .subscribe((res: any) => {
        if (res.type === AjaxResultType.success) {
          this.msr.success('删除成功');
        } else if (res && res.content) {
          this.msr.error(res.content);
        }
      });
  }
}
