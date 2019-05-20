import { Component, OnInit, Injector } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService } from 'ng-zorro-antd';
import { WarehouseService } from '@shared/business/services/warehouse.service';
import { Router, ActivatedRoute } from '@angular/router';
import { InboundReceiptDto, InboundReceiptQueryDto, SortDescription } from '@shared/business/app.model';
import { AuthConfig, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-inbound-receipt-list',
  templateUrl: './inbound-receipt-list.component.html',
  styles: []
})
export class InboundReceiptListComponent extends ComponentBase implements OnInit {

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

  inboundReceiptDtoList: InboundReceiptDto[] = [];
  queryData: InboundReceiptQueryDto = { number: '', supplier: '', supplierNo: '', warehouseName: '', warehouseNumber: '' };
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
    { field: 'Supplier', order: null },
    { field: 'SupplierNo', order: null },
    { field: 'WarehouseName', order: null },
    { field: 'WarehouseNumber', order: null },
    { field: 'DateTime', order: null }
  ];

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.Warehouse.Warehouse.InboundReceipt', [
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
      this.warehouseService.searchInboundReceipt(this.queryData, this.sorts, this.pageIndex, this.pageSize)
        .subscribe((res: AjaxResult) => {
          if (res.type === AjaxResultType.success) {
            this.inboundReceiptDtoList = res.data.rows;
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
    this.queryData = { number: '', supplier: '', supplierNo: '', warehouseName: '', warehouseNumber: '' };
    this.search();
  }

  reset() {
    this.queryData = { number: '', supplier: '', supplierNo: '', warehouseName: '', warehouseNumber: '' };
  }

  currentPageDataChange($event: InboundReceiptDto[]) {
    this.inboundReceiptDtoList = $event;
    this.refreshStatus();
  }

  pageIndexChange() {
    this.search();
  }

  checkAll(value: boolean): void {
    this.inboundReceiptDtoList.forEach(item => (this.mapOfCheckedId[item.id] = value));
    this.refreshStatus();
  }

  refreshStatus(): void {
    this.hasSelected = this.hasRowSelected();
    this.isAllDisplayDataChecked = this.inboundReceiptDtoList.every(item => this.mapOfCheckedId[item.id]);
    this.isIndeterminate =
      this.inboundReceiptDtoList.some(item => this.mapOfCheckedId[item.id]) && !this.isAllDisplayDataChecked;
  }

  add() {
    this.router.navigateByUrl('/dashboard/warehouse/inboundreceipt-add');
  }

  edit(data: InboundReceiptDto) {
    //this.router.navigateByUrl(`/bashboard/purchase/purchaseorder-edit/${data.id}`);
    this.router.navigate(['dashboard', 'warehouse', 'inboundreceipt-edit', data.id]);
  }

  delete() {
    const ids = this.inboundReceiptDtoList.filter(v => this.mapOfCheckedId[v.id]).map(v => v.id);
    if (!ids || ids.length === 0) {
      this.msg.warning('请选择要操作的数据');
    } else {
      this.warehouseService.deleteInboundReceipt(ids)
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
