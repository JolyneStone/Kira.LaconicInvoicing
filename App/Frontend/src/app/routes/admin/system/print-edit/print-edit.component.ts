import { PrintManagementService } from './../../../../shared/business/services/print.management.service';
import { AuthConfig, AjaxResultType } from './../../../../shared/osharp/osharp.model';
import { PrintTemplateDto, TemplateType } from './../../../../shared/business/app.model';
import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService, NzButtonComponent } from 'ng-zorro-antd';
import { I18NService } from '@core/i18n/i18n.service';
import { ActivatedRoute } from '@angular/router';
import { LodopService, Lodop } from '@delon/abc';

@Component({
  selector: 'app-print-edit',
  templateUrl: './print-edit.component.html',
  styleUrls: ['./print-edit.component.less'],
})
export class PrintEditComponent extends ComponentBase implements OnInit {

  constructor(
    private msg: NzMessageService,
    private i18nService: I18NService,
    private route: ActivatedRoute,
    private printManagementService: PrintManagementService,
    private lodopSrvice: LodopService,
    injector: Injector,
  ) {
    super(injector);
    this.printTemplateDto = new PrintTemplateDto();
    this.printTemplateDto.type = TemplateType.purchaseOrder;

    this.lodopSrvice.lodop.subscribe(({ lodop, ok }) => {
      if (!ok) {
        return;
      }
      this.msg.success(`打印机加载成功`);
      this.lodop = lodop;
    });
  }

  title: string;
  mode: 'Add' | 'Update' = 'Add';
  templateType: typeof TemplateType = TemplateType;
  typeKeys: any[];
  printTemplateDto: PrintTemplateDto;
  hasUpload = false;
  scripts: string[] = [];
  currentPage: number;
  isDisabled = false;
  lodop: Lodop = null;
  cog: any = {
    url: 'https://localhost:8443/CLodopfuncs.js',
    printer: '',
    paper: '',
    script: '',
  };
  serverUrl: string = 'https://localhost:8443/CLodopfuncs.js';
  @ViewChild('fileInput') fileInput: NzButtonComponent;

  ngOnInit() {
    this.typeKeys = Object.keys(this.templateType).filter(f => !isNaN(Number(f)));
    this.route.params.subscribe((params) => {
      if (params && params['id']) {
        this.mode = 'Update';
        this.title = this.i18nService.fanyi('app.account.print-edit');
        this.printManagementService.get(params['id'])
          .subscribe(res => {
            if (res && res.type === AjaxResultType.success) {
              this.printTemplateDto = res.data;
              this.scripts = this.printTemplateDto.script.split('Kira.LaconicInvoicingScript');
            } else if (res && res.content) {
              this.msg.error(res.content);
            }
          });
      } else {
        this.mode = 'Add';
        this.title = this.i18nService.fanyi('app.account.print-add');
        this.printTemplateDto = new PrintTemplateDto();
        this.printTemplateDto.type = TemplateType.purchaseOrder;
      }

      super.checkAuth();
    });

  }

  protected AuthConfig(): AuthConfig {
    const authConfig = new AuthConfig('Root.Print.Print.PrintManagement', []);
    if (this.mode === 'Add') {
      authConfig.funcs.push(this.mode, 'Upload', 'GetPrintTemplateScript');
    } else {
      authConfig.funcs.push(this.mode, 'Upload', 'GetPrintTemplateScript');
    }

    return authConfig;
  }

  clickEdit() {
    if (this.hasUpload) {
      this.hasUpload = false;
      if (this.printTemplateDto.script) {
        this.scripts = this.printTemplateDto.script.split('Kira.LaconicInvoicingScript');
      } else {
        this.currentPage = 0;
        if (!this.scripts) {
          this.scripts = [];
        }
      }
    } else {
      this.hasUpload = true;
    }
  }

  submit() {
    if (this.scripts) {
      this.printTemplateDto.script = this.scripts.join('Kira.LaconicInvoicingScript');
    }
    const files = (this.fileInput.el as any).files;
    if (this.mode === 'Add') {
      if (files.length > 0) {
        const reader = new FileReader(); // 新建一个FileReader
        reader.readAsText(files[0], 'UTF-8'); // 读取文件
        reader.onload = (evt) => {
          this.printTemplateDto.script = (evt.target as any).result; // 读取文件内容
          this.printManagementService.add(this.printTemplateDto)
            .subscribe(res => {
              if (res && res.type === AjaxResultType.success) {
                this.msg.success('添加成功');
              } else if (res && res.content) {
                this.msg.error(res.content);
              }
            });
        }
      } else {
        this.printManagementService.add(this.printTemplateDto)
          .subscribe(res => {
            if (res && res.type === AjaxResultType.success) {
              this.msg.success('添加成功');
            } else if (res && res.content) {
              this.msg.error(res.content);
            }
          });
      }
    } else {
      if (files.length > 0) {
        const reader = new FileReader(); // 新建一个FileReader
        reader.readAsText(files[0], 'UTF-8'); // 读取文件
        reader.onload = (evt) => {
          this.printTemplateDto.script = (evt.target as any).result; // 读取文件内容
          this.printManagementService.update(this.printTemplateDto)
            .subscribe(res => {
              if (res && res.type === AjaxResultType.success) {
                this.msg.success('更新成功');
              } else if (res && res.content) {
                this.msg.error(res.content);
              }
            });
        };
      } else {
        this.printManagementService.update(this.printTemplateDto)
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

  reload(options: any = { url: 'https://localhost:8443/CLodopfuncs.js' }) {
    this.cog.printer = '';
    this.cog.paper = '';

    this.lodopSrvice.cog = Object.assign({}, this.cog, options);
    if (options === null) this.lodopSrvice.reset();
  }

  addScript() {
    this.currentPage = this.scripts.length;
    this.scripts.push('');
    this.isDisabled = true;
    const LODOP = this.lodop;
    LODOP.PRINT_INITA(10, 10, 1180, 850, '打印设计');
    LODOP.On_Return = (TaskID, Value) => {
      this.scripts[this.currentPage] = Value.toString();
      this.isDisabled = false;
    };
    LODOP.NEWPAGEA();
    LODOP.PRINT_DESIGN();
  }

  insertScript() {
    const firstScripts = this.scripts.slice(0, this.currentPage);
    const tempScripts = this.scripts.slice(this.currentPage);
    firstScripts.push('');
    this.scripts = firstScripts.concat(tempScripts);
    this.currentPage += 1;
    this.isDisabled = true;
    const LODOP = this.lodop;
    LODOP.PRINT_INITA(10, 10, 1180, 850, '打印设计');
    LODOP.On_Return = (TaskID, Value) => {
      this.scripts[this.currentPage] = Value.toString();
      this.isDisabled = false;
    };
    LODOP.NEWPAGEA();
    LODOP.PRINT_DESIGN();
  }

  editScript() {
    if (this.currentPage < 0)
      return;
    const script = this.replaceScript(this.scripts[this.currentPage]);
    this.isDisabled = true;
    const LODOP = this.lodop;
    LODOP.PRINT_INITA(10, 20, 810, 610, '打印设计');
    if (script)
      eval(script);
    LODOP.On_Return = (TaskID, Value) => {
      this.scripts[this.currentPage] = this.replaceScript(Value.toString());
      this.isDisabled = false;
    };
    LODOP.PRINT_DESIGN();
  }

  deleteScript() {
    this.scripts = this.scripts.filter((v, i) => i !== this.currentPage);
  }

  replaceScript(script: string) {
    return script.replace(new RegExp('LODOP.PRINT_INITA\\(.*\\);', 'g'), '')
      .replace(new RegExp('LODOP.NEWPAGE\\(\\);', 'g'), '')
      .replace(new RegExp('LODOP.NEWPAGEA\\(\\);', 'g'), '')
      .replace(new RegExp('LODOP.ADD_PRINT_SETUP_BKIMG\\("<.*>"\\);', 'g'), '')
      .replace(new RegExp('LODOP.SET_SHOW_MODE\\("BKIMG_WIDTH".*?\\)', 'g'), '')
      .replace(new RegExp('LODOP.SET_SHOW_MODE\\("BKIMG_IN_PREVIEW".*?\\)', 'g'), '')
      .replace(new RegExp('LODOP.SET_SHOW_MODE\\("BKIMG_HEIGHT".*?\\)', 'g'), '');
  }
}
