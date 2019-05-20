import { SimpleWarehouseDto } from './../../../../shared/business/app.model';
import { Component, OnInit, Injector } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService } from 'ng-zorro-antd';
import { StatisticsService } from '@shared/business/services/statistics.service';
import { I18NService } from '@core/i18n/i18n.service';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';
import { StatisticsPeriod, ColumnChartDto, PieChartDto } from '@shared/business/app.model';
import { zip } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-warehouse-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent extends ComponentBase implements OnInit {

  constructor(
    private msg: NzMessageService,
    private statisticsService: StatisticsService,
    private i18nService: I18NService,
    injector: Injector,
  ) {
    super(injector);
  }

  warehousePieData: any[];
  materialPieData: any[];
  productPieData: any[];
  warehousePieTotal: string;
  materialPieTotal: string;
  productPieTotal: string;
  warehouseList: SimpleWarehouseDto[];
  selectedWarehouse?: string;

  protected AuthConfig() {
    return new AuthConfig('Root.Warehouse.Warehouse.Statistics', [
      'StatisticsInventoryAnalysis',
    ]);
  }

  async ngOnInit() {
    super.checkAuth();
    this.selectedWarehouse = null;
    this.warehouseList = [];
    this.warehousePieData = [];
    this.materialPieData = [];
    this.productPieData = [];
    this.warehousePieTotal = '';
    this.materialPieTotal = '';
    this.productPieTotal = '';

    if (this.auth.StatisticsInventoryAnalysis) {
      this.statisticsService.getWarehouses()
        .subscribe(res => {
          if (res && res.type === AjaxResultType.success) {
            this.warehouseList = res.data;
          } else if (res && res.content) {
            this.msg.error(res.content);
          }
        });

      this.statisticsPie();
    }
  }

  statisticsPie(id?: string) {
    this.statisticsService.getStatisticsInventoryAnalysis(id)
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success && res.data) {
          const data = res.data;
          let total = 0;
          if (res.data.warehousePie && res.data.warehousePie.data) {
            const warehousePie = res.data.warehousePie as PieChartDto;
            warehousePie.data.forEach(v => {
              total += v.ratio;
              this.warehousePieData.push({ x: v.name, y: v.ratio });
            });

            this.warehousePieTotal = this.defaultFormat(total);
          }

          total = 0;
          if (res.data.materialPie && res.data.materialPie.data) {
            const materialPie = res.data.materialPie as PieChartDto;
            materialPie.data.forEach(v => {
              total += v.ratio;
              this.materialPieData.push({ x: v.name, y: v.ratio });
            });
            this.materialPieTotal = this.defaultFormat(total);
          }

          total = 0;
          if (res.data.productPie && res.data.productPie.data) {
            const productPie = res.data.productPie as PieChartDto;
            productPie.data.forEach(v => {
              total += v.ratio;
              this.productPieData.push({ x: v.name, y: v.ratio });
            });
            this.productPieTotal = this.defaultFormat(total);
          }
        }
      });
  }

  defaultFormat(val: number) {
    return val.toFixed(0).toString();
  }
}
