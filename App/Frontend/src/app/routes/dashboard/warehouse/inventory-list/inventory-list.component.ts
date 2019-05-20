import { I18NService } from '@core/i18n/i18n.service';
import { WarehouseDto } from './../../../../shared/business/app.model';
import { Component, OnInit, ChangeDetectorRef, Injector, ViewChild } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { WarehouseService } from '@shared/business/services/warehouse.service';
import { InventoryEditComponent } from '../inventory-edit/inventory-edit.component';
import { InventoryDto, InventoryQueryDto, SortDescription } from '@shared/business/app.model';
import { AuthConfig, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';
import { GoodsCategory } from '../../../../shared/business/app.model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-inventory-list',
  templateUrl: './inventory-list.component.html',
  styles: []
})
export class InventoryListComponent extends ComponentBase implements OnInit {

  constructor(
    private msg: NzMessageService,
    private i18nService: I18NService,
    private warehouseService: WarehouseService,
    private route: ActivatedRoute,
    injector: Injector,
  ) {
    super(injector);
    this.queryData = { number: '', name: '', goodsCategory: null };
    this.inventoryDtoList = [];
  }
  @ViewChild('inventoryEdit') inventoryEdit: InventoryEditComponent;
  inventoryDtoList: InventoryDto[];
  queryData: InventoryQueryDto;
  warehouseDto: WarehouseDto;
  loading = false;
  expandForm = false;
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
    { field: 'GoodsCategory', order: null },
    { field: 'Datetime', order: null }
  ];
  goodsCategoryKeys: any[];
  goodsCategory: typeof GoodsCategory = GoodsCategory;

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.Warehouse.Warehouse.Inventory', [
      'Search',
      'Add',
      'Get',
      'Delete'
    ]);
  }

  async ngOnInit() {
    this.goodsCategoryKeys = Object.keys(this.goodsCategory).filter(f => !isNaN(Number(f)));
    this.route.params.subscribe((params) => {
      if (params && params['id']) {
        this.warehouseService.getWarehouse(params['id'])
          .subscribe(res => {
            if (res && res.type === AjaxResultType.success) {
              this.warehouseDto = res.data;
              this.queryData.warehouseId = this.warehouseDto.id;
              this.search();
            } else if (res && res.content) {
              this.msg.error(res.content);
            }
          });
      }
    });

    await super.checkAuth();
  }

  getGoodsCategoryName(category: GoodsCategory) {
    return this.i18nService.fanyi('app.dashboard.goods-category-' + (category === GoodsCategory.material ? 'material' : 'product'));
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
      this.warehouseService.searchInventory(this.queryData, this.sorts, this.pageIndex, this.pageSize)
        .subscribe((res: AjaxResult) => {
          if (res.type === AjaxResultType.success) {
            this.inventoryDtoList = res.data.rows;
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
    this.queryData = { number: '', name: '', goodsCategory: null, warehouseId: this.warehouseDto ? this.warehouseDto.id : null };
    this.search();
  }

  reset() {
    this.queryData = { number: '', name: '', goodsCategory: null, warehouseId: this.warehouseDto ? this.warehouseDto.id : null };
  }

  currentPageDataChange($event: InventoryDto[]) {
    this.inventoryDtoList = $event;
    this.refreshStatus();
  }

  pageIndexChange() {
    this.search();
  }

  checkAll(value: boolean): void {
    this.inventoryDtoList.forEach(item => (this.mapOfCheckedId[item.id] = value));
    this.refreshStatus();
  }

  refreshStatus(): void {
    this.hasSelected = this.hasRowSelected();
    this.isAllDisplayDataChecked = this.inventoryDtoList.every(item => this.mapOfCheckedId[item.id]);
    this.isIndeterminate =
      this.inventoryDtoList.some(item => this.mapOfCheckedId[item.id]) && !this.isAllDisplayDataChecked;
  }

  add() {
    this.inventoryEdit.show('Add', { warehouseId: this.warehouseDto.id });
  }

  edit(data: InventoryDto) {
    this.warehouseService.getInventory(data.id)
      .subscribe(res => {
        if (res.type === AjaxResultType.success) {
          this.inventoryEdit.show('Update', res.data);
        } else if (res.content) {
          this.msg.error(res.content);
        }
      });
  }

  delete() {
    const ids = this.inventoryDtoList.filter(v => this.mapOfCheckedId[v.id]).map(v => v.id);
    if (!ids || ids.length === 0) {
      this.msg.warning('请选择要操作的数据');
    } else {
      this.warehouseService.deleteInventory(ids)
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
