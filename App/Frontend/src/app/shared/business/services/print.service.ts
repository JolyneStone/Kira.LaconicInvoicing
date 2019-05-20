import { TemplateType } from '../app.model';
import { _HttpClient } from '@delon/theme';
import { Injectable } from '@angular/core';
import { AjaxResult } from '@shared/osharp/osharp.model';

@Injectable()
export class PrintService {

  constructor(
    private http: _HttpClient) {
  }

  getPrintTemplateScript(path: string) {
    return this.http.get<AjaxResult>('api/print/print/getprinttemplatescript', { path: path });
  }

  getAllByType(type: TemplateType) {
    return this.http.get<AjaxResult>('api/print/print/getallbytype', { type: type });
  }

  getPrintData(id: string, type: TemplateType) {
    let url: string;
    switch (type) {
      case TemplateType.purchaseOrder:
        url = 'api/purchase/purchaseorder/getprintdata';
        break;
      case TemplateType.inboundReceipt:
        url = 'api/wasehouse/inboundreceipt/getprintdata';
        break;
      case TemplateType.outboundReceipt:
        url = 'api/warehouse/outboundreceipt/getprintdata';
        break;
      case TemplateType.transferOrder:
        url = 'api/warehouse/transferorder/getprintdata';
        break;
      case TemplateType.saleOrder:
        url = 'api/sale/saleorder/getprintdata';
        break;
      default:
        url = 'api/purchase/purchaseorder/getprintdata';
        break;
    }

    return this.http.get<AjaxResult>(url, { id: id });
  }
}
