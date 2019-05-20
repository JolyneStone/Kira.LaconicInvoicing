import { ActivatedRoute } from '@angular/router';
import { PrintService } from '../../../../shared/business/services/print.service';
import { PrintTemplateDto, TemplateType } from '../../../../shared/business/app.model';
import { Component, OnInit, Injector, Input } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzModalRef, NzMessageService } from 'ng-zorro-antd';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';
import { LodopService, Lodop } from '@delon/abc';

@Component({
  selector: 'app-print-window',
  templateUrl: './print-window.component.html',
  styles: []
})
export class PrintWindowComponent extends ComponentBase implements OnInit {
  constructor(
    private printService: PrintService,
    private lodopSrv: LodopService,
    private msg: NzMessageService,
    private route: ActivatedRoute,
    injector: Injector,
  ) {
    super(injector);
    this.templates = [];
    this.lodopSrv.lodop.subscribe(({ lodop, ok }) => {
      if (!ok) {
        this.error = true;
        return;
      }
      this.msg.success(`打印机加载成功`);
      this.lodop = lodop;
      this.pinters = this.lodopSrv.printer;
    });

    this.route.queryParams.subscribe(queryParams => {
      const id = queryParams.id as string;
      const type = queryParams.type as TemplateType;
      this.printService.getPrintData(id, type)
        .subscribe(res => {
          if (res && res.data) {
            this.data = res.data;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
      this.printService.getAllByType(type)
        .subscribe(res => {
          if (res && res.data) {
            this.templates = res.data;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    });
  }

  data: any;
  cog: any = {
    url: 'https://localhost:8443/CLodopfuncs.js',
    printer: '',
    paper: '',
  };

  error = false;
  lodop: Lodop = null;
  pinters: any[] = [];
  papers: string[] = [];
  selectTemplate: PrintTemplateDto;
  templates: PrintTemplateDto[];

  async ngOnInit() {
    await this.checkAuth();
  }

  AuthConfig() {
    return new AuthConfig('Root.Print.Print.Print', [
      'GetAllByType',
      'GetPrintTemplateScript',
    ]);
  }

  reload(options: any = { url: 'https://localhost:8443/CLodopfuncs.js' }) {
    this.pinters = [];
    this.papers = [];
    this.cog.printer = '';
    this.cog.paper = '';

    this.lodopSrv.cog = Object.assign({}, this.cog, options);
    this.error = false;
    if (options === null) this.lodopSrv.reset();
    this.pinters = this.lodopSrv.printer;
  }

  changePinter(name: string) {
    this.papers = this.lodop.GET_PAGESIZES_LIST(name, '\n').split('\n');
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

  print() {
    if (!this.selectTemplate) {
      this.msg.warning('请先选择模板');
      return;
    }
    if (this.selectTemplate.script) {
      this.useLodop(false);
    } else {
      this.printService.getPrintTemplateScript(this.selectTemplate.path)
        .subscribe(res => {
          if (res && res.type === AjaxResultType.success) {
            this.selectTemplate.script = this.replaceScript(res.data);
            this.useLodop(false);
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }

  preview() {
    if (!this.selectTemplate) {
      this.msg.warning('请先选择模板');
      return;
    }
    if (this.selectTemplate.script) {
      this.useLodop(true);
    } else {
      this.printService.getPrintTemplateScript(this.selectTemplate.path)
        .subscribe(res => {
          if (res && res.type === AjaxResultType.success) {
            this.selectTemplate.script = this.replaceScript(res.data);
            this.useLodop(true);
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }

  useLodop(isPreview: boolean) {
    const LODOP = this.lodop;
    //LODOP.PRINT_INITA(10, 10, 1180, 850, '打印设计');
    LODOP.PRINT_INITA(10, 20, 810, 610, '打印设计');
    LODOP.SET_PRINTER_INDEXA(this.cog.printer);
    LODOP.SET_PRINT_PAGESIZE(0, 0, 0, this.cog.paper);
    LODOP.NEWPAGEA();
    if (this.selectTemplate.script) {
      const script = this.selectTemplate.script
                      .replace(/"{/g, '`{')
                      .replace(/}"/g, '}`')
                      .replace(/Kira.LaconicInvoicingScript/g, 'LODOP.NEWPAGEA();');
      const data = this.data;
      eval(script);
    }
    if (isPreview) {
      LODOP.PREVIEW();
    } else {
      LODOP.PRINT();
    }
  }
}
