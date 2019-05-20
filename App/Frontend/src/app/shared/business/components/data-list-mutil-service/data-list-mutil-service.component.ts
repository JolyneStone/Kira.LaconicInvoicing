import { NzModalRef } from 'ng-zorro-antd';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { Component, OnInit, ChangeDetectorRef, Input, Injector } from '@angular/core';
import { QueryCondition } from '@shared/business/app.model';
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-data-list-mutil-service',
  templateUrl: './data-list-mutil-service.component.html',
  styles: []
})
export class DataListMutilServiceComponent extends ComponentBase implements OnInit {
  constructor(
    private cdr: ChangeDetectorRef,
    private modal: NzModalRef,
    injector: Injector,
  ) {
    super(injector);
  }

  data: any[] = [];
  loading = false;
  hasLoad = false;
  selectedRows: any[] = [];
  mapOfCheckedId: Map<string, boolean> = new Map<string, boolean>();
  hasSelected = false;
  isIndeterminate = false;
  isAllDisplayDataChecked = false;
  pageSize = 5;
  pageIndex = 1;
  total = 0;
  @Input() serverAuthConfig: AuthConfig;
  @Input() read: (query: QueryCondition) => void;
  @Input() columns: Array<{ name: string, get: (data: any) => any }>;
  @Input() queryCondition: QueryCondition = new QueryCondition();

  async ngOnInit() {
    await this.checkAuth();
    this.search();
  }

  AuthConfig() {
    return this.serverAuthConfig;
  }

  refresh() {
    this.pageIndex = 1;
    this.pageSize = 5;
    this.search();
  }

  pageIndexChange() {
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
    this.refreshStatus();
  }

  checkAll(value: boolean): void {
    this.data.forEach(item => (this.mapOfCheckedId[item.id] = value));
    this.refreshStatus();
  }

  refreshStatus(): void {
    this.hasSelected = this.hasRowSelected();
    this.isAllDisplayDataChecked = this.data.every(item => this.mapOfCheckedId[item.id]) || false;
    this.isIndeterminate =
    this.data.some(item => this.mapOfCheckedId[item.id]) && !this.isAllDisplayDataChecked;
  }

  hasRowSelected(): boolean {
    for (const v of this.mapOfCheckedId.values()) {
      if (v === true)
        return true;
    }
    return false;
  }

  close() {
    this.modal.destroy();
  }
}
