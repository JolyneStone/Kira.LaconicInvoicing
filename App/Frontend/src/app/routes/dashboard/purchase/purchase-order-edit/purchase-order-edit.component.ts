import { WarehouseService } from './../../../../shared/business/services/warehouse.service';
import { Component, OnInit, AfterViewInit, ChangeDetectorRef, Injector } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { PurchaseService } from '@shared/business/services/purchase.service';
import { I18NService } from '@core/i18n/i18n.service';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';
import { MaterialDto, PurchaseOrderDto, QueryCondition } from '@shared/business/app.model';
import { ActivatedRoute, Router } from '@angular/router';
import { DataListSingleServiceComponent } from '@shared/business/components/data-list-single-service/data-list-single-service.component';
import { PurchaseOrderItemDto, PurchaseOrderStatus, TemplateType } from '../../../../shared/business/app.model';

@Component({
  selector: 'app-purchase-order-edit',
  templateUrl: './purchase-order-edit.component.html',
  styles: []
})
export class PurchaseOrderEditComponent extends ComponentBase implements OnInit {
  constructor(
    private msg: NzMessageService,
    private bsr: BusinessService,
    private purchaseService: PurchaseService,
    private warehouseService: WarehouseService,
    private i18nService: I18NService,
    private modalService: NzModalService,
    private route: ActivatedRoute,
    private router: Router,
    injector: Injector,
  ) {
    super(injector);
    this.purchaseOrderDto = new PurchaseOrderDto();
    this.purchaseOrderDto.status = PurchaseOrderStatus.book;
    this.purchaseOrderDto.items = [];
  }

  mode: 'Add' | 'Update' = 'Add';
  purchaseOrderDto: PurchaseOrderDto;
  payWays: any[];
  vendorId: string;
  editIndex = -1;
  editObj: PurchaseOrderItemDto;
  statusKeys: any[];
  purchaseOrderStatus: typeof PurchaseOrderStatus = PurchaseOrderStatus;
  private isAdding = false;

  protected AuthConfig(): AuthConfig {
    const authConfig = new AuthConfig('Root.Purchase.Purchase.PurchaseOrder', []);
    if (this.mode === 'Add') {
      authConfig.funcs.push(this.mode, 'GetNewNumber');
    } else {
      authConfig.funcs.push(this.mode);
    }

    return authConfig;
  }

  ngOnInit() {
    this.statusKeys = Object.keys(this.purchaseOrderStatus).filter(f => !isNaN(Number(f)));
    this.route.params.subscribe((params) => {
      if (params && params['id']) {
        this.mode = 'Update';
        this.purchaseService.getPurchaseOrder(params['id'])
          .subscribe(res => {
            if (res && res.type === AjaxResultType.success) {
              this.purchaseOrderDto = res.data;
            } else if (res && res.content) {
              this.msg.error(res.content);
            }
          });
      } else {
        this.mode = 'Add';
      }

      super.checkAuth();
    });

    this.bsr.getBaseDataValuesByType('PAYWAY')
      .subscribe(payWays => {
        this.payWays = payWays;
      });
  }

  get totalAmount(): number {
    let totalAmount = 0;
    if (this.purchaseOrderDto.items && this.purchaseOrderDto.items.length > 0) {
      this.purchaseOrderDto.items.forEach(item => {
        if (item.price && item.amount)
          totalAmount += item.price * item.amount;
      });
    }

    if (this.purchaseOrderDto.freight && !isNaN(this.purchaseOrderDto.freight))
      totalAmount += Number(this.purchaseOrderDto.freight);

    return totalAmount;
  }
  get totalQuantity(): number {
    let totalQuantity = 0;
    if (this.purchaseOrderDto.items && this.purchaseOrderDto.items.length > 0) {
      this.purchaseOrderDto.items.forEach(item => {
        if (item.amount)
          totalQuantity += Number(item.amount);
      });
    }

    return totalQuantity;
  }

  getNewNumber() {
    this.purchaseService.getPurchaseOrderNewNumber()
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.purchaseOrderDto.number = res.data;
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
      });
  }

  selectVendor() {
    const modal = this.modalService.create({
      nzTitle: this.i18nService.fanyi('app.dashboard.vendor'),
      nzContent: DataListSingleServiceComponent,
      nzComponentParams: {
        serverAuthConfig: new AuthConfig('Root.Purchase.Purchase.Vendor', ['Search']),
        columns: [
          { name: this.i18nService.fanyi('app.dashboard.vendor-name'), get: d => d.name },
          { name: this.i18nService.fanyi('app.dashboard.vendor-number'), get: d => d.number },
          { name: this.i18nService.fanyi('app.dashboard.vendor-type'), get: d => d.type }
        ],
        read: (query: QueryCondition) => {
          const componentInstance = modal.getContentComponent();
          if (!componentInstance.auth.Search) {
            this.msg.warning('您没有该权限');
          } else {
            this.purchaseService.searchVendor(null, null, query.pageIndex, query.pageSize)
              .subscribe(res => {
                if (res.type === AjaxResultType.success) {
                  componentInstance.data = res.data.rows;
                  componentInstance.total = res.data.total;
                } else if (res && res.content) {
                  this.msg.error(res.content);
                }
              });
          }
        }
      },
      nzFooter: [
        {
          label: this.i18nService.fanyi('app.cancel'),
          type: 'default',
          onClick: componentInstance => {
            componentInstance.close();
          }
        },
        {
          label: this.i18nService.fanyi('app.ok'),
          type: 'primary',
          onClick: componentInstance => {
            const data = componentInstance.selectData;
            if (data === null) {
              this.msg.warning(this.i18nService.fanyi('app.please-signle-selected'));
              return;
            }

            this.purchaseOrderDto.vendorName = data.name;
            this.purchaseOrderDto.vendorNumber = data.number;
            this.purchaseOrderDto.consignor = data.contactPerson;
            this.purchaseOrderDto.consignorContact = data.phoneNumber || data.email;
            this.purchaseOrderDto.sourceAddress = data.address;
            this.purchaseOrderDto.items = [];
            this.vendorId = data.id;
            componentInstance.close();
          }
        }
      ]
    });
  }

  selectWarehouse() {
    const modal = this.modalService.create({
      nzTitle: this.i18nService.fanyi('app.dashboard.warehouse'),
      nzContent: DataListSingleServiceComponent,
      nzComponentParams: {
        serverAuthConfig: new AuthConfig('Root.Warehouse.Warehouse.Warehouse', ['Search']),
        columns: [
          { name: this.i18nService.fanyi('app.dashboard.warehouse-name'), get: d => d.name },
          { name: this.i18nService.fanyi('app.dashboard.warehouse-number'), get: d => d.number },
          {
            name: this.i18nService.fanyi('app.dashboard.warehouse-address'), get: d =>
              d.address && d.address.lenght > 10 ? d.address.substr(0, 10) + '...' : d.address
          }
        ],
        read: (query: QueryCondition) => {
          const componentInstance = modal.getContentComponent();
          if (!componentInstance.auth.Search) {
            this.msg.warning('您没有该权限');
          } else {
            this.warehouseService.searchWarehouse(null, null, query.pageIndex, query.pageSize)
              .subscribe(res => {
                if (res.type === AjaxResultType.success) {
                  componentInstance.data = res.data.rows;
                  componentInstance.total = res.data.total;
                } else if (res && res.content) {
                  this.msg.error(res.content);
                }
              });
          }
        }
      },
      nzFooter: [
        {
          label: this.i18nService.fanyi('app.cancel'),
          type: 'default',
          onClick: componentInstance => {
            componentInstance.close();
          }
        },
        {
          label: this.i18nService.fanyi('app.ok'),
          type: 'primary',
          onClick: componentInstance => {
            const data = componentInstance.selectData;
            if (data === null) {
              this.msg.warning(this.i18nService.fanyi('app.please-signle-selected'));
              return;
            }

            this.purchaseOrderDto.warehouseName = data.name;
            this.purchaseOrderDto.warehouseNumber = data.number;
            this.purchaseOrderDto.consignee = data.manager;
            this.purchaseOrderDto.consigneeContact = data.managerContact;
            this.purchaseOrderDto.destAddress = data.address;
            componentInstance.close();
          }
        }
      ]
    });
  }

  selectMaterialItem(index: number) {
    if (!this.purchaseOrderDto.vendorNumber) {
      this.msg.warning('请先选择供应商');
      this.isAdding = false;
      return;
    }

    const modal = this.modalService.create({
      nzTitle: this.i18nService.fanyi('app.dashboard.material'),
      nzContent: DataListSingleServiceComponent,
      nzComponentParams: {
        serverAuthConfig: new AuthConfig('Root.Purchase.Purchase.Vendor', ['GetMaterials']),
        columns: [
          { name: this.i18nService.fanyi('app.dashboard.material-name'), get: d => d.name },
          { name: this.i18nService.fanyi('app.dashboard.material-number'), get: d => d.number },
          { name: this.i18nService.fanyi('app.dashboard.material-price'), get: d => d.price }
        ],
        read: (query: QueryCondition) => {
          const componentInstance = modal.getContentComponent();
          if (!componentInstance.auth.GetMaterials) {
            this.msg.warning('您没有该权限');
          } else {
            query.filters = [{ field: 'VendorId', conditionOp: 'Equal', value: this.vendorId, logicOp: 'AndAlso' }];
            this.purchaseService.getMaterialsByVendor(query)
              .subscribe(res => {
                if (res.type === AjaxResultType.success) {
                  componentInstance.data = res.data.rows;
                  componentInstance.total = res.data.total;
                } else if (res && res.content) {
                  this.msg.error(res.content);
                }
              });
          }
        }
      },
      nzFooter: [
        {
          label: this.i18nService.fanyi('app.cancel'),
          type: 'default',
          onClick: componentInstance => {
            componentInstance.close();
            this.isAdding = false;
          }
        },
        {
          label: this.i18nService.fanyi('app.ok'),
          type: 'primary',
          onClick: componentInstance => {
            const data = componentInstance.selectData;
            if (data === null) {
              this.msg.warning(this.i18nService.fanyi('app.please-signle-selected'));
              return;
            }

            this.editObj.number = data.number;
            this.editObj.name = data.name;
            this.editObj.type = data.type;
            this.editObj.spec = data.spec;
            this.editObj.brand = data.brand;
            this.editObj.unit = data.unit;
            this.editObj.price = data.price;
            this.editObj.comment = data.comment;
            this.editObj.amount = 0;

            componentInstance.close();
          }
        }
      ]
    });
  }

  addItem() {
    if (this.purchaseOrderDto.vendorNumber) {
      this.purchaseOrderDto.items.push(new PurchaseOrderItemDto());
      this.isAdding = true;
      this.editItem(this.purchaseOrderDto.items.length - 1);
    } else {
      this.msg.info('请先选择供应商');
    }
  }

  delItem(index: number) {
    if (this.purchaseOrderDto.items && this.purchaseOrderDto.items.length < index)
      this.purchaseOrderDto.items = this.purchaseOrderDto.items.splice(index, 1);
  }

  editItem(index: number) {
    this.editObj = { ...this.purchaseOrderDto.items[index] };
    this.editIndex = index;
  }

  saveItem(index: number) {
    if (this.editObj && this.editObj.number && this.editObj.name && this.editObj.price !== null && this.editObj.amount !== null) {
      if (this.purchaseOrderDto.items.findIndex(m => this.editObj.number === m.number) < 0) {
        this.purchaseOrderDto.items[index] = { ...this.editObj };
        this.editIndex = -1;
      }
    }
  }

  cancelItem(index: number) {
    this.editObj = {};
    if (this.isAdding) {
      this.purchaseOrderDto.items = this.purchaseOrderDto.items.splice(0, index);
      this.isAdding = false;
    }
    this.editIndex = -1;
  }

  submit() {
    if (!this.purchaseOrderDto.items || this.purchaseOrderDto.items.length <= 0) {
      this.msg.warning('请添加采购项');
      return;
    }
    this.purchaseOrderDto.totalAmount = this.totalAmount;
    this.purchaseOrderDto.totalQuantity = this.totalQuantity;
    if (this.mode === 'Add') {
      this.purchaseService.addPurchaseOrder(this.purchaseOrderDto)
        .subscribe(res => {
          if (res && res.type === AjaxResultType.success) {
            this.msg.success('添加成功');
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    } else {
      this.purchaseService.updatePurchaseOrder(this.purchaseOrderDto)
        .subscribe(res => {
          if (res && res.type === AjaxResultType.success) {
            this.msg.success('编辑成功');
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }

  startPrint() {
    if (this.mode === 'Add') {
      return;
    }

    this.router.navigate(['dashboard', 'print', 'print-window'], {
      queryParams: {
        id: this.purchaseOrderDto.id,
        type: TemplateType.purchaseOrder
      }
    });
  }

  startExport() {
    if (this.mode === 'Add') {
      return;
    }

    this.router.navigate(['dashboard', 'document', 'document-window'], {
      queryParams: {
        id: this.purchaseOrderDto.id,
        type: TemplateType.purchaseOrder
      }
    });
  }
}
