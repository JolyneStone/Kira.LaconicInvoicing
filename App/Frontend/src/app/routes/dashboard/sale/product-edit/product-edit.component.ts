import { Component, OnInit, ChangeDetectorRef, Injector, ViewChild } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { SaleService } from '@shared/business/services/sale.service';
import { I18NService } from '@core/i18n/i18n.service';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';
import { ProductDto, QueryCondition } from '@shared/business/app.model';
import { DataListComponent } from '@shared/business/components/data-list/data-list.component';
import { zip } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { DataListMutilServiceComponent } from '@shared/business/components/data-list-mutil-service/data-list-mutil-service.component';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
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
export class ProductEditComponent extends ComponentBase implements OnInit {
  constructor(
    private msg: NzMessageService,
    private bsr: BusinessService,
    private cdr: ChangeDetectorRef,
    private saleService: SaleService,
    private i18nService: I18NService,
    private modalService: NzModalService,
    injector: Injector,
  ) {
    super(injector);
    this._authConfig = new AuthConfig('Root.Sale.Sale.Product', []);
    this.productDto = new ProductDto();
    this.isVisible = false;
  }

  title: string;
  isVisible = false;
  mode: 'Add' | 'Update' = 'Add';
  productDto: ProductDto;
  types: any[];
  units: any[];
  private _authConfig: AuthConfig;
  @ViewChild('dataList') dataList: DataListComponent;

  protected AuthConfig(): AuthConfig {
    return this._authConfig;
  }

  async show(mode: 'Add' | 'Update', dto?: ProductDto) {
    this.mode = mode;
    this.isVisible = true;
    this.productDto = new ProductDto();
    if (mode === 'Add') {
      this._authConfig.funcs = [mode, 'GetNewNumber'];
      this.title = this.i18nService.fanyi('app.dashboard.add-product');
    } else {
      this._authConfig.funcs = [mode, 'GetCustomers', 'UpdateCustomers'];
      this.title = this.i18nService.fanyi('app.dashboard.edit-product');
      this.productDto = dto;
    }

    await super.checkAuth();
    if (this.auth.GetCustomers) {
      this.dataList.read = (query) => this.handleGetCustomers(query);
    }
    if (this.auth.UpdateCustomers) {
      this.dataList.submit = () => this.handleCustomers();
    }

    this.dataList.searchAll = () => this.handleGetCustomersAll();
    this.dataList.cancel = () => this.cancel();

    if (mode === 'Update') {
      this.dataList.loadData(new QueryCondition(), [
        { name: this.i18nService.fanyi('app.dashboard.customer-number'), get: d => d.number },
        { name: this.i18nService.fanyi('app.dashboard.customer-name'), get: d => d.name },
        { name: this.i18nService.fanyi('app.dashboard.customer-type'), get: d => d.type }
      ]);
    }
  }

  ngOnInit() {
    zip(
      this.bsr.getBaseDataValuesByType('PRODUCTTYPE'),
      this.bsr.getBaseDataValuesByType('UNITTYPE')
    )
    .pipe(
      catchError(([types, units]) => {
        return [types, units];
      }),
    )
      .subscribe(([types, units]: any) => {
        this.types = types;
        this.units = units;
        this.cdr.detectChanges();
      });
  }

  getNewNumber() {
    this.saleService.getProductNewNumber()
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.productDto.number = res.data;
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
      });
  }

  cancel() {
    this.isVisible = false;
  }

  ok() {
    if (this.mode === 'Add') {
      this.saleService.addProduct(this.productDto)
        .subscribe(res => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('添加产品信息成功');
            this.isVisible = true;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    } else {
      this.saleService.updateProduct(this.productDto)
        .subscribe(res => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('更新产品信息成功');
            this.isVisible = true;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }

  handleGetCustomers(query: QueryCondition) {
    if (!this.auth.GetCustomer) {
      if (!query.filters)
        query.filters = [];
      query.filters.push({ field: 'ProductId', conditionOp: 'Equal', value: this.productDto.id, logicOp: 'AndAlso' });
      this.saleService.getCustomersByProduct(query)
        .subscribe(res => {
          if (res.type === AjaxResultType.success) {
            this.isVisible = true;
            this.dataList.data = res.data.rows;
            this.dataList.total = res.data.total;
            this.dataList.checkAll(true);
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }

  handleCustomers() {
    const ids = this.dataList.data.filter(v => this.dataList.mapOfCheckedId[v.id]).map(v => v.id);
    this.saleService.updateCustomersByProduct(this.productDto.id, ids)
      .subscribe(res => {
        if (res.type === AjaxResultType.success) {
          this.ok();
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
      });
  }

  handleCancel() {
    this.cancel();
  }

  handleGetCustomersAll() {
    const modal = this.modalService.create({
      nzTitle: this.i18nService.fanyi('app.dashboard.product'),
      nzContent: DataListMutilServiceComponent,
      nzComponentParams: {
        serverAuthConfig: new AuthConfig('Root.Sale.Sale.Customer', ['Search']),
        columns: [
          { name: this.i18nService.fanyi('app.dashboard.customer-number'), get: d => d.number },
          { name: this.i18nService.fanyi('app.dashboard.customer-name'), get: d => d.name },
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
            const products = componentInstance.data.filter(v => componentInstance.mapOfCheckedId[v.id]);
            products.forEach(m => {
              if (this.dataList.data.indexOf(m) < 0) {
                this.dataList.data.push(m);
                this.dataList.mapOfCheckedId[m.id] = true;
                this.cdr.detectChanges();
              }
            });

            componentInstance.close();
          }
        }
      ]
    });
  }
}
