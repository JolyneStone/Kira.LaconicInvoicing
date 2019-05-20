import { Menu } from '@delon/theme';

// 导航菜单
const headingMain: Menu = { text: '导航菜单', i18n: 'menu.main', group: true, children: [] };
const home: Menu = { text: '仪表盘', i18n: 'menu.dashboard', link: '/dashboard', icon: 'anticon anticon-dashboard' };

export const defaultMenu: Menu[] = [
  {
    text: '导航菜单', i18n: 'menu.main', group: true, children: [
      { text: '仪表盘', i18n: 'menu.dashboard', link: '/dashboard', icon: 'anticon anticon-dashboard' }
    ]
  },
  {
    text: '采购管理', i18n: 'menu.dashboard.purchase', group: true, children: [
      { text: '供应商管理', i18n: 'menu.dashboard.purchase.vendor', icon: 'anticon anticon-car', link: '/dashboard/purchase/vendorlist' },
      { text: '原料管理', i18n: 'menu.dashboard.purchase.material', icon: 'anticon anticon-experiment', link: '/dashboard/purchase/materiallist' },
      { text: '采购单管理', i18n: 'menu.dashboard.purchase.purchaseorder', icon: 'anticon anticon-reconciliation', link: '/dashboard/purchase/purchaseorderlist' },
      { text: '统计分析', i18n: 'menu.dashboard.purchase.statistics', icon: 'anticon anticon-bar-chart', link: '/dashboard/purchase/statistics' },
    ]
  },
  {
    text: '库存管理', i18n: 'menu.dashboard.warehouse', group: true, children: [
      { text: '仓库管理', i18n: 'menu.dashboard.warehouse.warehouse', icon: 'anticon anticon-shop', link: '/dashboard/warehouse/warehouselist' },
      { text: '入库单管理', i18n: 'menu.dashboard.warehouse.inboundreceipt', icon: 'anticon anticon-down', link: '/dashboard/warehouse/inboundreceiptlist' },
      { text: '出库单管理', i18n: 'menu.dashboard.warehouse.outboundreceipt', icon: 'anticon anticon-up', link: '/dashboard/warehouse/outboundreceiptlist' },
      { text: '调拨单管理', i18n: 'menu.dashboard.warehouse.transferorder', icon: 'anticon anticon-swap', link: '/dashboard/warehouse/transferorderlist' },
      { text: '统计分析', i18n: 'menu.dashboard.warehouse.statistics', icon: 'anticon anticon-line-chart', link: '/dashboard/warehouse/statistics' },
    ]
  },
  {
    text: '销售管理', i18n: 'menu.dashboard.sale', group: true, children: [
      { text: '客户管理', i18n: 'menu.dashboard.sale.customer', icon: 'anticon anticon-crown', link: '/dashboard/sale/customerlist' },
      { text: '产品管理', i18n: 'menu.dashboard.sale.product', icon: 'anticon anticon-rocket', link: '/dashboard/sale/productlist' },
      { text: '销售单管理', i18n: 'menu.dashboard.sale.saleorder', icon: 'anticon anticon-schedule', link: '/dashboard/sale/saleorderlist' },
      { text: '统计分析', i18n: 'menu.dashboard.sale.statistics', icon: 'anticon anticon-area-chart', link: '/dashboard/sale/statistics' },
    ]
  }
];
