import { Router, ActivatedRoute } from '@angular/router';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { Component, OnInit, ChangeDetectorRef, Injector } from '@angular/core';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { PurchaseService } from '@shared/business/services/purchase.service';
import { I18NService } from '@core/i18n/i18n.service';
import { AuthConfig, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';
import { PurchaseOrderDto, PurchaseOrderQueryDto, SortDescription, MaterialDto } from '@shared/business/app.model';

@Component({
  selector: 'app-purchase-order-list',
  templateUrl: './purchase-order-list.component.html',
  styles: []
})
export class PurchaseOrderListComponent extends ComponentBase implements OnInit {

  constructor(
    private msg: NzMessageService,
    private purchaseService: PurchaseService,
    private router: Router,
    private route: ActivatedRoute,
    injector: Injector,
  ) {
    super(injector);
    super.checkAuth();
  }

  purchaseOrderDtoList: PurchaseOrderDto[] = [];
  queryData: PurchaseOrderQueryDto = { number: '', vendorName: '', vendorNumber: '' };
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
    { field: 'VendorName', order: null },
    { field: 'VendorNumber', order: null },
    { field: 'TotalAmount', order: null },
    { field: 'Datetime', order: null }
  ];

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.Purchase.Purchase.PurchaseOrder', [
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
      this.purchaseService.searchPurchaseOrder(this.queryData, this.sorts, this.pageIndex, this.pageSize)
        .subscribe((res: AjaxResult) => {
          if (res.type === AjaxResultType.success) {
            this.purchaseOrderDtoList = res.data.rows;
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
    this.queryData = { number: '', vendorName: '', vendorNumber: '' };
    this.search();
  }

  reset() {
    this.queryData = { number: '', vendorName: '', vendorNumber: '' };
  }

  currentPageDataChange($event: PurchaseOrderDto[]) {
    this.purchaseOrderDtoList = $event;
    this.refreshStatus();
  }

  pageIndexChange() {
    this.search();
  }

  checkAll(value: boolean): void {
    this.purchaseOrderDtoList.forEach(item => (this.mapOfCheckedId[item.id] = value));
    this.refreshStatus();
  }

  refreshStatus(): void {
    this.hasSelected = this.hasRowSelected();
    this.isAllDisplayDataChecked = this.purchaseOrderDtoList.every(item => this.mapOfCheckedId[item.id]);
    this.isIndeterminate =
      this.purchaseOrderDtoList.some(item => this.mapOfCheckedId[item.id]) && !this.isAllDisplayDataChecked;
  }

  add() {
    this.router.navigateByUrl('/dashboard/purchase/purchaseorder-add');
  }

  edit(data: PurchaseOrderDto) {
    //this.router.navigateByUrl(`/bashboard/purchase/purchaseorder-edit/${data.id}`);
    this.router.navigate(['dashboard', 'purchase', 'purchaseorder-edit', data.id]);
  }

  delete() {
    const ids = this.purchaseOrderDtoList.filter(v => this.mapOfCheckedId[v.id]).map(v => v.id);
    if (!ids || ids.length === 0) {
      this.msg.warning('请选择要操作的数据');
    } else {
      this.purchaseService.deletePurchaseOrder(ids)
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
