import { TemplateType } from './../../../../shared/business/app.model';
import { Component, OnInit, Injector } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { PurchaseService } from '@shared/business/services/purchase.service';
import { WarehouseService } from '@shared/business/services/warehouse.service';
import { I18NService } from '@core/i18n/i18n.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TransferOrderDto, TransferOrderItemDto, QueryCondition, GoodsCategory } from '@shared/business/app.model';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';
import { DataListSingleServiceComponent } from '@shared/business/components/data-list-single-service/data-list-single-service.component';

@Component({
  selector: 'app-transfer-order-edit',
  templateUrl: './transfer-order-edit.component.html',
  styles: []
})
export class TransferOrderEditComponent extends ComponentBase implements OnInit {
  constructor(
    private msg: NzMessageService,
    private bsr: BusinessService,
    private warehouseService: WarehouseService,
    private i18nService: I18NService,
    private modalService: NzModalService,
    private route: ActivatedRoute,
    private router: Router,
    injector: Injector,
  ) {
    super(injector);
    this.transferOrderDto = new TransferOrderDto();
    this.transferOrderDto.items = [];
  }

  mode: 'Add' | 'Update' = 'Add';
  transferOrderDto: TransferOrderDto;
  editIndex = -1;
  editObj: TransferOrderItemDto;
  private sourceWarehouseId: string;
  private isAdding = false;

  protected AuthConfig(): AuthConfig {
    const authConfig = new AuthConfig('Root.Warehouse.Warehouse.TransferOrder', []);
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
        this.warehouseService.getTransferOrder(params['id'])
          .subscribe(res => {
            if (res && res.type === AjaxResultType.success) {
              this.transferOrderDto = res.data;
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
    this.warehouseService.getTransferOrderNewNumber()
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.transferOrderDto.number = res.data;
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
      });
  }

  selectWarehouse(isSourceWarehouse: boolean = false) {
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

            if (!isSourceWarehouse) {
              this.transferOrderDto.destWarehouseName = data.name;
              this.transferOrderDto.destWarehouseNumber = data.number;
              this.transferOrderDto.destAddress = data.address;
            } else {
              this.transferOrderDto.sourceWarehouseName = data.name;
              this.transferOrderDto.sourceWarehouseNumber = data.number;
              this.transferOrderDto.sourceAddress = data.address;
              this.sourceWarehouseId = data.id;
              this.transferOrderDto.items = [];
            }

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
        serverAuthConfig: new AuthConfig('Root.Warehouse.Warehouse.Inventory', ['Search']),
        columns: [
          { name: this.i18nService.fanyi('app.dashboard.product-name'), get: d => d.name },
          { name: this.i18nService.fanyi('app.dashboard.product-number'), get: d => d.number },
          { name: this.i18nService.fanyi('app.dashboard.product-price'), get: d => d.price },
        ],
        read: (query: QueryCondition) => {
          const componentInstance = modal.getContentComponent();
          if (!componentInstance.auth.Search) {
            this.msg.warning('您没有该权限');
          } else {
            this.warehouseService.searchInventoryProduct({ warehouseId: this.sourceWarehouseId, goodsCategory: GoodsCategory.material }, null, query.pageIndex, query.pageSize)
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

            if (!this.editObj) {
              this.editObj = new TransferOrderItemDto();
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
        serverAuthConfig: new AuthConfig('Root.Warehouse.Warehouse.Inventory', ['Search']),
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
            this.warehouseService.searchInventoryMaterial({ warehouseId: this.sourceWarehouseId, goodsCategory: GoodsCategory.material }, null, query.pageIndex, query.pageSize)
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

            if (!this.editObj) {
              this.editObj = new TransferOrderItemDto();
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
    if(!this.transferOrderDto.sourceWarehouseNumber){
      this.msg.warning("请先选择调出仓库");
      return;
    }
    this.transferOrderDto.items.push(new TransferOrderItemDto());
    this.isAdding = true;
    this.editItem(this.transferOrderDto.items.length - 1);
  }

  delItem(index: number) {
    if (this.transferOrderDto.items && this.transferOrderDto.items.length < index)
      this.transferOrderDto.items = this.transferOrderDto.items.splice(index, 1);
  }

  editItem(index: number) {
    this.editObj = { ...this.transferOrderDto.items[index] };
    this.editIndex = index;
  }

  saveItem(index: number) {
    if (this.editObj && this.editObj.number && this.editObj.name && this.editObj.price !== null && this.editObj.amount !== null) {
      this.transferOrderDto.items[index] = { ...this.editObj };
      this.editIndex = -1;
    }
  }

  cancelItem(index: number) {
    this.editObj = {};
    if (this.isAdding) {
      this.transferOrderDto.items = this.transferOrderDto.items.splice(0, index);
      this.isAdding = false;
    }
    this.editIndex = -1;
  }

  submit() {
    if (!this.transferOrderDto.items || this.transferOrderDto.items.length <= 0) {
      this.msg.warning('请添加调拨项');
      return;
    }

    if (this.mode === 'Add') {
      this.warehouseService.addTransferOrder(this.transferOrderDto)
        .subscribe(res => {
          if (res && res.type === AjaxResultType.success) {
            this.msg.success('添加成功');
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    } else {
      this.warehouseService.updateTransferOrder(this.transferOrderDto)
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
        id: this.transferOrderDto.id,
        type: TemplateType.transferOrder
      }
    });
  }

  startExport() {
    if (this.mode === 'Add') {
      return;
    }

    this.router.navigate(['dashboard', 'document', 'document-window'], {
      queryParams: {
        id: this.transferOrderDto.id,
        type: TemplateType.transferOrder
      }
    });
  }
}
