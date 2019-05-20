import { MaterialQueryDto } from './../../../../shared/business/app.model';
import { I18NService } from './../../../../core/i18n/i18n.service';
import { Component, OnInit, ChangeDetectorRef, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { SettingsService } from '@delon/theme/public_api';
import { NzMessageService, NzModalRef, NzModalService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { AuthConfig, AjaxResultType, AjaxResult } from '@shared/osharp/osharp.model';
import { VendorDto, QueryCondition } from '../../../../shared/business/app.model';
import { FormGroup } from '@angular/forms';
import { DataListComponent } from '../../../../shared/business/components/data-list/data-list.component';
import { DataListMutilServiceComponent } from '../../../../shared/business/components/data-list-mutil-service/data-list-mutil-service.component';
import { PurchaseService } from '@shared/business/services/purchase.service';

@Component({
  selector: 'app-vendor-edit',
  templateUrl: './vendor-edit.component.html',
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
export class VendorEditComponent extends ComponentBase implements OnInit, AfterViewInit {

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
    this._authConfig = new AuthConfig('Root.Purchase.Purchase.Vendor', []);
    this.vendorDto = new VendorDto();
    this.isVisible = false;
  }

  title: string;
  isVisible = false;
  mode: 'Add' | 'Update' = 'Add';
  vendorDto: VendorDto;
  types: any[];
  private _authConfig: AuthConfig;
  @ViewChild('dataList') dataList: DataListComponent;

  protected AuthConfig(): AuthConfig {
    return this._authConfig;
  }

  async show(mode: 'Add' | 'Update', dto?: VendorDto) {
    this.mode = mode;
    this.isVisible = true;
    this.vendorDto = new VendorDto();
    if (mode === 'Add') {
      this._authConfig.funcs = [mode, 'GetNewNumber'];
      this.title = this.i18nService.fanyi('app.dashboard.add-vendor');
    } else {
      this._authConfig.funcs = [mode, 'GetMaterials', 'UpdateMaterials'];
      this.title = this.i18nService.fanyi('app.dashboard.edit-vendor');
      this.vendorDto = dto;
    }

    await super.checkAuth();
    if (this.auth.GetMaterials) {
      this.dataList.read = (query) => this.handleGetMaterials(query);
    }
    if (this.auth.UpdateMaterials) {
      this.dataList.submit = () => this.handleMaterials();
    }

    this.dataList.searchAll = () => this.handleGetMaterialsAll();
    this.dataList.cancel = () => this.cancel();

    if (mode === 'Update'){
      this.dataList.loadData(new QueryCondition(), [
        { name: this.i18nService.fanyi('app.dashboard.material-number'), get: d => d.number },
        { name: this.i18nService.fanyi('app.dashboard.material-name'), get: d => d.name },
        { name: this.i18nService.fanyi('app.dashboard.material-type'), get: d => d.type }
      ]);
    }
  }

  ngOnInit() {
    this.bsr.getBaseDataValuesByType('VENDORTYPE').subscribe(res => {
      this.types = res;
      this.cdr.detectChanges();
    });
  }

  ngAfterViewInit() {
  }

  getNewNumber() {
    this.purchaseService.getVendorNewNumber()
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.vendorDto.number = res.data;
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
      })
  }

  cancel() {
    this.isVisible = false;
  }

  ok() {
    if (this.mode === 'Add') {
      this.purchaseService.addVendor(this.vendorDto)
        .subscribe(res => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('添加供应商信息成功');
            this.isVisible = true;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    } else {
      this.purchaseService.updateVendor(this.vendorDto)
        .subscribe(res => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('更新供应商信息成功');
            this.isVisible = true;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }

  handleGetMaterials(query: QueryCondition) {
    if (!this.auth.GetMaterial) {
      if (!query.filters)
        query.filters = [];
      query.filters.push({ field: 'VendorId', conditionOp: 'Equal', value: this.vendorDto.id, logicOp: 'AndAlso' });
      this.purchaseService.getMaterialsByVendor(query)
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

  handleMaterials() {
    const ids = this.dataList.data.filter(v => this.dataList.mapOfCheckedId[v.id]).map(v => v.id);
    this.purchaseService.updateMaterialsByVendor(this.vendorDto.id, ids)
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

  handleGetMaterialsAll() {
    const modal = this.modalService.create({
      nzTitle: this.i18nService.fanyi('app.dashboard.vendor'),
      nzContent: DataListMutilServiceComponent,
      nzComponentParams: {
        serverAuthConfig: new AuthConfig('Root.Purchase.Purchase.Material', ['Search']),
        columns: [
          { name: this.i18nService.fanyi('app.dashboard.material-number'), get: d => d.number },
          { name: this.i18nService.fanyi('app.dashboard.material-name'), get: d => d.name },
          { name: this.i18nService.fanyi('app.dashboard.material-type'), get: d => d.type }
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
