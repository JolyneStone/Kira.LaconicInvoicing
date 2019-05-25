import { appSettings } from './../../../../app.settings';
import { FileTemplateDto } from './../../../../shared/business/app.model';
import { Component, OnInit, Injector } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { DocumentService } from '@shared/business/services/document.service';
import { LodopService, Lodop } from '@delon/abc';
import { NzMessageService } from 'ng-zorro-antd';
import { ActivatedRoute } from '@angular/router';
import { TemplateType, PrintTemplateDto } from '@shared/business/app.model';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-document-window',
  templateUrl: './document-window.component.html',
  styles: []
})
export class DocumentWindowComponent extends ComponentBase implements OnInit {
  constructor(
    private documentService: DocumentService,
    private msg: NzMessageService,
    private route: ActivatedRoute,
    injector: Injector,
  ) {
    super(injector);
    this.templates = [];

    this.route.queryParams.subscribe(queryParams => {
      this.id = queryParams.id as string;
      this.type = queryParams.type as TemplateType;
      this.documentService.getAllByType(this.type)
        .subscribe(res => {
          if (res && res.data) {
            this.templates = res.data;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    });
  }

  id: string;
  type: TemplateType;
  data: any;
  selectTemplate: FileTemplateDto;
  templates: FileTemplateDto[];

  async ngOnInit() {
    await this.checkAuth();
  }

  AuthConfig() {
    return new AuthConfig('Root.File.File.File', [
      'GetAllByType',
      'ExportDocument',
    ]);
  }

  export() {
    if (!this.selectTemplate) {
      this.msg.warning('请先选择模板');
      return;
    }

    this.documentService.export(this.id, this.selectTemplate.id, this.type)
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          window.open(`${appSettings.service_url}api/file/file/download/?name=${res.data.destName}&filename=${res.data.fileName}`);
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
      });
  }
}
