import { Component, OnInit, Injector } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService } from 'ng-zorro-antd';
import { SaleService } from '@shared/business/services/sale.service';
import { Router, ActivatedRoute } from '@angular/router';
import { SaleOrderDto, SaleOrderQueryDto, SortDescription } from '@shared/business/app.model';
import { AuthConfig, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-sale-order-list',
  templateUrl: './sale-order-list.component.html',
  styles: []
})
export class SaleOrderListComponent extends ComponentBase implements OnInit {

  constructor(
    private msg: NzMessageService,
    private saleService: SaleService,
    private router: Router,
    private route: ActivatedRoute,
    injector: Injector,
  ) {
    super(injector);
    super.checkAuth();
  }

  saleOrderDtoList: SaleOrderDto[] = [];
  queryData: SaleOrderQueryDto = { number: '', customerName: '', customerNumber: '' };
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
    { field: 'Number', order: null },
    { field: 'CustomerName', order: null },
    { field: 'CustomerNumber', order: null },
    { field: 'TotalAmount', order: null },
    { field: 'Datetime', order: null }
  ];

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.Sale.Sale.SaleOrder', [
      'Search',
      'Add',
      'Get',
      'Delete'
    ]);
  }

  ngOnInit() {
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
      this.saleService.searchSaleOrder(this.queryData, this.sorts, this.pageIndex, this.pageSize)
        .subscribe((res: AjaxResult) => {
          if (res.type === AjaxResultType.success) {
            this.saleOrderDtoList = res.data.rows;
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
    this.queryData = { number: '', customerName: '', customerNumber: '' };
    this.search();
  }

  reset() {
    this.queryData = { number: '', customerName: '', customerNumber: '' };
  }

  currentPageDataChange($event: SaleOrderDto[]) {
    this.saleOrderDtoList = $event;
    this.refreshStatus();
  }

  pageIndexChange() {
    this.search();
  }

  checkAll(value: boolean): void {
    this.saleOrderDtoList.forEach(item => (this.mapOfCheckedId[item.id] = value));
    this.refreshStatus();
  }

  refreshStatus(): void {
    this.hasSelected = this.hasRowSelected();
    this.isAllDisplayDataChecked = this.saleOrderDtoList.every(item => this.mapOfCheckedId[item.id]);
    this.isIndeterminate =
      this.saleOrderDtoList.some(item => this.mapOfCheckedId[item.id]) && !this.isAllDisplayDataChecked;
  }

  add() {
    this.router.navigateByUrl('/dashboard/sale/saleorder-add');
  }

  edit(data: SaleOrderDto) {
    //this.router.navigateByUrl(`/bashboard/sale/saleorder-edit/${data.id}`);
    this.router.navigate(['dashboard', 'sale', 'saleorder-edit', data.id]);
  }

  delete() {
    const ids = this.saleOrderDtoList.filter(v => this.mapOfCheckedId[v.id]).map(v => v.id);
    if (!ids || ids.length === 0) {
      this.msg.warning('请选择要操作的数据');
    } else {
      this.saleService.deleteSaleOrder(ids)
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
