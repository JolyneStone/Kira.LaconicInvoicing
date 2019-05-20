import { Component, AfterViewInit, Injector } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { List } from 'linqts';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'admin-identity-role',
  templateUrl: './role.component.html',
})
export class RoleComponent extends GridComponentBase implements AfterViewInit {
  windowOptions: kendo.ui.WindowOptions;
  window: kendo.ui.Window;
  moduleTreeOptions: kendo.ui.TreeViewOptions;
  moduleTree: kendo.ui.TreeView;
  http: HttpClient;

  constructor(injector: Injector) {
    super(injector);
    this.http = injector.get(HttpClient);
    this.moduleName = 'role';
    this.windowOptions = {
      visible: false,
      width: 500,
      height: 620,
      modal: true,
      title: '角色权限设置',
      actions: ['Pin', 'Minimize', 'Maximize', 'Close'],
      resize: e => this.onWinResize(e),
    };
    this.moduleTreeOptions = {
      autoBind: true,
      checkboxes: { checkChildren: true },
      dataTextField: 'name',
      select: e => this.kendoui.OnTreeNodeSelect(e),
    };
  }

  async ngAfterViewInit() {
    await this.checkAuth();
    if (this.auth.Read) {
      super.InitBase();
      super.ViewInitBase();
    } else {
      this.osharp.error('无权查看该页面');
    }
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.Admin.Identity.Role', [
      'Read',
      'Create',
      'Update',
      'Delete',
      'SetModules',
    ]);
  }

  //#region GridBase

  protected GetModel() {
    return {
      id: 'id',
      fields: {
        id: { type: 'number', editable: false },
        name: { type: 'string', validation: { required: true } },
        remark: { type: 'string', editable: true },
        isAdmin: { type: 'boolean', editable: true },
        isDefault: { type: 'boolean', editable: true },
        isLocked: { type: 'boolean', editable: true },
        createdTime: { type: 'date', editable: false },
        updatable: { type: 'boolean', editable: false },
        deletable: { type: 'boolean', editable: false },
      },
    };
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      {
        command: [
          {
            name: 'setModules',
            text: '',
            iconClass: 'k-icon k-i-unlink-horizontal',
            click: e => this.windowOpen(e),
          },
          {
            name: 'destroy',
            iconClass: 'k-icon k-i-delete',
            text: '',
            visible: d => d.deletable,
          },
        ],
        width: 100,
      },
      { field: 'id', title: '编号', width: 70 },
      {
        field: 'name',
        title: '角色名',
        width: 150,
        filterable: this.osharp.data.stringFilterable,
      },
      {
        field: 'remark',
        title: '备注',
        width: 250,
        filterable: this.osharp.data.stringFilterable,
      },
      {
        field: 'isAdmin',
        title: '管理',
        width: 95,
        template: d => this.kendoui.Boolean(d.isAdmin),
        editor: (container, options) =>
          this.kendoui.BooleanEditor(container, options),
      },
      {
        field: 'isDefault',
        title: '默认',
        width: 95,
        template: d => this.kendoui.Boolean(d.isDefault),
        editor: (container, options) =>
          this.kendoui.BooleanEditor(container, options),
      },
      {
        field: 'isLocked',
        title: '锁定',
        width: 95,
        template: d => this.kendoui.Boolean(d.isLocked),
        editor: (container, options) =>
          this.kendoui.BooleanEditor(container, options),
      },
      {
        field: 'createdTime',
        title: '注册时间',
        width: 120,
        format: '{0:yy-MM-dd HH:mm}',
      },
    ];
  }

  protected FilterGridAuth(options: kendo.ui.GridOptions) {
    // 命令列
    let cmdColumn =
      options.columns && options.columns.find(m => m.command != null);
    let cmds =
      cmdColumn && (cmdColumn.command as kendo.ui.GridColumnCommandItem[]);
    if (cmds) {
      if (!this.auth.SetModules) {
        this.osharp.remove(cmds, m => m.name == 'setModules');
      }
    }
    options = super.FilterGridAuth(options);
    return options;
  }

  //#endregion

  //#region Window

  private winRole;
  onWinInit(win) {
    this.window = win;
  }
  windowOpen(e) {
    e.preventDefault();
    let tr = $(e.target).closest('tr');
    this.winRole = this.grid.dataItem(tr);
    this.window
      .title('角色模块设置-' + this.winRole.name)
      .open()
      .center()
      .resize();
    // 设置树数据
    this.moduleTree.setDataSource(
      this.kendoui.CreateHierarchicalDataSource(
        'api/admin/module/ReadRoleModules?roleId=' + this.winRole.id,
      ),
    );
  }
  private onWinResize(e) {
    $('.win-content .k-tabstrip .k-content').height(e.height - 140);
  }
  onWinSubmit() {
    let moduleRoot = this.moduleTree.dataSource.data()[0];
    let modules = [];
    this.osharp.getTreeNodes(moduleRoot, modules);
    let checkModuleIds = new List(modules)
      .Where(m => m.checked)
      .Select(m => m.id)
      .ToArray();
    let body = { roleId: this.winRole.id, moduleIds: checkModuleIds };
    this.http.post('api/admin/role/SetModules', body).subscribe(res => {
      this.osharp.ajaxResult(res, () => {
        this.grid.dataSource.read();
        this.window.close();
      });
    });
  }

  //#endregion

  //#region Tree

  onModuleTreeInit(tree) {
    this.moduleTree = tree;
  }

  //#endregion
}
