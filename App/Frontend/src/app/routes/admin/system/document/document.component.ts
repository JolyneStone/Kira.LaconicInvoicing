import { DocumentManagementService } from './../../../../shared/business/services/document.management.service';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { Component, OnInit, Injector } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';
import { Router } from '@angular/router';
import { PrintManagementService } from '@shared/business/services/print.management.service';
import { appSettings } from 'app/app.settings';
import { FileTemplateDto, TemplateType } from '@shared/business/app.model';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-document',
  templateUrl: './document.component.html',
  styles: []
})
export class DocumentComponent extends ComponentBase implements OnInit {

  constructor(
    private http: _HttpClient,
    private msr: NzMessageService,
    private router: Router,
    private documentManagementService: DocumentManagementService,
    injector: Injector
  ) {
    super(injector);
    this.appSettings = appSettings;
  }

  appSettings: any;
  fileTemplateDtoList: FileTemplateDto[];
  loading = false;
  templateType: typeof TemplateType = TemplateType;

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.File.File.FileManagement', [
      'Download',
      'Add',
      'Update',
      'Delete',
      'GetAll',
    ]);
  }

  async ngOnInit() {
    this.fileTemplateDtoList = [];
    await super.checkAuth();
    this.getAll();
  }

  getAll() {
    this.documentManagementService.getAll()
      .subscribe(res => {
        if (res.type === AjaxResultType.success) {
          this.fileTemplateDtoList = res.data;
        } else if (res.content) {
          this.msr.error(res.content);
        }
      });
  }

  refresh() {
    this.getAll();
  }

  add() {
    this.router.navigateByUrl('/admin/system/document-add');
  }

  edit(id: string) {
    this.router.navigate(['admin', 'system', 'document-edit', id]);
  }

  delete(id: string) {
    this.documentManagementService.delete(id)
      .subscribe((res: any) => {
        if (res.type === AjaxResultType.success) {
          this.msr.success('删除成功');
        } else if (res && res.content) {
          this.msr.error(res.content);
        }
      });
  }
}
