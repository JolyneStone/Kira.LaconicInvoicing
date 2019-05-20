import { User as NzUser } from '@delon/theme';

//#region OSharp Tools
export class AjaxResult {
  type: AjaxResultType;
  content?: string;
  data?: any;
}
export enum AjaxResultType {
  info = 203,
  success = 200,
  error = 500,
  unAuth = 401,
  forbidden = 403,
  noFound = 404,
  locked = 423
}

/** 分页数据 */
export class PageData<T> {
  rows: T[];
  total: number;
}
export class ListNode {
  id: number;
  text: string;
}

/** 查询条件 */
export class FilterRule {
  /**属性名 */
  field: string;
  /**属性值 */
  value: string;
  /**对比操作 */
  operate: FilterOperate;

  /**
   * 实例化一个条件信息
   * @param field 字段名
   * @param value 属性值
   * @param operate 对比操作
   */
  constructor(field: string, value: string, operate: FilterOperate = FilterOperate.equal) {
    this.field = field;
    this.value = value;
    this.operate = operate;
  }
}
/** 查询条件组 */
export class FilterGroup {
  /**条件集合 */
  rules: FilterRule[] = [];
  /**条件间操作 */
  operate: FilterOperate = FilterOperate.and;
  /**条件组集合 */
  groups: FilterGroup[] = [];
  level: number = 1;

  static Init(group: FilterGroup) {
    if (!group.level) {
      group.level = 1;
    }
    group.groups.forEach(subGroup => {
      subGroup.level = group.level + 1;
      FilterGroup.Init(subGroup);
    });
  }
}
/**比较操作枚举 */
export enum FilterOperate {
  and = 1,
  or = 2,
  equal = 3,
  notEqual = 4,
  less = 5,
  lessOrEqual = 6,
  greater = 7,
  greaterOrEqual = 8,
  startsWith = 9,
  endsWith = 10,
  contains = 11,
  notContains = 12,
}
export class FilterOperateEntry {
  operate: FilterOperate;
  display: string;

  constructor(operate: FilterOperate) {
    this.operate = operate;
    switch (operate) {
      case FilterOperate.and:
        this.display = "并且";
        break;
      case FilterOperate.or:
        this.display = "或者";
        break;
      case FilterOperate.equal:
        this.display = "等于";
        break;
      case FilterOperate.notEqual:
        this.display = "不等于";
        break;
      case FilterOperate.less:
        this.display = "小于";
        break;
      case FilterOperate.lessOrEqual:
        this.display = "小于等于";
        break;
      case FilterOperate.greater:
        this.display = "大于";
        break;
      case FilterOperate.greaterOrEqual:
        this.display = "大于等于";
        break;
      case FilterOperate.startsWith:
        this.display = "开始于";
        break;
      case FilterOperate.endsWith:
        this.display = "结束于";
        break;
      case FilterOperate.contains:
        this.display = "包含";
        break;
      case FilterOperate.notContains:
        this.display = "不包含";
        break;
      default:
        this.display = "未知操作";
        break;
    }
    this.display = `${<number>operate}.${this.display}`;
  }
}
/** 分页请求 */
export class PageRequest {
  /** 分页条件信息 */
  pageCondition: PageCondition = new PageCondition();
  /** 查询条件组 */
  filterGroup: FilterGroup = new FilterGroup();
}
/** 分页条件 */
export class PageCondition {
  /** 页序 */
  pageIndex: number = 1;
  /** 分页大小 */
  pageSize: number = 20;
  /** 排序条件集合 */
  sortConditions: SortCondition[] = [];
}
export class SortCondition {
  sortField: string;
  listSortDirection: ListSortDirection;
}
export enum ListSortDirection {
  ascending,
  descending
}


/** 实体属性信息 */
export class EntityProperty {
  name: string;
  display: string;
  typeName: string;
  isUserFlag: boolean;
  valueRange: any[];
}

/**
 * 验证码类
 */
export class VerifyCode {
  /** 验证码后台编号 */
  id: string;
  /** 验证码图片的Base64格式 */
  image: string = "data:image/png;base64,null";
  /** 输入的验证码 */
  code: string;
}

//#endregion

//#region Identity Model
export class LoginDto {
  account: string;
  password: string;
  verifyCode: string;
  verifyCodeId: string;
  remember = true;
  returnUrl: string;
}
export class RegisterDto {
  userName: string;
  password: string;
  confirmPassword: string;
  nickName: string;
  email: string;
  verifyCode: string;
  verifyCodeId: string;
}
export class ChangePasswordDto {
  userId: string;
  oldPassword: string;
  newPassword: string;
  confirmPassword: string;
}
export class ConfirmEmailDto {
  userId: string;
  code: string;
}
export class SendMailDto {
  email: string;
  verifyCode: string;
  verifyCodeId: string;
}
export class ResetPasswordDto {
  userId: string;
  token: string;
  newPassword: string;
  confirmPassword: string;
}
/**权限配置信息 */
export class AuthConfig {
  constructor(
    /**当前模块的位置，即上级模块的路径，如Root,Root.Admin,Root.Admin.Identity */
    public position: string,
    /**要权限控制的功能名称，可以是节点名称或全路径 */
    public funcs: string[]
  ) { }
}

/** 用户信息 */
export class User implements NzUser {
  constructor() {
    this.roles = [];
  }
  id?: number;
  name?: string;
  avatar?: string;
  email?: string;
  [key: string]: any;
  nickName?: string;
  roles?: string[];
  isAdmin?: boolean;
  telephone?: string;
}

//#endregion

//#region system

/**
 * 系统初始化安装DTO
 */
export class InstallDto {
  siteName: string;
  siteDescription: string;
  adminUserName: string;
  adminPassword: string;
  confirmPassword: string;
  adminEmail: string;
  adminNickName: string;
}

//#endregion

//#region delon

export class AdResult {
  /**
   * 是否显示结果框
   */
  show: boolean = false;
  /**结果类型，可选为： 'success' | 'error' | 'minus-circle-o'*/
  type: 'success' | 'error' | 'minus-circle-o';
  /** 结果标题 */
  title: string;
  /** 结果描述 */
  description: string;
}

//#endregion
