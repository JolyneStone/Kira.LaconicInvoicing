import { Component, OnInit, AfterViewInit, ChangeDetectorRef, Injector, ViewChild } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { SaleService } from '@shared/business/services/sale.service';
import { I18NService } from '@core/i18n/i18n.service';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';
import { CustomerDto, QueryCondition } from '@shared/business/app.model';
import { DataListComponent } from '@shared/business/components/data-list/data-list.component';
import { DataListMutilServiceComponent } from '@shared/business/components/data-list-mutil-service/data-list-mutil-service.component';

@Component({
  selector: 'app-customer-edit',
  templateUrl: './customer-edit.component.html',
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
export class CustomerEditComponent extends ComponentBase implements OnInit, AfterViewInit {

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
    this._authConfig = new AuthConfig('Root.Sale.Sale.Customer', []);
    this.customerDto = new CustomerDto();
    this.isVisible = false;
  }

  title: string;
  isVisible = false;
  mode: 'Add' | 'Update' = 'Add';
  customerDto: CustomerDto;
  types: any[];
  private _authConfig: AuthConfig;
  @ViewChild('dataList') dataList: DataListComponent;

  protected AuthConfig(): AuthConfig {
    return this._authConfig;
  }

  async show(mode: 'Add' | 'Update', dto?: CustomerDto) {
    this.mode = mode;
    this.isVisible = true;
    this.customerDto = new CustomerDto();
    if (mode === 'Add') {
      this._authConfig.funcs = [mode, 'GetNewNumber'];
      this.title = this.i18nService.fanyi('app.dashboard.add-customer');
    } else {
      this._authConfig.funcs = [mode, 'GetProducts', 'UpdateProducts'];
      this.title = this.i18nService.fanyi('app.dashboard.edit-customer');
      this.customerDto = dto;
    }

    await super.checkAuth();
    if (this.auth.GetProducts) {
      this.dataList.read = (query) => this.handleGetProducts(query);
    }
    if (this.auth.UpdateProducts) {
      this.dataList.submit = () => this.handleProducts();
    }

    this.dataList.searchAll = () => this.handleGetProductsAll();
    this.dataList.cancel = () => this.cancel();

    if (mode === 'Update'){
      this.dataList.loadData(new QueryCondition(), [
        { name: this.i18nService.fanyi('app.dashboard.product-number'), get: d => d.number },
        { name: this.i18nService.fanyi('app.dashboard.product-name'), get: d => d.name },
        { name: this.i18nService.fanyi('app.dashboard.product-type'), get: d => d.type }
      ]);
    }
  }

  ngOnInit() {
    this.bsr.getBaseDataValuesByType('CUSTOMERTYPE').subscribe(res => {
      this.types = res;
      this.cdr.detectChanges();
    });
  }

  ngAfterViewInit() {
  }

  getNewNumber() {
    this.saleService.getCustomerNewNumber()
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.customerDto.number = res.data;
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
      })
  }

  cancel() {
    this.dataList.data = [];
    this.isVisible = false;
  }

  ok() {
    if (this.mode === 'Add') {
      this.saleService.addCustomer(this.customerDto)
        .subscribe(res => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('添加客户信息成功');
            this.dataList.data = [];
            this.isVisible = false;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    } else {
      this.saleService.updateCustomer(this.customerDto)
        .subscribe(res => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('更新客户信息成功');
            this.isVisible = true;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }

  handleGetProducts(query: QueryCondition) {
    if (!this.auth.GetProduct) {
      if (!query.filters)
        query.filters = [];
      query.filters.push({ field: 'CustomerId', conditionOp: 'Equal', value: this.customerDto.id, logicOp: 'AndAlso' });
      this.saleService.getProductsByCustomer(query)
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

  handleProducts() {
    const ids = this.dataList.data.filter(v => this.dataList.mapOfCheckedId[v.id]).map(v => v.id);
    this.saleService.updateProductsByCustomer(this.customerDto.id, ids)
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

  handleGetProductsAll() {
    const modal = this.modalService.create({
      nzTitle: this.i18nService.fanyi('app.dashboard.customer'),
      nzContent: DataListMutilServiceComponent,
      nzComponentParams: {
        serverAuthConfig: new AuthConfig('Root.Sale.Sale.Product', ['Search']),
        columns: [
          { name: this.i18nService.fanyi('app.dashboard.product-number'), get: d => d.number },
          { name: this.i18nService.fanyi('app.dashboard.product-name'), get: d => d.name },
          { name: this.i18nService.fanyi('app.dashboard.product-type'), get: d => d.type }
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
          }
        },
        {
          label: this.i18nService.fanyi('app.ok'),
          type: 'primary',
          onClick: componentInstance => {
            const products = componentInstance.data.filter(v => componentInstance.mapOfCheckedId[v.id]);
            products.forEach(m => {
              if (this.dataList.data.findIndex(v=> v.id === m.id) < 0) {
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
