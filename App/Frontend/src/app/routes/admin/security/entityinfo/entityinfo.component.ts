import { Component, OnInit, OnDestroy, AfterViewInit, Injector, } from '@angular/core';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'admin-security-entityinfo',
  template: `<div id="grid-box-{{moduleName}}"></div>`
})
export class EntityinfoComponent extends GridComponentBase implements AfterViewInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = "entityinfo";
  }

  async ngAfterViewInit() {
    await this.checkAuth();
    if (this.auth.Read) {
      super.InitBase();
      super.ViewInitBase();
    }
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Admin.Security.EntityInfo", ["Read", "Update"]);
  }

  protected GetModel() {
    return {
      id: "id",
      fields: {
        name: { type: "string", editable: false },
        typeName: { type: "string", editable: false },
        auditEnabled: { type: "boolean" },
        updatable: { type: "boolean", editable: false },
      }
    };
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      { field: "name", title: "实体名称", width: 150, filterable: this.osharp.data.stringFilterable },
      { field: "typeName", title: "实体类型", width: 250, filterable: this.osharp.data.stringFilterable },
      {
        field: "auditEnabled", title: "数据审计", width: 95,
        template: d => this.kendoui.Boolean(d.auditEnabled),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }
    ];
  }
}
