import { TemplateType } from '../app.model';
import { _HttpClient } from '@delon/theme';
import { Injectable } from '@angular/core';
import { PrintTemplateDto } from '../app.model';
import { AjaxResult } from '@shared/osharp/osharp.model';

@Injectable()
export class PrintManagementService {

  constructor(
    private http: _HttpClient) {
  }

  add(dto: PrintTemplateDto) {
    return this.http.post<AjaxResult>('api/print/printmanagement/add', dto);
  }

  update(dto: PrintTemplateDto) {
    return this.http.post<AjaxResult>('api/print/printmanagement/update', dto);
  }

  delete(id: string) {
    return this.http.delete('api/print/printmanagement/delete', { id: id });
  }

  get(id: string) {
    return this.http.get<AjaxResult>('api/print/printmanagement/get', { id: id });
  }

  getAll() {
    return this.http.get<AjaxResult>('api/print/printmanagement/getall');
  }

  getPrintTemplateScript(path: string) {
    return this.http.get<AjaxResult>('api/print/printmanagement/getprinttemplatescript', { path: path });
  }

  getAllByType(type: TemplateType) {
    return this.http.get<AjaxResult>('api/print/printmanagement/getallbytype', { type: type });
  }
}
