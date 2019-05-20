import { Component, AfterViewInit, Injector, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { List } from 'linqts';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';


@Component({
  selector: 'admin-identity-user',
  templateUrl: './user.component.html'
})
export class UserComponent extends GridComponentBase implements AfterViewInit {

  roleWindow: kendo.ui.Window;
  moduleWindow: kendo.ui.Window;
  windowOptions: kendo.ui.WindowOptions;
  tabstripOptions: kendo.ui.TabStripOptions;
  roleTreeOptions: kendo.ui.TreeViewOptions;
  roleTree: kendo.ui.TreeView;
  moduleTreeOptions: kendo.ui.TreeViewOptions;
  moduleTree: kendo.ui.TreeView;

  http: HttpClient;

  constructor(injector: Injector) {
    super(injector);
    this.http = injector.get(HttpClient);
    this.moduleName = 'user';
    this.windowOptions = {
      visible: false, width: 500, height: 620, modal: true, title: '用户权限设置', actions: ['Pin', 'Minimize', 'Maximize', 'Close'],
      resize: e => this.onWinResize(e)
    };
    this.roleTreeOptions = { checkboxes: { checkChildren: true, }, dataTextField: 'name', select: e => this.kendoui.OnTreeNodeSelect(e) };
    this.moduleTreeOptions = { checkboxes: { checkChildren: true }, dataTextField: 'name', select: e => this.kendoui.OnTreeNodeSelect(e) };
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
    return new AuthConfig('Root.Admin.Identity.User', ['Read', 'Create', 'Update', 'Delete', 'SetRoles', 'SetModules']);
  }

  //#region GridBase

  protected GetModel() {
    return {
      id: 'id',
      fields: {
        id: { type: 'number', editable: false },
        userName: { type: 'string', validation: { required: true } },
        nickName: { type: 'string', validation: { required: true } },
        email: { type: 'string', validation: { required: true } },
        emailConfirmed: { type: 'boolean', editable: true },
        phoneNumber: { type: 'string', editable: true },
        phoneNumberConfirmed: { type: 'boolean', editable: true },
        isLocked: { type: 'boolean', editable: true },
        lockoutEnabled: { type: 'boolean', editable: true },
        lockoutEnd: { type: 'date', editable: false },
        accessFailedCount: { type: 'number', editable: false },
        createdTime: { type: 'date', editable: false },
        roles: { editable: false },
        updatable: { type: 'boolean', editable: false },
        deletable: { type: 'boolean', editable: false },
      }
    };
  }
  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      {
        command: [
          { name: 'setRoles', text: '', iconClass: 'k-icon k-i-link-horizontal', click: e => this.roleWindowOpen(e) },
          { name: 'setModules', text: '', iconClass: 'k-icon k-i-unlink-horizontal', click: e => this.moduleWindowOpen(e) },
          { name: 'destroy', iconClass: 'k-icon k-i-delete', text: '', visible: d => d.Deletable },
        ],
        width: 100
      },
      { field: 'id', title: '编号', width: 70, locked: true },
      {
        field: 'userName',
        title: '用户名',
        width: 150,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: 'nickName',
        title: '昵称',
        width: 130,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: 'email',
        title: '邮箱',
        width: 180,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: 'emailConfirmed',
        title: '邮箱确认',
        width: 95,
        template: d => this.kendoui.Boolean(d.emailConfirmed),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: 'phoneNumber',
        title: '手机号',
        width: 105,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: 'phoneNumberConfirmed',
        title: '手机确认',
        width: 95,
        template: d => this.kendoui.Boolean(d.phoneNumberConfirmed),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: 'roles',
        title: '角色',
        width: 180,
        template: d => this.osharp.expandAndToString(d.roles)
      }, {
        field: 'isLocked',
        title: '是否锁定',
        width: 95,
        template: d => this.kendoui.Boolean(d.isLocked),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: 'lockoutEnabled',
        title: '登录锁',
        width: 90,
        template: d => this.kendoui.Boolean(d.lockoutEnabled),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: 'lockoutEnd',
        title: '锁时间',
        width: 120,
        format: '{0:yy-MM-dd HH:mm}'
      }, {
        field: 'accessFailedCount',
        title: '登录错误',
        width: 95
      }, {
        field: 'createdTime',
        title: '注册时间',
        width: 120,
        format: '{0:yy-MM-dd HH:mm}'
      }
    ];
  }

  protected FilterGridAuth(options: kendo.ui.GridOptions) {
    // 命令列
    let cmdColumn = options.columns && options.columns.find(m => m.command != null);
    let cmds = cmdColumn && cmdColumn.command as kendo.ui.GridColumnCommandItem[];
    if (cmds) {
      if (!this.auth.SetRoles) {
        this.osharp.remove(cmds, m => m.name == 'setRoles');
      }
      if (!this.auth.SetModules) {
        this.osharp.remove(cmds, m => m.name == 'setModules');
      }
    }
    options = super.FilterGridAuth(options);
    return options;
  }
  //#endregion

  //#region Window

  private winUser: any;

  //#region RoleWindow

  onRoleWinInit(win) {
    this.roleWindow = win;
  }
  private roleWindowOpen(e) {
    e.preventDefault();
    let tr = $(e.target).closest('tr');
    this.winUser = this.grid.dataItem(tr);
    this.roleWindow.title('用户角色设置-' + this.winUser.userName).open().center().resize();
    this.roleTree.setDataSource(this.kendoui.CreateHierarchicalDataSource('api/admin/role/ReadUserRoles?userId=' + this.winUser.id));
  }
  onRoleWinSubmit() {
    let roles = this.roleTree.dataSource.data();
    let checkRoleIds = new List(roles.slice(0)).Where(m => m.checked).Select(m => m.id).ToArray();

    let params = { userId: this.winUser.id, roleIds: checkRoleIds };

    this.http.post('api/admin/user/setroles', params).subscribe(res => {
      this.osharp.ajaxResult(res, () => {
        this.grid.dataSource.read();
        this.roleWindow.close();
      });
    });
  }

  //#endregion

  //#region ModuleWindow

  onModuleWinInit(win) {
    this.moduleWindow = win;
  }
  private moduleWindowOpen(e) {
    e.preventDefault();
    let tr = $(e.target).closest('tr');
    this.winUser = this.grid.dataItem(tr);
    this.moduleWindow.title('用户模块设置-' + this.winUser.userName).open().center().resize();
    this.moduleTree.setDataSource(this.kendoui.CreateHierarchicalDataSource('api/admin/module/ReadUserModules?userId=' + this.winUser.id));
  }
  onModuleWinSubmit() {
    let moduleRoot = this.moduleTree.dataSource.data()[0];
    let modules = [];
    this.osharp.getTreeNodes(moduleRoot, modules);
    let checkModuleIds = new List(modules).Where(m => m.checked).Select(m => m.id).ToArray();
    let params = { userId: this.winUser.id, moduleIds: checkModuleIds };

    this.http.post('api/admin/user/setmodules', params).subscribe(res => {
      this.osharp.ajaxResult(res, () => {
        this.grid.dataSource.read();
        this.moduleWindow.close();
      });
    });
  }

  //#endregion

  private onWinResize(e) {
    $('.win-content .k-tabstrip .k-content').height(e.height - 140);
  }

  //#endregion

  //#region Tree

  onRoleTreeInit(tree) {
    this.roleTree = tree;
  }
  onModuleTreeInit(tree) {
    this.moduleTree = tree;
  }

  //#endregion

}
