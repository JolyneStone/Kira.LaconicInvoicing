import { Component, OnInit, ChangeDetectorRef, Injector, Input, ViewChild, ElementRef } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzModalRef } from 'ng-zorro-antd';
import { AuthConfig } from '@shared/osharp/osharp.model';
import { QueryCondition } from '@shared/business/app.model';

@Component({
  selector: 'app-data-list-single-service',
  templateUrl: './data-list-single-service.component.html',
  styles: []
})
export class DataListSingleServiceComponent extends ComponentBase implements OnInit {
  constructor(
    private modal: NzModalRef,
    injector: Injector,
  ) {
    super(injector);
  }

  data: any[] = [];
  selectData: any = null;
  loading = false;
  hasLoad = false;
  isIndeterminate = false;
  isAllDisplayDataChecked = false;
  pageSize = 10;
  pageIndex = 1;
  total = 0;
  @Input() serverAuthConfig: AuthConfig;
  @Input() read: (query: QueryCondition) => void;
  @Input() columns: Array<{ name: string, get: (data: any) => any }>;
  @Input() queryCondition: QueryCondition = new QueryCondition();
  @ViewChild('tag', { read: ElementRef }) tagElement: ElementRef;

  async ngOnInit() {
    await this.checkAuth();
    this.tagElement.nativeElement.style.display = 'none';
    this.search();
  }

  AuthConfig() {
    return this.serverAuthConfig;
  }

  get selectName() {
    if (this.selectData && this.columns) {
      return this.columns[0].get(this.selectData);
    }
    return '';
  }

  refresh() {
    this.pageIndex = 1;
    this.pageSize = 5;
    this.search();
  }

  pageIndexChange(event: number) {
    this.pageIndex = event;
    this.search();
  }

  search() {
    this.queryCondition.sorts = null;
    this.queryCondition.filters = null;
    this.queryCondition.pageSize = this.pageSize;
    this.queryCondition.pageIndex = this.pageIndex;
    if (this.read)
      this.read(this.queryCondition);
  }

  currentPageDataChange($event: any[]) {
    this.data = $event;
  }

  selectRow(selectData: any) {
    this.selectData = selectData;
    this.tagElement.nativeElement.innerHTML = this.columns[0].get(this.selectData);
    this.tagElement.nativeElement.style.display = 'inline';
  }

  close() {
    this.modal.destroy();
  }
}
