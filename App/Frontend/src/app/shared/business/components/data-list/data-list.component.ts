import { NzMessageService } from 'ng-zorro-antd';
import { QueryCondition, SortDescription } from '../../app.model';
import { _HttpClient } from '@delon/theme';
import { Component, OnInit, ChangeDetectorRef, Injector, Input } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { AuthConfig, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';
import { VendorDto } from '@shared/business/app.model';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-data-list',
  templateUrl: './data-list.component.html',
  styles: []
})
export class DataListComponent implements OnInit {
  constructor(
    private cdr: ChangeDetectorRef,
  ) {
  }

  data: any[] = [];
  columns: Array<{ name: string, get: (data: any) => any }>;
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
  queryCondition: QueryCondition;
  @Input() read: (query: QueryCondition) => void;
  @Input() cancel: () => void;
  @Input() submit?: () => void;
  @Input() searchAll?: () => void;

  ngOnInit() {
  }

  loadData(
    queryCondition: QueryCondition,
    columns: Array<{ name: string, get: (data: any) => any }>) {
    this.queryCondition = queryCondition;
    this.columns = columns;
    this.search();
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
    // this.http.get(this.urls.search, this.queryCondition)
    //   .subscribe((res: AjaxResult) => {
    //     if (res.type === AjaxResultType.success) {
    //       this.data = res.data.rows;
    //     } else if (res.content) {
    //       this.msg.error(res.content);
    //     }
    //   });
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
}
