import { Component, OnInit, Injector } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService } from 'ng-zorro-antd';
import { WarehouseService } from '@shared/business/services/warehouse.service';
import { Router, ActivatedRoute } from '@angular/router';
import { OutboundReceiptDto, OutboundReceiptQueryDto, SortDescription } from '@shared/business/app.model';
import { AuthConfig, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-outbound-receipt-list',
  templateUrl: './outbound-receipt-list.component.html',
  styles: []
})
export class OutboundReceiptListComponent extends ComponentBase implements OnInit {

  constructor(
    private msg: NzMessageService,
    private warehouseService: WarehouseService,
    private router: Router,
    private route: ActivatedRoute,
    injector: Injector,
  ) {
    super(injector);
    super.checkAuth();
  }

  outboundReceiptDtoList: OutboundReceiptDto[] = [];
  queryData: OutboundReceiptQueryDto = { number: '', receiver: '', receiverNo: '', warehouseName: '', warehouseNumber: '' };
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
    { field: 'Number', order: null },
    { field: 'Supplier', order: null },
    { field: 'SupplierNo', order: null },
    { field: 'WarehouseName', order: null },
    { field: 'WarehouseNumber', order: null },
    { field: 'DateTime', order: null }
  ];

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.Warehouse.Warehouse.OutboundReceipt', [
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
      this.warehouseService.searchOutboundReceipt(this.queryData, this.sorts, this.pageIndex, this.pageSize)
        .subscribe((res: AjaxResult) => {
          if (res.type === AjaxResultType.success) {
            this.outboundReceiptDtoList = res.data.rows;
            this.total = res.data.total;
          } else if (res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }

  refresh() {
    this.pageIndex = 1;
    this.queryData = { number: '', receiver: '', receiverNo: '', warehouseName: '', warehouseNumber: '' };
    this.search();
  }

  reset() {
    this.queryData = { number: '', receiver: '', receiverNo: '', warehouseName: '', warehouseNumber: '' };
  }

  currentPageDataChange($event: OutboundReceiptDto[]) {
    this.outboundReceiptDtoList = $event;
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
    this.outboundReceiptDtoList.forEach(item => (this.mapOfCheckedId[item.id] = value));
    this.refreshStatus();
  }

  refreshStatus(): void {
    this.hasSelected = this.hasRowSelected();
    this.isAllDisplayDataChecked = this.outboundReceiptDtoList.every(item => this.mapOfCheckedId[item.id]);
    this.isIndeterminate =
      this.outboundReceiptDtoList.some(item => this.mapOfCheckedId[item.id]) && !this.isAllDisplayDataChecked;
  }

  add() {
    this.router.navigateByUrl('/dashboard/warehouse/outboundreceipt-add');
  }

  edit(data: OutboundReceiptDto) {
    //this.router.navigateByUrl(`/bashboard/purchase/purchaseorder-edit/${data.id}`);
    this.router.navigate(['dashboard', 'warehouse', 'outboundreceipt-edit', data.id]);
  }

  delete() {
    const ids = this.outboundReceiptDtoList.filter(v => this.mapOfCheckedId[v.id]).map(v => v.id);
    if (!ids || ids.length === 0) {
      this.msg.warning('请选择要操作的数据');
    } else {
      this.warehouseService.deleteOutboundReceipt(ids)
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
