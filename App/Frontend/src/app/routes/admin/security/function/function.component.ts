import { Component, AfterViewInit, Injector, } from '@angular/core';

import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'admin-security-function',
  template: `<div id="grid-box-{{moduleName}}"></div>`
})

export class FunctionComponent extends GridComponentBase implements AfterViewInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = "function";
  }

  async ngAfterViewInit() {
    await this.checkAuth();
    if (this.auth.Read) {
      super.InitBase();
      super.ViewInitBase();
    } else {
      this.osharp.error("无权查看此页面");
    }
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Admin.Security.Function", ["Read", "Update"]);
  }

  protected GetModel() {
    return {
      id: "id",
      fields: {
        id: { type: "string", editable: false },
        name: { type: "string", editable: false },
        accessType: { type: "number" },
        cacheExpirationSeconds: { type: "number" },
        auditOperationEnabled: { type: "boolean" },
        auditEntityEnabled: { type: "boolean" },
        isCacheSliding: { type: "boolean" },
        isController: { type: "boolean", editable: false },
        isLocked: { type: "boolean" },
        isAjax: { type: "boolean", editable: false },
        area: { type: "string", editable: false },
        controller: { type: "string", editable: false },
        action: { type: "string", editable: false },
        updatable: { type: "boolean", editable: false },
      }
    };
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      {
        field: "id", title: "编号", width: 200, hidden: true,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "name", title: "功能名称", width: 200,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "accessType", title: "功能类型", width: 95,
        template: d => this.osharp.valueToText(d.accessType, this.osharp.data.accessType),
        editor: (container, options) => this.kendoui.DropDownListEditor(container, options, this.osharp.data.accessType),
        filterable: { ui: element => this.kendoui.DropDownList(element, this.osharp.data.accessType) }
      }, { field: "cacheExpirationSeconds", title: "缓存秒数", width: 95 },
      {
        field: "auditOperationEnabled", title: "操作审计", width: 95,
        template: d => this.kendoui.Boolean(d.auditOperationEnabled),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "auditEntityEnabled", title: "数据审计", width: 95,
        template: d => this.kendoui.Boolean(d.auditEntityEnabled),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "isCacheSliding", title: "滑动过期", width: 95,
        template: d => this.kendoui.Boolean(d.isCacheSliding),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "isLocked", title: "已锁定", width: 95,
        template: d => this.kendoui.Boolean(d.isLocked),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "isAjax", title: "Ajax访问", width: 95,
        template: d => this.kendoui.Boolean(d.isAjax),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "isController", title: "是否控制器", width: 95,
        template: d => this.kendoui.Boolean(d.isController),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "area", title: "区域", width: 100, hidden: true,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "controller", title: "控制器", width: 100, hidden: true,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "action", title: "功能方法", width: 120, hidden: true,
        filterable: this.osharp.data.stringFilterable
      }
    ];
  }

  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    let options = super.GetDataSourceOptions();
    options.group = [{ field: "area" }, { field: "controller" }];
    options.pageSize = 10;
    return options;
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    let options = super.GetGridOptions(dataSource);
    options.columnMenu = { sortable: false };
    return options;
  }
}
