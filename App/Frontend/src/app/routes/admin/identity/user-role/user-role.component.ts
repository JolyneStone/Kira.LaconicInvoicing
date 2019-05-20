import { Component, AfterViewInit, Injector, } from '@angular/core';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'admin-identity-user-role',
  template: `<div id="grid-box-{{moduleName}}"></div>`
})
export class UserRoleComponent extends GridComponentBase implements AfterViewInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = "userrole";
  }

  async ngAfterViewInit() {
    let auth = await this.checkAuth();
    if (auth.Read) {
      super.InitBase();
      super.ViewInitBase();
    }
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Admin.Identity.UserRole", ["Read", "Update"]);
  }

  protected GetModel() {
    return {
      id: "id",
      fields: {
        userId: { type: "number", editable: false },
        roleId: { type: "number", editable: false },
        userName: { type: "string", validation: { required: true } },
        roleName: { type: "string", validation: { required: true } },
        isLocked: { type: "boolean" },
        createdTime: { type: "date", editable: false },
        updatable: { type: "boolean", editable: false },
      }
    };
  }
  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      {
        field: "userId",
        title: "用户",
        width: 150,
        template: "#=userId#.#=userName#"
      }, {
        field: "roleId",
        title: "角色",
        width: 150,
        template: "#=roleId#.#=roleName#"
      }, {
        field: "isLocked",
        title: "锁定",
        width: 95,
        template: d => this.kendoui.Boolean(d.isLocked),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "createdTime",
        title: "注册时间",
        width: 115,
        format: "{0:yy-MM-dd HH:mm}"
      }
    ];
  }

  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    let options = super.GetDataSourceOptions();
    delete options.transport.destroy;
    return options;
  }

}
