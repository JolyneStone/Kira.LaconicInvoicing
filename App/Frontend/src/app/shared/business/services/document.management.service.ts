import { Injectable } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { AjaxResult } from '@shared/osharp/osharp.model';
import { TemplateType, FileTemplateDto } from '../app.model';

@Injectable()
export class DocumentManagementService {

  constructor(
    private http: _HttpClient) {
  }

  uploadTemplate(form: FormData) {
    return this.http.post<AjaxResult>('api/file/filemanagement/uploadtemplate', form);
  }

  add(dto: FileTemplateDto) {
    return this.http.post<AjaxResult>('api/file/filemanagement/add', dto);
  }

  update(dto: FileTemplateDto) {
    return this.http.post<AjaxResult>('api/file/filemanagement/update', dto);
  }

  delete(id: string) {
    return this.http.delete('api/file/filemanagement/delete', { id: id });
  }

  get(id: string) {
    return this.http.get<AjaxResult>('api/file/filemanagement/get', { id: id });
  }

  getAll() {
    return this.http.get<AjaxResult>('api/file/filemanagement/getall');
  }


  getAllByType(type: TemplateType) {
    return this.http.get<AjaxResult>('api/file/filemanagement/getallbytype', { type: type });
  }
}