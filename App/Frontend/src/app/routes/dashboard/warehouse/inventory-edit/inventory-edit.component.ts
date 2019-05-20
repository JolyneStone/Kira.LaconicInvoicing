import { Component, OnInit, Injector } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { WarehouseService } from '@shared/business/services/warehouse.service';
import { I18NService } from '@core/i18n/i18n.service';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';
import { WarehouseDto, InventoryDto, GoodsCategory, QueryCondition, InboundReceiptItemDto } from '@shared/business/app.model';
import { PurchaseService } from '../../../../shared/business/services/purchase.service';
import { DataListSingleServiceComponent } from '@shared/business/components/data-list-single-service/data-list-single-service.component';

@Component({
  selector: 'app-inventory-edit',
  templateUrl: './inventory-edit.component.html',
  styles: [
    `
      ::ng-deep .ant-modal-content{
        width: 720px;
      }
      ::ng-deep .vertical-center-modal {
        display: flex;
        align-items: center;
        justify-content: center;
      }

      ::ng-deep .vertical-center-modal .ant-modal {
        top: 0;
      }

      ::ng-deep .ant-tabs {
        overflow: inherit !important;
      }
    `
  ]
})
export class InventoryEditComponent extends ComponentBase implements OnInit {

  constructor(
    private msg: NzMessageService,
    private purchaseService: PurchaseService,
    private warehouseService: WarehouseService,
    private modalService: NzModalService,
    private i18nService: I18NService,
    injector: Injector,
  ) {
    super(injector);
    this._authConfig = new AuthConfig('Root.Warehouse.Warehouse.Inventory', []);
    this.inventoryDto = new WarehouseDto();
    this.isVisible = false;
    this.goodsCategoryKeys = Object.keys(this.goodsCategoryStatus).filter(f => !isNaN(Number(f)));
  }

  title: string;
  isVisible = false;
  mode: 'Add' | 'Update' = 'Add';
  inventoryDto: InventoryDto;
  goodsCategoryKeys: any[];
  goodsCategoryStatus: typeof GoodsCategory = GoodsCategory;
  get goodsCategoryData() {
    return this.inventoryDto.goodsCategory;
  }
  set goodsCategoryData(goodsCategory: GoodsCategory) {
    const value = Number(goodsCategory); // 获取到的值为字符串
    if (this.inventoryDto.goodsCategory !== value) {
      this.inventoryDto.number = null;
      this.inventoryDto.name = null;
      this.inventoryDto.amount = 0;
    }
    this.inventoryDto.goodsCategory = value;
  }
  private _authConfig: AuthConfig;

  protected AuthConfig(): AuthConfig {
    return this._authConfig;
  }

  async show(mode: 'Add' | 'Update', dto?: InventoryDto) {
    this.mode = mode;
    this.isVisible = true;
    this.inventoryDto = dto;
    if (mode === 'Add') {
      this._authConfig.funcs = [mode];
      this.title = this.i18nService.fanyi('app.dashboard.add-inventory');
      this.inventoryDto.goodsCategory = GoodsCategory.material;
    } else {
      this._authConfig.funcs = [mode, 'GetInventory', 'UpdateInventory'];
      this.title = this.i18nService.fanyi('app.dashboard.edit-inventory');
    }

    await super.checkAuth();
  }

  ngOnInit() {
  }

  selectItem() {
    if (this.inventoryDto.goodsCategory === GoodsCategory.material) {
      this.selectMaterialItem();
    } else {
      this.selectProductItem();
    }
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

            if (!this.inventoryDto) {
              this.inventoryDto = new InventoryDto();
              this.inventoryDto.goodsCategory = GoodsCategory.product;
            }

            this.inventoryDto.number = data.number;
            this.inventoryDto.name = data.name;
            componentInstance.close();
          }
        }
      ]
    });
  }

  selectMaterialItem() {
    const modal = this.modalService.create({
      nzTitle: this.i18nService.fanyi('app.dashboard.material'),
      nzContent: DataListSingleServiceComponent,
      nzComponentParams: {
        serverAuthConfig: new AuthConfig('Root.Purchase.Purchase.Material', ['Search']),
        columns: [
          { name: this.i18nService.fanyi('app.dashboard.material-name'), get: d => d.name },
          { name: this.i18nService.fanyi('app.dashboard.material-number'), get: d => d.number },
          { name: this.i18nService.fanyi('app.dashboard.material-price'), get: d => d.price },
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

            if (!this.inventoryDto) {
              this.inventoryDto = new InventoryDto();
              this.inventoryDto.goodsCategory = GoodsCategory.material;
            }

            this.inventoryDto.number = data.number;
            this.inventoryDto.name = data.name;
            componentInstance.close();
          }
        }
      ]
    });
  }

  cancel() {
    this.isVisible = false;
  }

  ok() {
    if (this.mode === 'Add') {
      if(this.inventoryDto.number && this.inventoryDto.name && this.inventoryDto.amount !== null && this.inventoryDto.amount > 0){
      this.warehouseService.addInventory(this.inventoryDto)
        .subscribe(res => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('添加库存信息成功');
            this.isVisible = true;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
      }
    } else {
      if(this.inventoryDto.amount && this.inventoryDto.amount> 0){
      this.warehouseService.updateInventory(this.inventoryDto)
        .subscribe(res => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('更新库存信息成功');
            this.isVisible = true;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
      }
    }
  }
}
