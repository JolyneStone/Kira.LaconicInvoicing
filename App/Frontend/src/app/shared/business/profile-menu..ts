import { Menu } from "@delon/theme";

// 导航菜单
const headingMain: Menu = { text: '导航菜单', i18n: 'menu.main', group: true, children: [] };
const home: Menu = { text: '仪表盘', i18n: 'menu.dashboard', link: '/admin/dashboard', icon: 'anticon anticon-dashboard' };

export const profileMenu: Menu[] = [
  {
    text: '导航菜单', i18n: 'menu.main', group: true, children: [
      { text: '仪表盘', i18n: 'menu.dashboard', link: '/admin/dashboard', icon: 'anticon anticon-dashboard' }
    ]
  },
  {
    text: '个人信息', i18n: 'menu.account', group: true, children: [
      { text: '个人中心', i18n: 'menu.account.center', icon: 'anticon anticon-user', link: '/profile/account/center' },
      { text: '个人设置', i18n: 'menu.account.settings', icon: 'anticon anticon-setting', link: '/profile/account/settings' }
    ]
  }
];
