import { Menu } from "@delon/theme";

// 导航菜单
const headingMain: Menu = { text: '导航菜单', i18n: 'menu.main', group: true, children: [] };
const home: Menu = { text: '仪表盘', i18n: 'menu.dashboard', link: '/admin/dashboard', icon: 'anticon anticon-dashboard' };

export const adminMenu: Menu[] = [
  {
    text: '导航菜单', i18n: 'menu.main', group: true, children: [
      { text: '仪表盘', i18n: 'menu.dashboard', link: '/admin/dashboard', icon: 'anticon anticon-dashboard' }
    ]
  },
  {
    text: '权限模块', i18n: 'menu.permission', group: true, children: [
      {
        text: '身份认证', i18n: 'menu.permission.identity', group: true, icon: 'anticon anticon-key', children: [
          { text: '用户信息管理', i18n: 'menu.permission.identity.user', link: '/admin/identity/user' },
          { text: '角色信息管理', i18n: 'menu.permission.identity.role', link: '/admin/identity/role' },
          { text: '用户角色管理', i18n: 'menu.permission.identity.user-role', link: '/admin/identity/user-role' }
        ]
      },
      {
        text: '权限安全', i18n: 'menu.permission.security', group: true, icon: 'anticon anticon-safety', children: [
          { text: '模块信息管理', i18n: 'menu.permission.security.module', link: '/admin/security/module' },
          { text: '功能信息管理', i18n: 'menu.permission.security.function', link: '/admin/security/function' },
          { text: '角色功能管理', i18n: 'menu.permission.security.role-function', link: '/admin/security/role-function' },
          { text: '用户功能管理', i18n: 'menu.permission.security.user-function', link: '/admin/security/user-function' },
          { text: '实体信息管理', i18n: 'menu.permission.security.entityinfo', link: '/admin/security/entityinfo' },
          { text: '角色数据管理', i18n: 'menu.permission.security.role-entity', link: '/admin/security/role-entity' },
        ]
      }
    ]
  },
  {
    text: '系统模块', i18n: 'menu.system', group: true, children: [
      {
        text: '系统配置', i18n: 'menu.stytem.config', group: true, icon: 'anticon anticon-file-protect', children: [
          { text: '数据字典', i18n: 'menu.system.config.basedata', link: '/admin/system/base-data' },
          { text: '套打模板', i18n: 'menu.system.config.print-management', link: '/admin/system/print' },
          { text: '文档模板', i18n: 'menu.system.config.document-management', link: '/admin/system/document' },
        ]
      },
      {
        text: '系统管理', i18n: 'menu.system.manage', group: true, icon: 'anticon anticon-setting', children: [
          { text: '公告发布', i18n: 'menu.system.manage.notice', link: '/admin/system/notice' },
          { text: '操作审计', i18n: 'menu.system.manage.audit-operation', link: '/admin/system/audit-operation' },
          { text: '数据审计', i18n: 'menu.system.manage.audit-entity', link: '/admin/system/audit-entity' },
          { text: '模块包', i18n: 'menu.system.manage.pack', link: '/admin/system/pack' },
        ]
      }
    ]
  }
];
