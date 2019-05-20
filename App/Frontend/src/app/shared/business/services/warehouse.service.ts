import { Injectable, Injector } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { AjaxResult } from '@shared/osharp/osharp.model';
import { SortDescription, QueryCondition, WarehouseDto, WarehouseQueryDto, InboundReceiptQueryDto, InboundReceiptDto, OutboundReceiptQueryDto, OutboundReceiptDto, InventoryQueryDto, InventoryDto, TransferOrderQueryDto, TransferOrderDto, GoodsCategory } from '../app.model';
import { Observable } from 'rxjs';

@Injectable()
export class WarehouseService {
  private http: _HttpClient;

  constructor(injector: Injector) {
    this.http = injector.get(_HttpClient);
  }

  getWarehouseNewNumber(): Observable<AjaxResult> {
    return this.http.get<AjaxResult>('api/warehouse/warehouse/getnewnumber');
  }

  searchWarehouse(queryDto?: WarehouseQueryDto, sorts?: SortDescription[], pageIndex?: number, pageSize?: number): Observable<AjaxResult> {
    let queryCondition: QueryCondition = null;
    if (queryDto !== null) {
      queryCondition = new QueryCondition();
      if (queryDto.number) {
        queryCondition.filters.push({ field: 'Number', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.number });
      }
      if (queryDto.name) {
        queryCondition.filters.push({ field: 'Name', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.name });
      }
      if (queryDto.address) {
        queryCondition.filters.push({ field: 'Address', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.address });
      }
      if (queryDto.rangeDate && queryDto.rangeDate.length > 0) {
        if (queryDto.rangeDate[0])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'GreaterThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[0].toLocaleDateString() });
        if (queryDto.rangeDate.length >= 1 && queryDto.rangeDate[1])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'LessThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[1].toLocaleDateString() });
      }
    }

    if (sorts !== null) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.sorts = sorts;
    }

    if (pageIndex) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageIndex = pageIndex;
    }

    if (pageSize) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageSize = pageSize;
    }

    return this.http.post<AjaxResult>('api/warehouse/warehouse/search', queryCondition);
  }

  getWarehouse(id: string) {
    return this.http.get<AjaxResult>('api/warehouse/warehouse/get', { id: id });
  }

  addWarehouse(dto: WarehouseDto) {
    return this.http.post<AjaxResult>('api/warehouse/warehouse/add', dto);
  }

  updateWarehouse(dto: WarehouseDto) {
    return this.http.post<AjaxResult>('api/warehouse/warehouse/update', dto);
  }

  deleteWarehouse(ids: string[]) {
    return this.http.post('api/warehouse/warehouse/delete', ids);
  }

  getInboundReceiptNewNumber(): Observable<AjaxResult> {
    return this.http.get<AjaxResult>('api/warehouse/inboundreceipt/getnewnumber');
  }

  searchInboundReceipt(queryDto?: InboundReceiptQueryDto, sorts?: SortDescription[], pageIndex?: number, pageSize?: number): Observable<AjaxResult> {
    let queryCondition: QueryCondition = null;
    if (queryDto !== null) {
      queryCondition = new QueryCondition();
      if (queryDto.number) {
        queryCondition.filters.push({ field: 'Number', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.number });
      }
      if (queryDto.supplier) {
        queryCondition.filters.push({ field: 'Supplier', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.supplier });
      }
      if (queryDto.supplierNo) {
        queryCondition.filters.push({ field: 'SupplierNo', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.supplierNo });
      }
      if (queryDto.warehouseName) {
        queryCondition.filters.push({ field: 'WarehouseName', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.warehouseName });
      }
      if (queryDto.warehouseNumber) {
        queryCondition.filters.push({ field: 'WarehouseNumber', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.warehouseNumber });
      }
      if (queryDto.rangeDate && queryDto.rangeDate.length > 0) {
        if (queryDto.rangeDate[0])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'GreaterThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[0].toLocaleDateString() });
        if (queryDto.rangeDate.length >= 1 && queryDto.rangeDate[1])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'LessThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[1].toLocaleDateString() });
      }
    }

    if (sorts !== null) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.sorts = sorts;
    }

    if (pageIndex) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageIndex = pageIndex;
    }

    if (pageSize) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageSize = pageSize;
    }

    return this.http.post<AjaxResult>('api/warehouse/inboundreceipt/search', queryCondition);
  }

  getInboundReceipt(id: string) {
    return this.http.get<AjaxResult>('api/warehouse/inboundreceipt/get', { id: id });
  }

  addInboundReceipt(dto: InboundReceiptDto) {
    return this.http.post<AjaxResult>('api/warehouse/inboundreceipt/add', dto);
  }

  updateInboundReceipt(dto: InboundReceiptDto) {
    return this.http.post<AjaxResult>('api/warehouse/inboundreceipt/update', dto);
  }

  deleteInboundReceipt(ids: string[]) {
    return this.http.post('api/warehouse/inboundreceipt/delete', ids);
  }

  getOutboundReceiptNewNumber(): Observable<AjaxResult> {
    return this.http.get<AjaxResult>('api/warehouse/outboundreceipt/getnewnumber');
  }

  searchOutboundReceipt(queryDto?: OutboundReceiptQueryDto, sorts?: SortDescription[], pageIndex?: number, pageSize?: number): Observable<AjaxResult> {
    let queryCondition: QueryCondition = null;
    if (queryDto !== null) {
      queryCondition = new QueryCondition();
      if (queryDto.number) {
        queryCondition.filters.push({ field: 'Number', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.number });
      }
      if (queryDto.receiver) {
        queryCondition.filters.push({ field: 'Receiver', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.receiver });
      }
      if (queryDto.receiverNo) {
        queryCondition.filters.push({ field: 'ReceiverNo', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.receiverNo });
      }
      if (queryDto.warehouseName) {
        queryCondition.filters.push({ field: 'WarehouseName', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.warehouseName });
      }
      if (queryDto.warehouseNumber) {
        queryCondition.filters.push({ field: 'WarehouseNumber', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.warehouseNumber });
      }
      if (queryDto.rangeDate && queryDto.rangeDate.length > 0) {
        if (queryDto.rangeDate[0])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'GreaterThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[0].toLocaleDateString() });
        if (queryDto.rangeDate.length >= 1 && queryDto.rangeDate[1])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'LessThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[1].toLocaleDateString() });
      }
    }

    if (sorts !== null) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.sorts = sorts;
    }

    if (pageIndex) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageIndex = pageIndex;
    }

    if (pageSize) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageSize = pageSize;
    }

    return this.http.post<AjaxResult>('api/warehouse/outboundreceipt/search', queryCondition);
  }

  getOutboundReceipt(id: string) {
    return this.http.get<AjaxResult>('api/warehouse/outboundreceipt/get', { id: id });
  }

  addOutboundReceipt(dto: OutboundReceiptDto) {
    return this.http.post<AjaxResult>('api/warehouse/outboundreceipt/add', dto);
  }

  updateOutboundReceipt(dto: OutboundReceiptDto) {
    return this.http.post<AjaxResult>('api/warehouse/outboundreceipt/update', dto);
  }

  deleteOutboundReceipt(ids: string[]) {
    return this.http.post('api/warehouse/outboundreceipt/delete', ids);
  }

  searchInventory(queryDto?: InventoryQueryDto, sorts?: SortDescription[], pageIndex?: number, pageSize?: number): Observable<AjaxResult> {
    let queryCondition: QueryCondition = null;
    if (queryDto !== null) {
      queryCondition = new QueryCondition();
      if (queryDto.warehouseId) {
        queryCondition.filters.push({ field: 'WarehouseId', conditionOp: 'Equal', logicOp: 'AndAlso', value: queryDto.warehouseId });
      }
      if (queryDto.number) {
        queryCondition.filters.push({ field: 'Number', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.number });
      }
      if (queryDto.goodsCategory) {
        queryCondition.filters.push({ field: 'GoodsCategory', conditionOp: 'Equal', logicOp: 'AndAlso', value: queryDto.goodsCategory });
      }
      if (queryDto.name) {
        queryCondition.filters.push({ field: 'Name', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.name });
      }
      if (queryDto.rangeDate && queryDto.rangeDate.length > 0) {
        if (queryDto.rangeDate[0])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'GreaterThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[0].toLocaleDateString() });
        if (queryDto.rangeDate.length >= 1 && queryDto.rangeDate[1])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'LessThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[1].toLocaleDateString() });
      }
    }

    if (sorts !== null) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.sorts = sorts;
    }

    if (pageIndex) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageIndex = pageIndex;
    }

    if (pageSize) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageSize = pageSize;
    }

    return this.http.post<AjaxResult>('api/warehouse/inventory/search', queryCondition);
  }

  searchInventoryMaterial(queryDto?: InventoryQueryDto, sorts?: SortDescription[], pageIndex?: number, pageSize?: number): Observable<AjaxResult> {
    let queryCondition: QueryCondition = null;
    if (queryDto !== null) {
      queryCondition = new QueryCondition();
      queryCondition.filters.push({ field: 'GoodsCategory', conditionOp: 'Equal', logicOp: 'AndAlso', value: GoodsCategory.material });
      if (queryDto.warehouseId) {
        queryCondition.filters.push({ field: 'WarehouseId', conditionOp: 'Equal', logicOp: 'AndAlso', value: queryDto.warehouseId });
      }
      if (queryDto.number) {
        queryCondition.filters.push({ field: 'Number', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.number });
      }
      if (queryDto.name) {
        queryCondition.filters.push({ field: 'Name', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.name });
      }
      if (queryDto.rangeDate && queryDto.rangeDate.length > 0) {
        if (queryDto.rangeDate[0])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'GreaterThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[0].toLocaleDateString() });
        if (queryDto.rangeDate.length >= 1 && queryDto.rangeDate[1])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'LessThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[1].toLocaleDateString() });
      }
    }

    if (sorts !== null) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.sorts = sorts;
    }

    if (pageIndex) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageIndex = pageIndex;
    }

    if (pageSize) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageSize = pageSize;
    }

    return this.http.post<AjaxResult>('api/warehouse/inventory/searchmaterial', queryCondition);
  }

  searchInventoryProduct(queryDto?: InventoryQueryDto, sorts?: SortDescription[], pageIndex?: number, pageSize?: number): Observable<AjaxResult> {
    let queryCondition: QueryCondition = null;
    if (queryDto !== null) {
      queryCondition = new QueryCondition();
      queryCondition.filters.push({ field: 'GoodsCategory', conditionOp: 'Equal', logicOp: 'AndAlso', value: GoodsCategory.product });
      if (queryDto.warehouseId) {
        queryCondition.filters.push({ field: 'WarehouseId', conditionOp: 'Equal', logicOp: 'AndAlso', value: queryDto.warehouseId });
      }
      if (queryDto.number) {
        queryCondition.filters.push({ field: 'Number', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.number });
      }
      if (queryDto.name) {
        queryCondition.filters.push({ field: 'Name', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.name });
      }
      if (queryDto.rangeDate && queryDto.rangeDate.length > 0) {
        if (queryDto.rangeDate[0])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'GreaterThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[0].toLocaleDateString() });
        if (queryDto.rangeDate.length >= 1 && queryDto.rangeDate[1])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'LessThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[1].toLocaleDateString() });
      }
    }

    if (sorts !== null) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.sorts = sorts;
    }

    if (pageIndex) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageIndex = pageIndex;
    }

    if (pageSize) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageSize = pageSize;
    }

    return this.http.post<AjaxResult>('api/warehouse/inventory/searchproduct', queryCondition);
  }

  getInventory(id: string) {
    return this.http.get<AjaxResult>('api/warehouse/inventory/get', { id: id });
  }

  addInventory(dto: InventoryDto) {
    return this.http.post<AjaxResult>('api/warehouse/inventory/add', dto);
  }

  updateInventory(dto: InventoryDto) {
    return this.http.post<AjaxResult>('api/warehouse/inventory/update', dto);
  }

  deleteInventory(ids: string[]) {
    return this.http.post('api/warehouse/inventory/delete', ids);
  }

  getTransferOrderNewNumber(): Observable<AjaxResult> {
    return this.http.get<AjaxResult>('api/warehouse/transferorder/getnewnumber');
  }

  searchTransferOrder(queryDto?: TransferOrderQueryDto, sorts?: SortDescription[], pageIndex?: number, pageSize?: number): Observable<AjaxResult> {
    let queryCondition: QueryCondition = null;
    if (queryDto !== null) {
      queryCondition = new QueryCondition();
      if (queryDto.number) {
        queryCondition.filters.push({ field: 'Number', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.number });
      }
      if (queryDto.sourceWarehouseName) {
        queryCondition.filters.push({ field: 'SourceWarehouseName', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.sourceWarehouseName });
      }
      if (queryDto.sourceWarehouseNumber) {
        queryCondition.filters.push({ field: 'SourceWarehouseNumber', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.sourceWarehouseNumber });
      }
      if (queryDto.destWarehouseName) {
        queryCondition.filters.push({ field: 'DestWarehouseName', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.destWarehouseName });
      }
      if (queryDto.destWarehouseNumber) {
        queryCondition.filters.push({ field: 'DestWarehouseNumber', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.destWarehouseNumber });
      }
      if (queryDto.rangeDate && queryDto.rangeDate.length > 0) {
        if (queryDto.rangeDate[0])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'GreaterThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[0].toLocaleDateString() });
        if (queryDto.rangeDate.length >= 1 && queryDto.rangeDate[1])
          queryCondition.filters.push({ field: 'DateTime', conditionOp: 'LessThanOrEqual', logicOp: 'AndAlso', value: queryDto.rangeDate[1].toLocaleDateString() });
      }
    }

    if (sorts !== null) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.sorts = sorts;
    }

    if (pageIndex) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageIndex = pageIndex;
    }

    if (pageSize) {
      if (queryCondition === null) {
        queryCondition = new QueryCondition();
      }

      queryCondition.pageSize = pageSize;
    }

    return this.http.post<AjaxResult>('api/warehouse/transferorder/search', queryCondition);
  }

  getTransferOrder(id: string) {
    return this.http.get<AjaxResult>('api/warehouse/transferorder/get', { id: id });
  }

  addTransferOrder(dto: TransferOrderDto) {
    return this.http.post<AjaxResult>('api/warehouse/transferorder/add', dto);
  }

  updateTransferOrder(dto: TransferOrderDto) {
    return this.http.post<AjaxResult>('api/warehouse/transferorder/update', dto);
  }

  deleteTransferOrder(ids: string[]) {
    return this.http.post('api/warehouse/transferorder/delete', ids);
  }
}
