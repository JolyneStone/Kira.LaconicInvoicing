import { Injectable, Injector } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { Observable } from 'rxjs';
import { AjaxResult } from '@shared/osharp/osharp.model';
import { CustomerQueryDto, SortDescription, QueryCondition, CustomerDto, ProductQueryDto, ProductDto, SaleOrderQueryDto, SaleOrderDto } from '../app.model';

@Injectable()
export class SaleService {
  private http: _HttpClient;

  constructor(injector: Injector) {
    this.http = injector.get(_HttpClient);
  }

  getCustomerNewNumber(): Observable<AjaxResult> {
    return this.http.get<AjaxResult>('api/sale/customer/getnewnumber');
  }

  searchCustomer(queryDto?: CustomerQueryDto, sorts?: SortDescription[], pageIndex?: number, pageSize?: number): Observable<AjaxResult> {
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

    return this.http.post<AjaxResult>('api/sale/customer/search', queryCondition);
  }

  getCustomer(id: string) {
    return this.http.get<AjaxResult>('api/sale/customer/get', { id: id });
  }

  addCustomer(dto: CustomerDto) {
    return this.http.post<AjaxResult>('api/sale/customer/add', dto);
  }

  updateCustomer(dto: CustomerDto) {
    return this.http.post<AjaxResult>('api/sale/customer/update', dto);
  }

  deleteCustomer(ids: string[]) {
    return this.http.post('api/sale/customer/delete', ids);
  }

  getProductsByCustomer(query: QueryCondition) {
    return this.http.post<AjaxResult>('api/sale/customer/getproducts', query);
  }

  updateProductsByCustomer(id: string, productIds: string[]) {
    return this.http.post<AjaxResult>('api/sale/customer/updateproducts', { id: id, ids: productIds });
  }


  getProductNewNumber(): Observable<AjaxResult> {
    return this.http.get<AjaxResult>('api/sale/product/getnewnumber');
  }

  searchProduct(queryDto?: ProductQueryDto, sorts?: SortDescription[], pageIndex?: number, pageSize?: number): Observable<AjaxResult> {
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

    return this.http.post<AjaxResult>('api/sale/product/search', queryCondition);
  }

  getProduct(id: string) {
    return this.http.get<AjaxResult>('api/sale/product/get', { id: id });
  }

  addProduct(dto: ProductDto) {
    return this.http.post<AjaxResult>('api/sale/product/add', dto);
  }

  updateProduct(dto: ProductDto) {
    return this.http.post<AjaxResult>('api/sale/product/update', dto);
  }

  deleteProduct(ids: string[]) {
    return this.http.post('api/sale/product/delete', ids);
  }

  getCustomersByProduct(query: QueryCondition) {
    return this.http.post<AjaxResult>('api/sale/product/getcustomers', query);
  }

  updateCustomersByProduct(id: string, customerIds: string[]) {
    return this.http.post<AjaxResult>('api/sale/product/updatecustomers', { id: id, Ids: customerIds });
  }


  getSaleOrderNewNumber(): Observable<AjaxResult> {
    return this.http.get<AjaxResult>('api/sale/saleorder/getnewnumber');
  }

  searchSaleOrder(queryDto?: SaleOrderQueryDto, sorts?: SortDescription[], pageIndex?: number, pageSize?: number): Observable<AjaxResult> {
    let queryCondition: QueryCondition = null;
    if (queryDto !== null) {
      queryCondition = new QueryCondition();
      if (queryDto.number) {
        queryCondition.filters.push({ field: 'Number', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.number });
      }
      if (queryDto.customerName) {
        queryCondition.filters.push({ field: 'CustomerName', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.customerName });
      }
      if (queryDto.customerNumber) {
        queryCondition.filters.push({ field: 'CustomerNumber', conditionOp: 'StartsWith', logicOp: 'AndAlso', value: queryDto.customerNumber });
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

    return this.http.post<AjaxResult>('api/sale/saleorder/search', queryCondition);
  }

  getSaleOrder(id: string) {
    return this.http.get<AjaxResult>('api/sale/saleorder/get', { id: id });
  }

  addSaleOrder(dto: SaleOrderDto) {
    return this.http.post<AjaxResult>('api/sale/saleorder/add', dto);
  }

  updateSaleOrder(dto: SaleOrderDto) {
    return this.http.post<AjaxResult>('api/sale/saleorder/update', dto);
  }

  deleteSaleOrder(ids: string[]) {
    return this.http.post('api/sale/saleorder/delete', ids);
  }
}