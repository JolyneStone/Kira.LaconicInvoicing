import { PurchaseOrderDto, PurchaseOrderQueryDto } from './../app.model';
import { VendorQueryDto, QueryCondition, QueryConditionItem, VendorDto, MaterialQueryDto, MaterialDto } from '../app.model';
import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { _HttpClient } from '@delon/theme';
import { AjaxResult } from '@shared/osharp/osharp.model';
import { SortDescription } from '../app.model';


@Injectable()
export class PurchaseService {
  private http: _HttpClient;

  constructor(injector: Injector) {
    this.http = injector.get(_HttpClient);
  }

  getVendorNewNumber(): Observable<AjaxResult> {
    return this.http.get<AjaxResult>('api/purchase/vendor/getnewnumber');
  }

  searchVendor(queryDto?: VendorQueryDto, sorts?: SortDescription[], pageIndex?: number, pageSize?: number): Observable<AjaxResult> {
    let queryCondition: QueryCondition = null;
    if (queryDto !== null) {
      queryCondition = new QueryCondition();
      if (queryDto.name) {
        queryCondition.filters.push({ field: 'Name', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.name });
      }
      if (queryDto.number) {
        queryCondition.filters.push({ field: 'Number', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.number });
      }
      if (queryDto.type) {
        queryCondition.filters.push({ field: 'Type', conditionOp: 'Equal', logicOp: 'AndAlso', value: queryDto.type });
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

    return this.http.post<AjaxResult>('api/purchase/vendor/search', queryCondition);
  }

  getVendor(id: string) {
    return this.http.get<AjaxResult>('api/purchase/vendor/get', { id: id });
  }

  addVendor(dto: VendorDto) {
    return this.http.post<AjaxResult>('api/purchase/vendor/add', dto);
  }

  updateVendor(dto: VendorDto) {
    return this.http.post<AjaxResult>('api/purchase/vendor/update', dto);
  }

  deleteVendor(ids: string[]) {
    return this.http.post('api/purchase/vendor/delete', ids);
  }

  getMaterialsByVendor(query: QueryCondition) {
    return this.http.post<AjaxResult>('api/purchase/vendor/getmaterials', query);
  }

  updateMaterialsByVendor(id: string, materialIds: string[]) {
    return this.http.post<AjaxResult>('api/purchase/vendor/updatematerials', { id: id, ids: materialIds });
  }


  getMaterialNewNumber(): Observable<AjaxResult> {
    return this.http.get<AjaxResult>('api/purchase/material/getnewnumber');
  }

  searchMaterial(queryDto?: MaterialQueryDto, sorts?: SortDescription[], pageIndex?: number, pageSize?: number): Observable<AjaxResult> {
    let queryCondition: QueryCondition = null;
    if (queryDto !== null) {
      queryCondition = new QueryCondition();
      if (queryDto.name) {
        queryCondition.filters.push({ field: 'Name', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.name });
      }
      if (queryDto.number) {
        queryCondition.filters.push({ field: 'Number', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.number });
      }
      if (queryDto.type) {
        queryCondition.filters.push({ field: 'Type', conditionOp: 'Equal', logicOp: 'AndAlso', value: queryDto.type });
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

    return this.http.post<AjaxResult>('api/purchase/material/search', queryCondition);
  }

  getMaterial(id: string) {
    return this.http.get<AjaxResult>('api/purchase/material/get', { id: id });
  }

  addMaterial(dto: MaterialDto) {
    return this.http.post<AjaxResult>('api/purchase/material/add', dto);
  }

  updateMaterial(dto: MaterialDto) {
    return this.http.post<AjaxResult>('api/purchase/material/update', dto);
  }

  deleteMaterial(ids: string[]) {
    return this.http.post('api/purchase/material/delete', ids);
  }

  getVendorsByMaterial(query: QueryCondition) {
    return this.http.post<AjaxResult>('api/purchase/material/getvendors', query);
  }

  updateVendorsByMaterial(id: string, vendorIds: string[]) {
    return this.http.post<AjaxResult>('api/purchase/material/updatevendors', { id: id, Ids: vendorIds });
  }


  getPurchaseOrderNewNumber(): Observable<AjaxResult> {
    return this.http.get<AjaxResult>('api/purchase/purchaseorder/getnewnumber');
  }

  searchPurchaseOrder(queryDto?: PurchaseOrderQueryDto, sorts?: SortDescription[], pageIndex?: number, pageSize?: number): Observable<AjaxResult> {
    let queryCondition: QueryCondition = null;
    if (queryDto !== null) {
      queryCondition = new QueryCondition();
      if (queryDto.number) {
        queryCondition.filters.push({ field: 'Number', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.number });
      }
      if (queryDto.vendorName) {
        queryCondition.filters.push({ field: 'VendorName', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.vendorName });
      }
      if (queryDto.vendorNumber) {
        queryCondition.filters.push({ field: 'VendorNumber', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.vendorNumber });
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

    return this.http.post<AjaxResult>('api/purchase/purchaseorder/search', queryCondition);
  }

  getPurchaseOrder(id: string) {
    return this.http.get<AjaxResult>('api/purchase/purchaseorder/get', { id: id });
  }

  addPurchaseOrder(dto: PurchaseOrderDto) {
    return this.http.post<AjaxResult>('api/purchase/purchaseorder/add', dto);
  }

  updatePurchaseOrder(dto: PurchaseOrderDto) {
    return this.http.post<AjaxResult>('api/purchase/purchaseorder/update', dto);
  }

  deletePurchaseOrder(ids: string[]) {
    return this.http.post('api/purchase/purchaseorder/delete', ids);
  }
}