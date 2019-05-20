import { Component, AfterViewInit, Injector } from '@angular/core';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig, FilterGroup, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'admin-security-role-entity',
  templateUrl: './role-entity.component.html'
})
export class RoleEntityComponent extends GridComponentBase implements AfterViewInit {

  splitterOptions: kendo.ui.SplitterOptions;
  http: HttpClient;
  filterGroup: FilterGroup;
  entityType: string;
  selectName: string = "未选择";
  groupJson: string;
  selectData: any = null;

  constructor(injector: Injector) {
    super(injector);
    this.http = injector.get(HttpClient);
    this.moduleName = "roleentity";
    this.splitterOptions = {
      panes: [{ size: "50%" }, { collapsible: true, collapsed: false }]
    };
  }

  async ngAfterViewInit() {
    await this.checkAuth();
    if (this.auth.Read) {
      super.InitBase();
      super.ViewInitBase();
    }
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Admin.Security.RoleEntity", ["Read", "Create", "Update", "Delete"]);
  }

  protected GetModel() {
    return {
      id: "id",
      fields: {
        id: { type: "string", editable: false, defaultValue: "00000000-0000-0000-0000-000000000000" },
        roleId: { type: "number", editable: true },
        entityId: { type: "string", editable: true },
        roleName: { type: "string", validation: { required: true } },
        entityName: { type: "string", validation: { required: true } },
        entityType: { type: "string", validation: { required: true } },
        operation: { type: "number", editable: true },
        filterGroup: { type: "object", editable: false },
        isLocked: { type: "boolean" },
        createdTime: { type: "date", editable: false },
        updatable: { type: "boolean", editable: false },
      }
    };
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    const columns = [{
      command: [
        { name: "destroy", iconClass: "k-icon k-i-delete", text: "" },
      ],
      width: 100
    }, {
      field: "roleId",
      title: "角色",
      width: 120,
      template: "#=roleId#.#=roleName#",
      editor: ($container, options) => this.kendoui.RemoteDropDownListEditor($container, options, "api/admin/role/ReadNode", "roleName", "roleId"),
      filterable: { ui: el => this.kendoui.RemoteDropDownList(el, "api/admin/role/ReadNode", "roleName", "roleId") }
    }, {
      field: "entityId",
      title: "数据实体",
      width: 300,
      template: "#=entityName# [#=entityType#]",
      editor: ($container, options) => this.kendoui.RemoteDropDownListEditor($container, options, "api/admin/entityinfo/ReadNode", "name", "id"),
      filterable: { ui: el => this.kendoui.RemoteDropDownList(el, "api/admin/entityinfo/ReadNode", "name", "id") }
    }, {
      field: "operation",
      title: "操作",
      width: 75,
      template: d => this.osharp.valueToText(d.operation, this.osharp.data.dataAuthOperations),
      editor: ($container, options) => this.kendoui.DropDownListEditor($container, options, this.osharp.data.dataAuthOperations),
      filterable: { ui: el => this.kendoui.DropDownList(el, this.osharp.data.dataAuthOperations) }
    }, {
      field: "isLocked",
      title: "锁定",
      width: 95,
      template: d => this.kendoui.Boolean(d.isLocked),
      editor: ($container, options) => this.kendoui.BooleanEditor($container, options)
    }, {
      field: "createdTime",
      title: "注册时间",
      width: 115,
      format: "{0:yy-MM-dd HH:mm}"
    }];
    return columns;
  }

  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    let options = super.GetDataSourceOptions();
    options.group = [{ field: "roleName" }, { field: "entityName" }];
    return options;
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    let options = super.GetGridOptions(dataSource);
    options.selectable = true;
    options.change = e => {
      let row = this.grid.select();
      if (row) {
        let data: any = this.grid.dataItem(row);
        if (data) {
          this.selectData = data;
          this.selectName = `${data.roleName} + ${data.entityName} + ${this.osharp.valueToText(data.operation, this.osharp.data.dataAuthOperations)}`;
          if (data.filterGroup) {
            this.filterGroup = data.filterGroup;
          } else {
            this.filterGroup = new FilterGroup();
          }
          this.entityType = data.entityType;
        } else {
          this.selectName = "未选择";
          this.filterGroup = new FilterGroup();
          this.entityType = null;
        }
      } else {
        this.selectName = "未选择";
        this.filterGroup = new FilterGroup();
        this.entityType = null;
      }
    };

    return options;
  }

  showGroupJson() {
    this.groupJson = JSON.stringify(this.filterGroup, null, 2);
  }
  saveFilterGroup() {
    if (this.entityType == null) {
      this.osharp.info("请在左边选中一行，再进行操作");
      return;
    }
    let data = this.selectData;
    let dto = { id: data.id, roleId: data.roleId, entityId: data.entityId, operation: data.operation, filterGroup: this.filterGroup, isLocked: data.isLocked };
    this.http.post<AjaxResult>("api/admin/roleentity/Update", [dto]).subscribe(res => {
      this.osharp.ajaxResult(res);
    });
  }
}
