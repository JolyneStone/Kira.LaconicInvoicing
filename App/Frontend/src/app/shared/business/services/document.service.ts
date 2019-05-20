import { Injectable } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { TemplateType } from '../app.model';
import { AjaxResult } from '@shared/osharp/osharp.model';

@Injectable()
export class DocumentService {

  constructor(
    private http: _HttpClient) {
  }

  getAllByType(type: TemplateType) {
    return this.http.get<AjaxResult>('api/file/file/getallbytype', { type: type });
  }

  export(id: string, templateId: string, type: TemplateType) {
    return this.http.get<AjaxResult>('api/file/file/exportdocument', { id: id, templateId: templateId, type: type });
  }
}
