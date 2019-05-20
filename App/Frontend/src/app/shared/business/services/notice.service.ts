import { _HttpClient } from '@delon/theme';
import { Injectable } from '@angular/core';
import { NoticeDto } from '../app.model';
import { AjaxResult } from '@shared/osharp/osharp.model';

@Injectable()
export class NoticeService {

  constructor(
    private http: _HttpClient) {
  }

  add(dto: NoticeDto) {
    return this.http.post<AjaxResult>('api/notification/notification/add', dto);
  }

  deleteByUser(id: string) {
    return this.http.delete('api/notification/notification/deletebyuser', { id: id });
  }

  clearAllByUser() {
    return this.http.delete('api/notification/notification/clearallbyuser');
  }

  delete(id: string) {
    return this.http.delete('api/notification/notification/delete', { id: id } );
  }

  clearAll() {
    return this.http.post<AjaxResult>('api/notification/notification/clearall');
  }

  getAllByUser(count: number, size: number) {
    return this.http.get<AjaxResult>('api/notification/notification/getallbyuser', { count: count, size: size });
  }

  getAll(count: number, size: number) {
    return this.http.get<AjaxResult>('api/notification/notification/getall', { count: count, size: size });
  }

  getAllUnRead() {
    return this.http.get<AjaxResult>('api/notification/notification/getallunread');
  }

  getAllUnReadCount() {
    return this.http.get<AjaxResult>('api/notification/notification/getallunreadcount');
  }

  readNotice(id: string) {
    return this.http.get<AjaxResult>('api/notification/notification/readNotice', { id: id });
  }
}
