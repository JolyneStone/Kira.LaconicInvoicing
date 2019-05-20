import { Component, OnInit, AfterViewInit, ChangeDetectorRef, Injector, ViewChild } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { PurchaseService } from '@shared/business/services/purchase.service';
import { I18NService } from '@core/i18n/i18n.service';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';
import { QueryCondition, MaterialDto } from '@shared/business/app.model';
import { DataListComponent } from '@shared/business/components/data-list/data-list.component';
import { DataListMutilServiceComponent } from '@shared/business/components/data-list-mutil-service/data-list-mutil-service.component';
import { zip } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-material-edit',
  templateUrl: './material-edit.component.html',
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
export class MaterialEditComponent extends ComponentBase implements OnInit {
  constructor(
    private msg: NzMessageService,
    private bsr: BusinessService,
    private cdr: ChangeDetectorRef,
    private purchaseService: PurchaseService,
    private i18nService: I18NService,
    private modalService: NzModalService,
    injector: Injector,
  ) {
    super(injector);
    this._authConfig = new AuthConfig('Root.Purchase.Purchase.Material', []);
    this.materialDto = new MaterialDto();
    this.isVisible = false;
  }

  title: string;
  isVisible = false;
  mode: 'Add' | 'Update' = 'Add';
  materialDto: MaterialDto;
  types: any[];
  units: any[];
  private _authConfig: AuthConfig;
  @ViewChild('dataList') dataList: DataListComponent;

  protected AuthConfig(): AuthConfig {
    return this._authConfig;
  }

  async show(mode: 'Add' | 'Update', dto?: MaterialDto) {
    this.mode = mode;
    this.isVisible = true;
    this.materialDto = new MaterialDto();
    if (mode === 'Add') {
      this._authConfig.funcs = [mode, 'GetNewNumber'];
      this.title = this.i18nService.fanyi('app.dashboard.add-material');
    } else {
      this._authConfig.funcs = [mode, 'GetVendors', 'UpdateVendors'];
      this.title = this.i18nService.fanyi('app.dashboard.edit-material');
      this.materialDto = dto;
    }

    await super.checkAuth();
    if (this.auth.GetVendors) {
      this.dataList.read = (query) => this.handleGetVendors(query);
    }
    if (this.auth.UpdateVendors) {
      this.dataList.submit = () => this.handleVendors();
    }

    this.dataList.searchAll = () => this.handleGetVendorsAll();
    this.dataList.cancel = () => this.cancel();

    if (mode === 'Update') {
      this.dataList.loadData(new QueryCondition(), [
        { name: this.i18nService.fanyi('app.dashboard.vendor-number'), get: d => d.number },
        { name: this.i18nService.fanyi('app.dashboard.vendor-name'), get: d => d.name },
        { name: this.i18nService.fanyi('app.dashboard.vendor-type'), get: d => d.type }
      ]);
    }
  }

  ngOnInit() {
    zip(
      this.bsr.getBaseDataValuesByType('MATERIALTYPE'),
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
    this.purchaseService.getMaterialNewNumber()
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.materialDto.number = res.data;
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
      this.purchaseService.addMaterial(this.materialDto)
        .subscribe(res => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('添加原料信息成功');
            this.isVisible = true;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    } else {
      this.purchaseService.updateMaterial(this.materialDto)
        .subscribe(res => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('更新原料信息成功');
            this.isVisible = true;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }

  handleGetVendors(query: QueryCondition) {
    if (!this.auth.GetVendor) {
      if (!query.filters)
        query.filters = [];
      query.filters.push({ field: 'MaterialId', conditionOp: 'Equal', value: this.materialDto.id, logicOp: 'AndAlso' });
      this.purchaseService.getVendorsByMaterial(query)
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

  handleVendors() {
    const ids = this.dataList.data.filter(v => this.dataList.mapOfCheckedId[v.id]).map(v => v.id);
    this.purchaseService.updateVendorsByMaterial(this.materialDto.id, ids)
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

  handleGetVendorsAll() {
    const modal = this.modalService.create({
      nzTitle: this.i18nService.fanyi('app.dashboard.material'),
      nzContent: DataListMutilServiceComponent,
      nzComponentParams: {
        serverAuthConfig: new AuthConfig('Root.Purchase.Purchase.Vendor', ['Search']),
        columns: [
          { name: this.i18nService.fanyi('app.dashboard.vendor-number'), get: d => d.number },
          { name: this.i18nService.fanyi('app.dashboard.vendor-name'), get: d => d.name },
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
            const materials = componentInstance.data.filter(v => componentInstance.mapOfCheckedId[v.id]);
            materials.forEach(m => {
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
