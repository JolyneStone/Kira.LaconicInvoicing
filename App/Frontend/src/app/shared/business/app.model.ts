export class QueryCondition {
  constructor() {
    this.filters = [];
    this.sorts = [];
  }

  filters?: QueryConditionItem[];
  sorts?: SortDescription[];
  pageIndex: number;
  pageSize: number;
}

export class QueryConditionItem {
  field?: string;
  logicOp?: string;
  conditionOp?: string;
  value?: any;
}

export class SortDescription {
  field?: string;
  order?: string;
}

/** 用户详细信息 */
export class UserDetail {
  constructor() {
  }

  userId?: number;
  nickName?: string;
  userName?: string;
  avatar?: string;
  personalProfile?: string;
  country?: string;
  province?: string;
  city?: string;
  address?: string;
  telephone?: string;
  department?: string;
  roles?: string[];
}

/** 重置邮箱DTO */
export class ResetEmailDto {
  newEmail: string;
  verifyCode: string;
  verifyCodeId: string;
}

export class BaseDataDto {
  id?: string;
  type?: string;
  value?: string;
}

export class BaseDataListDto {
  name?: string;
  code?: string;
}

/** 供应商DTO */
export class VendorDto {
  id?: string;
  number?: string;
  name?: string;
  type?: string;
  debt?: string;
  contactPerson?: string;
  phoneNumber?: string;
  email?: string;
  address?: string;
  comment?: string;
  operator?: string;
  dateTime?: Date;
}

/** 原料DTO */
export class MaterialDto {
  id?: string;
  number?: string;
  name?: string;
  type?: string;
  spec?: string;
  brand?: string;
  unit?: string;
  price?: number;
  comment?: string;
  operator?: string;
  dateTime?: Date;
  vendorId?: string;
  vendorName?: string;
}

export class VendorQueryDto {
  number?: string;
  name?: string;
  type?: string;
  rangeDate?: Date[];
}

export class MaterialQueryDto {
  number?: string;
  name?: string;
  type?: string;
  rangeDate?: Date[];
}

export enum PurchaseOrderStatus {
  book = 0,
  order = 1,
  transporting = 2,
  receipt = 3,
  complete = 4,
  return = 9
}

export enum WarehouseStatus {
  usable = 0,
  full = 1,
  maintain = 2,
  build = 3,
  abandon = 4
}

export class PurchaseOrderQueryDto {
  number?: string;
  vendorName?: string;
  vendorNumber?: string;
  rangeDate?: Date[];
}

export class PurchaseOrderDto {
  id?: string;
  number?: string;
  vendorNumber?: string;
  vendorName?: string;
  consignorContact?: string;
  consignor?: string;
  sourceAddress?: string;
  destAddress?: string;
  warehouseNumber?: string;
  warehouseName?: string;
  consignee?: string;
  consigneeContact?: string;
  freight?: number;
  totalAmount?: number;
  totalQuantity?: number;
  amountPaid?: number;
  payWay?: string;
  status?: PurchaseOrderStatus;
  comment?: string;
  operator?: string;
  dateTime?: Date;
  items?: PurchaseOrderItemDto[];
}

export class PurchaseOrderItemDto {
  id?: string;
  purchaseOrderId?: string;
  amount?: number;
  number?: string;
  name?: string;
  type?: string;
  spec?: string;
  brand?: string;
  unit?: string;
  price?: number;
  comment?: string;
}

export class WarehouseQueryDto {
  number?: string;
  name?: string;
  address?: string;
  rangeDate?: Date[];
}

export class WarehouseDto {
  id?: string;
  number?: string;
  name?: string;
  manager?: string;
  managerContact?: string;
  area?: number;
  status?: WarehouseStatus;
  address?: string;
  comment?: string;
  operator?: string;
  dateTime?: Date;
}

export enum GoodsCategory {
  material = 0,
  product = 1
}

export class InboundReceiptQueryDto {
  number?: string;
  supplier?: string;
  supplierNo?: string;
  warehouseNumber?: string;
  warehouseName?: string;
  rangeDate?: Date[];
}

export class OutboundReceiptQueryDto {
  number?: string;
  receiver?: string;
  receiverNo?: string;
  warehouseNumber?: string;
  warehouseName?: string;
  rangeDate?: Date[];
}

export class InboundReceiptDto {
  id?: string;
  warehouseNumber?: string;
  warehouseName?: string;
  number?: string;
  waybillNo?: string;
  supplier?: string;
  supplierNo?: string;
  supplyAddress?: string;
  deliveryMan?: string;
  deliveryManContact?: string;
  consignee?: string;
  consigneeContact?: string;
  comment?: string;
  operator?: string;
  dataTime?: Date;
  items?: InboundReceiptItemDto[];
}

export class OutboundReceiptDto {
  id?: string;
  warehouseNumber?: string;
  warehouseName?: string;
  number?: string;
  waybillNo?: string;
  receiver?: string;
  receiverNo?: string;
  receiverAddress?: string;
  deliveryMan?: string;
  deliveryManContact?: string;
  consignor?: string;
  consignorContact?: string;
  comment?: string;
  operator?: string;
  dataTime?: Date;
  items?: OutboundReceiptItemDto[];
}

export class InboundReceiptItemDto {
  id?: string;
  inboundReceiptId?: string;
  goodsCategory?: GoodsCategory;
  amount?: number;
  number?: string;
  name?: string;
  type?: string;
  spec?: string;
  brand?: string;
  unit?: string;
  price?: string;
  comment?: string;
}

export class OutboundReceiptItemDto {
  id?: string;
  outboundReceiptId?: string;
  goodsCategory?: GoodsCategory;
  amount?: number;
  number?: string;
  name?: string;
  type?: string;
  spec?: string;
  brand?: string;
  unit?: string;
  price?: string;
  comment?: string;
}

export class InventoryQueryDto {
  warehouseId?: string;
  number?: string;
  goodsCategory?: GoodsCategory;
  name?: string;
  rangeDate?: Date[];
}

export class InventoryDto {
  id?: string;
  warehouseId?: string;
  amount?: number;
  name?: string;
  number?: string;
  goodsCategory?: GoodsCategory;
  dateTime?: Date;
  spec?: string;
  price?: number;
  unit?: string;
  brand?: string;
  type?: string;
}

export class TransferOrderQueryDto {
  sourceWarehouseNumber?: string;
  sourceWarehouseName?: string;
  destWarehouseNumber?: string;
  destWarehouseName?: string;
  number?: string;
  rangeDate?: Date[];
}

export class TransferOrderDto {
  id?: string;
  number?: string;
  sourceWarehouseNumber?: string;
  sourceWarehouseName?: string;
  sourceAddress?: string;
  destWarehouseNumber?: string;
  destWarehouseName?: string;
  destAddress?: string;
  consignor?: string;
  consignorContact?: string;
  consignee?: string;
  consigneeContact?: string;
  waybillNo?: string;
  deliveryMan?: string;
  deliveryManContact?: string;
  comment?: string;
  operator?: string;
  dateTime?: Date;
  items?: TransferOrderItemDto[];
}

export class TransferOrderItemDto {
  id?: string;
  transferOrderId?: string;
  goodsCategory?: GoodsCategory;
  amount?: number;
  number?: string;
  name?: string;
  type?: string;
  spec?: string;
  brand?: string;
  unit?: string;
  price?: number;
  comment?: string;
}

export enum SaleOrderStatus {
  book = 0,
  order = 1,
  transporting = 2,
  receipt = 3,
  complete = 4,
  return = 9
}

export class ProductDto {
  id?: string;
  number?: string;
  name?: string;
  type?: string;
  spec?: string;
  brand?: string;
  unit?: string;
  costPrice?: number;
  wholesalePrice?: number;
  retailPrice?: number;
  comment?: string;
  operator?: string;
  dateTime?: Date;
}

export class CustomerDto {
  id?: string;
  number?: string;
  name?: string;
  type?: string;
  debt?: number;
  contactPerson?: string;
  phoneNumber?: string;
  email?: string;
  address?: string;
  comment?: string;
  operator?: string;
  dateTime?: Date;
}

export class SaleOrderDto {
  id?: string;
  number?: string;
  customerNumber?: string;
  customerName?: string;
  consignorContact?: string;
  consignor?: string;
  sourceAddress?: string;
  destAddress?: string;
  warehouseNumber?: string;
  warehouseName?: string;
  consignee?: string;
  consigneeContact?: string;
  freight?: number;
  totalAmount?: number;
  totalQuantity?: number;
  amountPaid?: number;
  payWay?: string;
  status?: SaleOrderStatus;
  comment?: string;
  operator?: string;
  dateTime?: Date;
  items?: SaleOrderItemDto[];
}

export class SaleOrderItemDto {
  id?: string;
  saleOrderId?: string;
  amount?: number;
  number?: string;
  name?: string;
  type?: string;
  spec?: string;
  brand?: string;
  unit?: string;
  price?: number;
  comment?: string;
}

export class CustomerQueryDto {
  number?: string;
  name?: string;
  type?: string;
  rangeDate?: Date[];
}

export class ProductQueryDto {
  number?: string;
  name?: string;
  type?: string;
  rangeDate?: Date[];
}

export class SaleOrderQueryDto {
  number?: string;
  customerName?: string;
  customerNumber?: string;
  rangeDate?: Date[];
}

export class NoticeDto {
  id?: string;
  author?: string;
  content?: string;
  dateTime?: Date;
  isRead?: boolean;
}

export enum TemplateType {
  purchaseOrder = 0,
  saleOrder = 1,
  inboundReceipt = 2,
  outboundReceipt = 3,
  transferOrder = 4
}

export class PrintTemplateDto {
  id?: string;
  name?: string;
  path?: string;
  comment?: string;
  type?: TemplateType;
  script?: string;
  dateTime?: Date;
}

export class FileTemplateDto {
  id?: string;
  name?: string;
  path?: string;
  comment?: string;
  type?: TemplateType;
  dateTime?: Date;
}

export enum StatisticsPeriod {
  month = 0,
  year = 1
}

export class ColumnChartDto {
  data?: ColumnDto[];
}

export class ColumnDto {
  xpos?: string;
  ypos?: string;
}

export class PieChartDto {
  data?: PieDto[];
}

export class PieDto {
  name?: string;
  ratio?: number;
}

export class SimpleWarehouseDto {
  id?: string;
  name?: string;
}

