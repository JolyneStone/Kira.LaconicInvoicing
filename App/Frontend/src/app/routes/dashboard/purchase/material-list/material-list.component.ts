import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { Component, OnInit, ChangeDetectorRef, Injector, ViewChild } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { PurchaseService } from '@shared/business/services/purchase.service';
import { SortDescription, MaterialDto, MaterialQueryDto } from '@shared/business/app.model';
import { AuthConfig, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';
import { MaterialEditComponent } from '../material-edit/material-edit.component';

@Component({
  selector: 'app-material-list',
  templateUrl: './material-list.component.html',
  styles: []
})
export class MaterialListComponent extends ComponentBase implements OnInit {

  constructor(
    private msg: NzMessageService,
    private cdr: ChangeDetectorRef,
    private bsr: BusinessService,
    private purchaseService: PurchaseService,
    injector: Injector,
  ) {
    super(injector);
    this.queryData = { number: '', name: '', type: '' };
    this.materialDtoList = [];
  }
  @ViewChild('materialEdit') materialEdit: MaterialEditComponent;
  materialDtoList: MaterialDto[];
  queryData: MaterialQueryDto;
  loading = false;
  expandForm = false;
  types: any[];
  selectedRows: any[] = [];
  mapOfCheckedId: Map<string, boolean> = new Map<string, boolean>(); //{ [key: string]: boolean } = {};
  hasSelected = false;
  isIndeterminate = false;
  isAllDisplayDataChecked = false;
  pageSize = 20;
  pageIndex = 1;
  total = 0;
  sorts: SortDescription[] = [
    { field: 'Name', order: null },
    { field: 'Number', order: null },
    { field: 'Type', order: null },
    { field: 'Datetime', order: null }
  ];

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.Purchase.Purchase.Material', [
      'Search',
      'Add',
      'Get',
      'Delete'
    ]);
  }

  async ngOnInit() {
    await super.checkAuth();
    this.bsr.getBaseDataValuesByType('MATERIALTYPE').subscribe(res => {
      this.types = res;
      this.cdr.detectChanges();
    });

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
      this.purchaseService.searchMaterial(this.queryData, this.sorts, this.pageIndex, this.pageSize)
        .subscribe((res: AjaxResult) => {
          if (res.type === AjaxResultType.success) {
            this.materialDtoList = res.data.rows;
            this.total = res.data.total;
          } else if (res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }

  refresh() {
    this.pageIndex = 1;
    this.pageSize = 20;
    this.queryData = { number: '', name: '', type: '' };
    this.search();
  }

  reset() {
    this.queryData = { number: '', name: '', type: '' };
  }

  currentPageDataChange($event: MaterialDto[]) {
    this.materialDtoList = $event;
    this.refreshStatus();
  }

  pageIndexChange() {
    this.search();
  }

  checkAll(value: boolean): void {
    this.materialDtoList.forEach(item => (this.mapOfCheckedId[item.id] = value));
    this.refreshStatus();
  }

  refreshStatus(): void {
    this.hasSelected = this.hasRowSelected();
    this.isAllDisplayDataChecked = this.materialDtoList.every(item => this.mapOfCheckedId[item.id]);
    this.isIndeterminate =
      this.materialDtoList.some(item => this.mapOfCheckedId[item.id]) && !this.isAllDisplayDataChecked;
  }

  add() {
    this.materialEdit.show('Add');
  }

  edit(data: MaterialDto) {
    this.purchaseService.getMaterial(data.id)
      .subscribe(res => {
        if (res.type === AjaxResultType.success) {
          this.materialEdit.show('Update', res.data);
        } else if (res.content) {
          this.msg.error(res.content);
        }
      });
  }

  delete() {
    const ids = this.materialDtoList.filter(v => this.mapOfCheckedId[v.id]).map(v => v.id);
    if (!ids || ids.length === 0) {
      this.msg.warning('请选择要操作的数据');
    } else {
      this.purchaseService.deleteMaterial(ids)
        .subscribe((res: any) => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('删除成功');
          } else if (res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }
}
