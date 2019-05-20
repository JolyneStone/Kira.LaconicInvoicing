import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService, NzButtonComponent } from 'ng-zorro-antd';
import { I18NService } from '@core/i18n/i18n.service';
import { ActivatedRoute } from '@angular/router';
import { DocumentManagementService } from '@shared/business/services/document.management.service';
import { LodopService } from '@delon/abc';
import { TemplateType, FileTemplateDto } from '@shared/business/app.model';
import { AjaxResultType, AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-document-edit',
  templateUrl: './document-edit.component.html',
  styleUrls: ['./document-edit.component.less'],
})
export class DocumentEditComponent extends ComponentBase implements OnInit {

  constructor(
    private msg: NzMessageService,
    private i18nService: I18NService,
    private route: ActivatedRoute,
    private documentManagementService: DocumentManagementService,
    injector: Injector,
  ) {
    super(injector);
    this.fileTemplateDto = new FileTemplateDto();
    this.fileTemplateDto.type = TemplateType.purchaseOrder;
  }

  title: string;
  mode: 'Add' | 'Update' = 'Add';
  templateType: typeof TemplateType = TemplateType;
  typeKeys: any[];
  fileTemplateDto: FileTemplateDto;
  hasUpload = false;
  @ViewChild('fileInput') fileInput: NzButtonComponent;

  ngOnInit() {
    this.typeKeys = Object.keys(this.templateType).filter(f => !isNaN(Number(f)));
    this.route.params.subscribe((params) => {
      if (params && params['id']) {
        this.mode = 'Update';
        this.title = this.i18nService.fanyi('app.account.document-edit');
        this.documentManagementService.get(params['id'])
          .subscribe(res => {
            if (res && res.type === AjaxResultType.success) {
              this.fileTemplateDto = res.data;
            } else if (res && res.content) {
              this.msg.error(res.content);
            }
          });
      } else {
        this.mode = 'Add';
        this.title = this.i18nService.fanyi('app.account.document-add');
        this.fileTemplateDto = new FileTemplateDto();
        this.fileTemplateDto.type = TemplateType.purchaseOrder;
      }

      super.checkAuth();
    });

  }

  protected AuthConfig(): AuthConfig {
    const authConfig = new AuthConfig('Root.File.File.FileManagement', []);
    if (this.mode === 'Add') {
      authConfig.funcs.push(this.mode, 'Upload', 'UploadTemplate');
    } else {
      authConfig.funcs.push(this.mode, 'Upload');
    }

    return authConfig;
  }

  submit() {
    const files = (this.fileInput.el as any).files;
    if (this.mode === 'Add' && files.length > 0) {
      const file = new FormData();
      file.append('file', files[0]);
      this.documentManagementService.uploadTemplate(file)
        .subscribe(res => {
          if (res && res.type === AjaxResultType.success) {
            this.fileTemplateDto.path = res.data;
            this.documentManagementService.add(this.fileTemplateDto)
              .subscribe(res1 => {
                if (res1 && res1.type === AjaxResultType.success) {
                  this.msg.success('添加成功');
                } else if (res1 && res1.content) {
                  this.msg.error(res1.content);
                }
              });
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    } else if (this.mode === 'Update') {
      if (files.length > 0) {
        const file = new FormData();
        file.append('file', files[0]);
        this.documentManagementService.uploadTemplate(file)
          .subscribe(res => {
            if (res && res.type === AjaxResultType.success) {
              this.fileTemplateDto.path = res.data;
              this.documentManagementService.add(this.fileTemplateDto)
                .subscribe(res1 => {
                  if (res1 && res1.type === AjaxResultType.success) {
                    this.msg.success('编辑成功');
                  } else if (res1 && res1.content) {
                    this.msg.error(res1.content);
                  }
                });
            } else if (res && res.content) {
              this.msg.error(res.content);
            }
          });
      } else {
        this.documentManagementService.update(this.fileTemplateDto)
          .subscribe(res => {
            if (res && res.type === AjaxResultType.success) {
              this.msg.success('更新成功');
            } else if (res && res.content) {
              this.msg.error(res.content);
            }
          });
      }
    }
  }
}
