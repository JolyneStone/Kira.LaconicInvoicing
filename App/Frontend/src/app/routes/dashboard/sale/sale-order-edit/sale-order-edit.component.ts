import { TemplateType } from './../../../../shared/business/app.model';
import { Component, OnInit, ChangeDetectorRef, Injector } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { SaleService } from '@shared/business/services/sale.service';
import { WarehouseService } from '@shared/business/services/warehouse.service';
import { I18NService } from '@core/i18n/i18n.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SaleOrderDto, SaleOrderStatus, SaleOrderItemDto, QueryCondition } from '@shared/business/app.model';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';
import { DataListSingleServiceComponent } from '@shared/business/components/data-list-single-service/data-list-single-service.component';

@Component({
  selector: 'app-sale-order-edit',
  templateUrl: './sale-order-edit.component.html',
  styles: []
})
export class SaleOrderEditComponent extends ComponentBase implements OnInit {
  constructor(
    private msg: NzMessageService,
    private bsr: BusinessService,
    private saleService: SaleService,
    private warehouseService: WarehouseService,
    private i18nService: I18NService,
    private modalService: NzModalService,
    private route: ActivatedRoute,
    private router: Router,
    injector: Injector,
  ) {
    super(injector);
    this.saleOrderDto = new SaleOrderDto();
    this.saleOrderDto.status = SaleOrderStatus.book;
    this.saleOrderDto.items = [];
  }

  mode: 'Add' | 'Update' = 'Add';
  saleOrderDto: SaleOrderDto;
  payWays: any[];
  customerId: string;
  editIndex = -1;
  editObj: SaleOrderItemDto;
  statusKeys: any[];
  saleOrderStatus: typeof SaleOrderStatus = SaleOrderStatus;
  private isAdding = false;

  protected AuthConfig(): AuthConfig {
    const authConfig = new AuthConfig('Root.Sale.Sale.SaleOrder', []);
    if (this.mode === 'Add') {
      authConfig.funcs.push(this.mode, 'GetNewNumber');
    } else {
      authConfig.funcs.push(this.mode);
    }

    return authConfig;
  }

  ngOnInit() {
    this.statusKeys = Object.keys(this.saleOrderStatus).filter(f => !isNaN(Number(f)));
    this.route.params.subscribe((params) => {
      if (params && params['id']) {
        this.mode = 'Update';
        this.saleService.getSaleOrder(params['id'])
          .subscribe(res => {
            if (res && res.type === AjaxResultType.success) {
              this.saleOrderDto = res.data;
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
    if (this.saleOrderDto.items && this.saleOrderDto.items.length > 0) {
      this.saleOrderDto.items.forEach(item => {
        if (item.price && item.amount)
          totalAmount += item.price * item.amount;
      });
    }

    if (this.saleOrderDto.freight && !isNaN(this.saleOrderDto.freight))
      totalAmount += Number(this.saleOrderDto.freight);

    return totalAmount;
  }
  get totalQuantity(): number {
    let totalQuantity = 0;
    if (this.saleOrderDto.items && this.saleOrderDto.items.length > 0) {
      this.saleOrderDto.items.forEach(item => {
        if (item.amount)
          totalQuantity += Number(item.amount);
      });
    }

    return totalQuantity;
  }

  getNewNumber() {
    this.saleService.getSaleOrderNewNumber()
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.saleOrderDto.number = res.data;
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
      });
  }

  selectCustomer() {
    const modal = this.modalService.create({
      nzTitle: this.i18nService.fanyi('app.dashboard.customer'),
      nzContent: DataListSingleServiceComponent,
      nzComponentParams: {
        serverAuthConfig: new AuthConfig('Root.Sale.Sale.Customer', ['Search']),
        columns: [
          { name: this.i18nService.fanyi('app.dashboard.customer-name'), get: d => d.name },
          { name: this.i18nService.fanyi('app.dashboard.customer-number'), get: d => d.number },
          { name: this.i18nService.fanyi('app.dashboard.customer-type'), get: d => d.type }
        ],
        read: (query: QueryCondition) => {
          const componentInstance = modal.getContentComponent();
          if (!componentInstance.auth.Search) {
            this.msg.warning('您没有该权限');
          } else {
            this.saleService.searchCustomer(null, null, query.pageIndex, query.pageSize)
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

            this.saleOrderDto.customerName = data.name;
            this.saleOrderDto.customerNumber = data.number;
            this.saleOrderDto.consignor = data.contactPerson;
            this.saleOrderDto.consignorContact = data.phoneNumber || data.email;
            this.saleOrderDto.sourceAddress = data.address;
            this.saleOrderDto.items = [];
            this.customerId = data.id;
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

            this.saleOrderDto.warehouseName = data.name;
            this.saleOrderDto.warehouseNumber = data.number;
            this.saleOrderDto.consignee = data.manager;
            this.saleOrderDto.consigneeContact = data.managerContact;
            this.saleOrderDto.destAddress = data.address;
            componentInstance.close();
          }
        }
      ]
    });
  }

  selectProductItem(index: number) {
    if (!this.saleOrderDto.customerNumber) {
      this.msg.warning('请先选择客户');
      this.isAdding = false;
      return;
    }

    const modal = this.modalService.create({
      nzTitle: this.i18nService.fanyi('app.dashboard.product'),
      nzContent: DataListSingleServiceComponent,
      nzComponentParams: {
        serverAuthConfig: new AuthConfig('Root.Sale.Sale.Customer', ['GetProducts']),
        columns: [
          { name: this.i18nService.fanyi('app.dashboard.product-name'), get: d => d.name },
          { name: this.i18nService.fanyi('app.dashboard.product-number'), get: d => d.number },
          { name: this.i18nService.fanyi('app.dashboard.product-cost-price'), get: d => d.costPrice },
          { name: this.i18nService.fanyi('app.dashboard.product-wholesale-price'), get: d => d.wholesalePrice },
          { name: this.i18nService.fanyi('app.dashboard.product-retail-price'), get: d => d.retailPrice }
        ],
        read: (query: QueryCondition) => {
          const componentInstance = modal.getContentComponent();
          if (!componentInstance.auth.GetProducts) {
            this.msg.warning('您没有该权限');
          } else {
            query.filters = [{ field: 'CustomerId', conditionOp: 'Equal', value: this.customerId, logicOp: 'AndAlso' }];
            this.saleService.getProductsByCustomer(query)
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
            this.editObj.price = data.wholesalePrice;
            this.editObj.comment = data.comment;
            this.editObj.amount = 0;

            componentInstance.close();
          }
        }
      ]
    });
  }

  addItem() {
    if (this.saleOrderDto.customerNumber) {
      this.saleOrderDto.items.push(new SaleOrderItemDto());
      this.isAdding = true;
      this.editItem(this.saleOrderDto.items.length - 1);
    } else {
      this.msg.info('请先选择客户');
    }
  }

  delItem(index: number) {
    if (this.saleOrderDto.items && this.saleOrderDto.items.length < index)
      this.saleOrderDto.items = this.saleOrderDto.items.splice(index, 1);
  }

  editItem(index: number) {
    this.editObj = { ...this.saleOrderDto.items[index] };
    this.editIndex = index;
  }

  saveItem(index: number) {
    if (this.editObj && this.editObj.number && this.editObj.name && this.editObj.price !== null && this.editObj.amount !== null) {
      if (this.saleOrderDto.items.findIndex(m => this.editObj.number === m.number) < 0) {
        this.saleOrderDto.items[index] = { ...this.editObj };
        this.editIndex = -1;
      }
    }
  }

  cancelItem(index: number) {
    this.editObj = {};
    if (this.isAdding) {
      this.saleOrderDto.items = this.saleOrderDto.items.splice(0, index);
      this.isAdding = false;
    }
    this.editIndex = -1;
  }

  submit() {
    if (!this.saleOrderDto.items || this.saleOrderDto.items.length <= 0) {
      this.msg.warning('请添加销售项');
      return;
    }
    this.saleOrderDto.totalAmount = this.totalAmount;
    this.saleOrderDto.totalQuantity = this.totalQuantity;
    if (this.mode === 'Add') {
      this.saleService.addSaleOrder(this.saleOrderDto)
        .subscribe(res => {
          if (res && res.type === AjaxResultType.success) {
            this.msg.success('添加成功');
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    } else {
      this.saleService.updateSaleOrder(this.saleOrderDto)
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
        id: this.saleOrderDto.id,
        type: TemplateType.saleOrder
      }
    });
  }

  startExport() {
    if (this.mode === 'Add') {
      return;
    }

    this.router.navigate(['dashboard', 'document', 'document-window'], {
      queryParams: {
        id: this.saleOrderDto.id,
        type: TemplateType.saleOrder
      }
    });
  }
}
