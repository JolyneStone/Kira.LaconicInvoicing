import { Component, OnInit, ChangeDetectorRef, Injector, ViewChild } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { SaleService } from '@shared/business/services/sale.service';
import { ProductEditComponent } from '../product-edit/product-edit.component';
import { ProductDto, ProductQueryDto, SortDescription } from '@shared/business/app.model';
import { AuthConfig, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styles: []
})
export class ProductListComponent extends ComponentBase implements OnInit {

  constructor(
    private msg: NzMessageService,
    private cdr: ChangeDetectorRef,
    private bsr: BusinessService,
    private saleService: SaleService,
    injector: Injector,
  ) {
    super(injector);
    this.queryData = { number: '', name: '', type: '' };
    this.productDtoList = [];
  }
  @ViewChild('productEdit') productEdit: ProductEditComponent;
  productDtoList: ProductDto[];
  queryData: ProductQueryDto;
  loading = false;
  expandForm = false;
  types: any[];
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
    { field: 'Type', order: null },
    { field: 'Datetime', order: null }
  ];

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.Sale.Sale.Product', [
      'Search',
      'Add',
      'Get',
      'Delete'
    ]);
  }

  async ngOnInit() {
    await super.checkAuth();
    this.bsr.getBaseDataValuesByType('PRODUCTTYPE').subscribe(res => {
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
      this.saleService.searchProduct(this.queryData, this.sorts, this.pageIndex, this.pageSize)
        .subscribe((res: AjaxResult) => {
          if (res.type === AjaxResultType.success) {
            this.productDtoList = res.data.rows;
            this.total = res.data.total;
          } else if (res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }

  refresh() {
    this.pageIndex = 1;
    this.queryData = { number: '', name: '', type: '' };
    this.search();
  }

  reset() {
    this.queryData = { number: '', name: '', type: '' };
  }

  currentPageDataChange($event: ProductDto[]) {
    this.productDtoList = $event;
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
    this.productDtoList.forEach(item => (this.mapOfCheckedId[item.id] = value));
    this.refreshStatus();
  }

  refreshStatus(): void {
    this.hasSelected = this.hasRowSelected();
    this.isAllDisplayDataChecked = this.productDtoList.every(item => this.mapOfCheckedId[item.id]);
    this.isIndeterminate =
      this.productDtoList.some(item => this.mapOfCheckedId[item.id]) && !this.isAllDisplayDataChecked;
  }

  add() {
    this.productEdit.show('Add');
  }

  edit(data: ProductDto) {
    this.saleService.getProduct(data.id)
      .subscribe(res => {
        if (res.type === AjaxResultType.success) {
          this.productEdit.show('Update', res.data);
        } else if (res.content) {
          this.msg.error(res.content);
        }
      });
  }

  delete() {
    const ids = this.productDtoList.filter(v => this.mapOfCheckedId[v.id]).map(v => v.id);
    if (!ids || ids.length === 0) {
      this.msg.warning('请选择要操作的数据');
    } else {
      this.saleService.deleteProduct(ids)
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
