import { SaleService } from './../../../../shared/business/services/sale.service';
import { TemplateType } from './../../../../shared/business/app.model';
import { Component, OnInit, ChangeDetectorRef, Injector } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { PurchaseService } from '@shared/business/services/purchase.service';
import { WarehouseService } from '@shared/business/services/warehouse.service';
import { I18NService } from '@core/i18n/i18n.service';
import { ActivatedRoute, Router } from '@angular/router';
import { InboundReceiptDto, InboundReceiptItemDto, QueryCondition, GoodsCategory } from '@shared/business/app.model';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';
import { DataListSingleServiceComponent } from '@shared/business/components/data-list-single-service/data-list-single-service.component';

@Component({
  selector: 'app-inbound-receipt-edit',
  templateUrl: './inbound-receipt-edit.component.html',
  styles: []
})
export class InboundReceiptEditComponent extends ComponentBase implements OnInit {
  constructor(
    private msg: NzMessageService,
    private purchaseService: PurchaseService,
    private saleService: SaleService,
    private warehouseService: WarehouseService,
    private i18nService: I18NService,
    private modalService: NzModalService,
    private route: ActivatedRoute,
    private router: Router,
    injector: Injector,
  ) {
    super(injector);
    this.inboundReceiptDto = new InboundReceiptDto();
    this.inboundReceiptDto.items = [];
  }

  mode: 'Add' | 'Update' = 'Add';
  inboundReceiptDto: InboundReceiptDto;
  editIndex = -1;
  editObj: InboundReceiptItemDto;
  private isAdding = false;

  protected AuthConfig(): AuthConfig {
    const authConfig = new AuthConfig('Root.Warehouse.Warehouse.InboundReceipt', []);
    if (this.mode === 'Add') {
      authConfig.funcs.push(this.mode, 'GetNewNumber');
    } else {
      authConfig.funcs.push(this.mode);
    }

    return authConfig;
  }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      if (params && params['id']) {
        this.mode = 'Update';
        this.warehouseService.getInboundReceipt(params['id'])
          .subscribe(res => {
            if (res && res.type === AjaxResultType.success) {
              this.inboundReceiptDto = res.data;
            } else if (res && res.content) {
              this.msg.error(res.content);
            }
          });
      } else {
        this.mode = 'Add';
      }

      super.checkAuth();
    });
  }

  getNewNumber() {
    this.warehouseService.getInboundReceiptNewNumber()
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.inboundReceiptDto.number = res.data;
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
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

            this.inboundReceiptDto.warehouseName = data.name;
            this.inboundReceiptDto.warehouseNumber = data.number;
            this.inboundReceiptDto.items = [];
            componentInstance.close();
          }
        }
      ]
    });
  }

  selectProductItem() {
    const modal = this.modalService.create({
      nzTitle: this.i18nService.fanyi('app.dashboard.product'),
      nzContent: DataListSingleServiceComponent,
      nzComponentParams: {
        serverAuthConfig: new AuthConfig('Root.Sale.Sale.Product', ['Search']),
        columns: [
          { name: this.i18nService.fanyi('app.dashboard.product-name'), get: d => d.name },
          { name: this.i18nService.fanyi('app.dashboard.product-number'), get: d => d.number },
          { name: this.i18nService.fanyi('app.dashboard.product-cost-price'), get: d => d.price },
        ],
        read: (query: QueryCondition) => {
          const componentInstance = modal.getContentComponent();
          if (!componentInstance.auth.Search) {
            this.msg.warning('您没有该权限');
          } else {
            this.saleService.searchProduct(null, null, query.pageIndex, query.pageSize)
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

            if (!this.editObj) {
              this.editObj = new InboundReceiptItemDto();
            }

            this.editObj.number = data.number;
            this.editObj.name = data.name;
            this.editObj.type = data.type;
            this.editObj.brand = data.brand;
            this.editObj.price = data.costPrice;
            this.editObj.spec = data.spec;
            this.editObj.unit = data.unit;
            this.editObj.amount = 0;
            this.editObj.goodsCategory = GoodsCategory.product;
            componentInstance.close();
          }
        }
      ]
    });
  }

  selectMaterialItem(index: number) {
    const modal = this.modalService.create({
      nzTitle: this.i18nService.fanyi('app.dashboard.material'),
      nzContent: DataListSingleServiceComponent,
      nzComponentParams: {
        serverAuthConfig: new AuthConfig('Root.Purchase.Purchase.Material', ['Search']),
        columns: [
          { name: this.i18nService.fanyi('app.dashboard.material-name'), get: d => d.name },
          { name: this.i18nService.fanyi('app.dashboard.material-number'), get: d => d.number },
          { name: this.i18nService.fanyi('app.dashboard.material-price'), get: d => d.price }
        ],
        read: (query: QueryCondition) => {
          const componentInstance = modal.getContentComponent();
          if (!componentInstance.auth.Search) {
            this.msg.warning('您没有该权限');
          } else {
            this.purchaseService.searchMaterial(null, null, query.pageIndex, query.pageSize)
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

            if (!this.editObj) {
              this.editObj = new InboundReceiptItemDto();
            }
            this.editObj.number = data.number;
            this.editObj.name = data.name;
            this.editObj.type = data.type;
            this.editObj.brand = data.brand;
            this.editObj.price = data.price;
            this.editObj.spec = data.spec;
            this.editObj.unit = data.unit;
            this.editObj.amount = 0;
            this.editObj.goodsCategory = GoodsCategory.material;
            componentInstance.close();
          }
        }
      ]
    });
  }

  addItem() {
    if(!this.inboundReceiptDto.warehouseNumber){
      this.msg.warning("请先选择仓库");
      return;
    }
    this.inboundReceiptDto.items.push(new InboundReceiptItemDto());
    this.isAdding = true;
    this.editItem(this.inboundReceiptDto.items.length - 1);
  }

  delItem(index: number) {
    if (this.inboundReceiptDto.items && this.inboundReceiptDto.items.length < index)
      this.inboundReceiptDto.items = this.inboundReceiptDto.items.splice(index, 1);
  }

  editItem(index: number) {
    this.editObj = { ...this.inboundReceiptDto.items[index] };
    this.editIndex = index;
  }

  saveItem(index: number) {
    if (this.editObj && this.editObj.number && this.editObj.name && this.editObj.price !== null && this.editObj.amount !== null) {
      this.inboundReceiptDto.items[index] = { ...this.editObj };
      this.editIndex = -1;
    }
  }

  cancelItem(index: number) {
    this.editObj = {};
    if (this.isAdding) {
      this.inboundReceiptDto.items = this.inboundReceiptDto.items.splice(0, index);
      this.isAdding = false;
    }
    this.editIndex = -1;
  }

  submit() {
    if (!this.inboundReceiptDto.items || this.inboundReceiptDto.items.length <= 0) {
      this.msg.warning('请添加入库项');
      return;
    }

    if (this.mode === 'Add') {
      this.warehouseService.addInboundReceipt(this.inboundReceiptDto)
        .subscribe(res => {
          if (res && res.type === AjaxResultType.success) {
            this.msg.success('添加成功');
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    } else {
      this.warehouseService.updateInboundReceipt(this.inboundReceiptDto)
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
        id: this.inboundReceiptDto.id,
        type: TemplateType.inboundReceipt
      }
    });
  }

  startExport() {
    if (this.mode === 'Add') {
      return;
    }

    this.router.navigate(['dashboard', 'document', 'document-window'], {
      queryParams: {
        id: this.inboundReceiptDto.id,
        type: TemplateType.inboundReceipt
      }
    });
  }
}
