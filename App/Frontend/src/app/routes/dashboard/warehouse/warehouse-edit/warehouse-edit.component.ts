import { Component, OnInit, AfterViewInit, ChangeDetectorRef, Injector, ViewChild } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { WarehouseService } from '@shared/business/services/warehouse.service';
import { I18NService } from '@core/i18n/i18n.service';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';
import { WarehouseDto, QueryCondition } from '@shared/business/app.model';
import { DataListComponent } from '@shared/business/components/data-list/data-list.component';
import { WarehouseStatus } from '../../../../shared/business/app.model';

@Component({
  selector: 'app-warehouse-edit',
  templateUrl: './warehouse-edit.component.html',
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
export class WarehouseEditComponent extends ComponentBase implements OnInit {

  constructor(
    private msg: NzMessageService,
    private warehouseService: WarehouseService,
    private i18nService: I18NService,
    injector: Injector,
  ) {
    super(injector);
    this._authConfig = new AuthConfig('Root.Warehouse.Warehouse.Warehouse', []);
    this.warehouseDto = new WarehouseDto();
    this.isVisible = false;
  }

  title: string;
  isVisible = false;
  mode: 'Add' | 'Update' = 'Add';
  warehouseDto: WarehouseDto;
  types: any[];
  statusKeys: any[];
  warehouseStatus: typeof WarehouseStatus = WarehouseStatus;
  private _authConfig: AuthConfig;

  protected AuthConfig(): AuthConfig {
    return this._authConfig;
  }

  async show(mode: 'Add' | 'Update', dto?: WarehouseDto) {
    this.mode = mode;
    this.isVisible = true;
    this.warehouseDto = new WarehouseDto();
    if (mode === 'Add') {
      this._authConfig.funcs = [mode, 'GetNewNumber'];
      this.title = this.i18nService.fanyi('app.dashboard.add-warehouse');
    } else {
      this._authConfig.funcs = [mode, 'GetWarehouses', 'UpdateWarehouses'];
      this.title = this.i18nService.fanyi('app.dashboard.edit-warehouse');
      this.warehouseDto = dto;
    }

    await super.checkAuth();
  }

  ngOnInit() {
    this.statusKeys = Object.keys(this.warehouseStatus).filter(f => !isNaN(Number(f)));
  }

  getNewNumber() {
    this.warehouseService.getWarehouseNewNumber()
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.warehouseDto.number = res.data;
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
      this.warehouseService.addWarehouse(this.warehouseDto)
        .subscribe(res => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('添加仓库信息成功');
            this.isVisible = true;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    } else {
      this.warehouseService.updateWarehouse(this.warehouseDto)
        .subscribe(res => {
          if (res.type === AjaxResultType.success) {
            this.msg.success('更新仓库信息成功');
            this.isVisible = true;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });
    }
  }
}
