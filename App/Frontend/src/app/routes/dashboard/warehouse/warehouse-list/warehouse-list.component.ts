import { Router } from '@angular/router';
import { WarehouseService } from './../../../../shared/business/services/warehouse.service';
import { Component, OnInit, ChangeDetectorRef, Injector, ViewChild } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService } from 'ng-zorro-antd';
import { WarehouseEditComponent } from '../warehouse-edit/warehouse-edit.component';
import { WarehouseDto, WarehouseQueryDto, SortDescription } from '@shared/business/app.model';
import { AuthConfig, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-warehouse-list',
  templateUrl: './warehouse-list.component.html',
  styles: []
})
export class WarehouseListComponent extends ComponentBase implements OnInit {

  constructor(
    private msg: NzMessageService,
    private warehouseService: WarehouseService,
    private router: Router,
    injector: Injector,
  ) {
    super(injector);
    this.queryData = { number: '', name: '', address: '' };
    this.warehouseDtoList = [];
  }
  @ViewChild('warehouseEdit') warehouseEdit: WarehouseEditComponent;
  warehouseDtoList: WarehouseDto[];
  queryData: WarehouseQueryDto;
  loading = false;
  expandForm = false;
  selectedRows: any[] = [];
  mapOfCheckedId: Map<string, boolean> = new Map<string, boolean>(); //{ [key: string]: boolean } = {};
  hasSelected = false;
  isIndeterminate = false;
  isAllDisplayDataChecked = false;
  pageSize = 10;
  pageIndex = 1;
  total = 0;
  sorts: SortDescription[] = [
    { field: 'Name', order: null },
    { field: 'Number', order: null },
    { field: 'Address', order: null },
    { field: 'Datetime', order: null }
  ];

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.Warehouse.Warehouse.Warehouse', [
      'Search',
      'Add',
      'Get',
      'Delete'
    ]);
  }

  async ngOnInit() {
    await super.checkAuth();
    this.search();
  }

  sort(sortName: string, order: string): void {
    if (this.sorts.some(s => s.field === sortName)) {
      this.sorts = this.sorts.map(s => {
        if (s.field === sortName) {
          s.order = order;
        }
        return s;
      });
    }

    this.search();
  }

  hasRowSelected(): boolean {
    for (const v of this.mapOfCheckedId.values()) {
      if (v === true)
        return true;
    }

    return false;
  }

  search() {
    if (this.auth.Search) {
      this.warehouseService.searchWarehouse(this.queryData, this.sorts, this.pageIndex, this.pageSize)
        .subscribe((res: AjaxResult) => {
          if (res.type === AjaxResultType.success) {
            this.warehouseDtoList = res.data.rows;
            this.total = res.data.total;
          } else if (res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }

  refresh() {
    this.pageIndex = 1;
    this.queryData = { number: '', name: '', address: '' };
    this.search();
  }

  reset() {
    this.queryData = { number: '', name: '', address: '' };
  }

  currentPageDataChange($event: WarehouseDto[]) {
    this.warehouseDtoList = $event;
    this.refreshStatus();
  }

  pageIndexChange(event: number) {
    this.pageIndex = event;
    this.search();
  }

  pageSizeChange(event: number) {
    this.pageSize = event;
  }

  checkAll(value: boolean): void {
    this.warehouseDtoList.forEach(item => (this.mapOfCheckedId[item.id] = value));
    this.refreshStatus();
  }

  refreshStatus(): void {
    this.hasSelected = this.hasRowSelected();
    this.isAllDisplayDataChecked = this.warehouseDtoList.every(item => this.mapOfCheckedId[item.id]);
    this.isIndeterminate =
      this.warehouseDtoList.some(item => this.mapOfCheckedId[item.id]) && !this.isAllDisplayDataChecked;
  }

  add() {
    this.warehouseEdit.show('Add');
  }

  edit(data: WarehouseDto) {
    this.warehouseService.getWarehouse(data.id)
      .subscribe(res => {
        if (res.type === AjaxResultType.success) {
          this.warehouseEdit.show('Update', res.data);
        } else if (res.content) {
          this.msg.error(res.content);
        }
      });
  }

  delete() {
    const ids = this.warehouseDtoList.filter(v => this.mapOfCheckedId[v.id]).map(v => v.id);
    if (!ids || ids.length === 0) {
      this.msg.warning('请选择要操作的数据');
    } else {
      this.warehouseService.deleteWarehouse(ids)
        .subscribe((res: any) => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('删除成功');
          } else if (res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }

  gotoInventory(id: string) {
    this.router.navigate(['dashboard', 'warehouse', 'inventorylist', id]);
  }
}
