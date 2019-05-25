import { AjaxResult, AjaxResultType } from './../../../../shared/osharp/osharp.model';
import { StatisticsPeriod } from './../../../../shared/business/app.model';
import { StatisticsService } from './../../../../shared/business/services/statistics.service';
import { Component, OnInit, Injector } from '@angular/core';
import { AuthConfig } from '../../../../shared/osharp/osharp.model';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { NzMessageService } from 'ng-zorro-antd';
import { I18NService } from '@core/i18n/i18n.service';
import { zip } from 'rxjs/operators';
import { addAllToArray } from '@angular/core/src/render3/util';
import { ColumnChartDto, PieChartDto } from '../../../../shared/business/app.model';

@Component({
  selector: 'app-purchase-statistics',
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

  periodKeys: any[];
  periodStatus: typeof StatisticsPeriod = StatisticsPeriod;
  period: StatisticsPeriod;
  trendStartDate: Date;
  trendEndDate: Date;
  trendAmountData: any[];
  trendQuantityData: any[];
  generalDates: Date[];
  generalVendorAmountData: any[];
  generalVendorQuantityData: any[];
  generalMaterialAmountData: any[];
  generalMaterialQuantityData: any[];
  generalVendorAmountTotal: string;
  generalVendorQuantityTotal: string;
  generalMaterialAmountTotal: string;
  generalMaterialQuantityTotal: string;
  dateFormat = 'yyyy-MM-dd';
  monthFormat = 'yyyy-MM';
  yearFormat = 'yyyy';

  protected AuthConfig() {
    return new AuthConfig('Root.Purchase.Purchase.Statistics', [
      'TrendStatisticsAnalysis',
      'GeneralStatisticsAnalysis'
    ]);
  }

  async ngOnInit() {
    super.checkAuth();
    this.period = StatisticsPeriod.month;
    if (this.auth.TrendStatisticsAnalysis) {
      this.periodKeys = Object.keys(this.periodStatus).filter(f => !isNaN(Number(f)));
      this.period = StatisticsPeriod.month;
      this.trendAmountData = [];
      this.trendQuantityData = [];
      this.trendPeriodChange(this.period);
      this.trendStatistics();
    }

    if (this.auth.GeneralStatisticsAnalysis) {
      const now = new Date();
      this.generalDates = [
        new Date(now.getFullYear(), 0, 1),
        new Date(new Date().getTime() + 3600 * 24 - 1)];
      this.generalVendorAmountData = [];
      this.generalVendorQuantityData = [];
      this.generalMaterialAmountData = [];
      this.generalMaterialQuantityData = [];
      this.generalVendorAmountTotal = '';
      this.generalVendorQuantityTotal = '';
      this.generalMaterialAmountTotal = '';
      this.generalMaterialQuantityTotal = '';
      this.generalStatistics(this.generalDates);
    }
  }

  trendPeriodChange(event: StatisticsPeriod) {
    const now = new Date();
    event = Number(event) as StatisticsPeriod;
    if (event === StatisticsPeriod.month) {
      this.trendStartDate = new Date(now.getFullYear(), 0, 1);
      this.trendEndDate = new Date(new Date(now.getFullYear(), now.getMonth() + 1, 1).getTime() - 1);
    } else {
      this.trendStartDate = new Date(now.getFullYear() - 5, 0, 1);
      this.trendEndDate = new Date((new Date(now.getFullYear() + 1, 0, 1).getTime() - 1));
    }
  }

  trendStatistics() {
    if (this.trendStartDate >= this.trendEndDate) {
      this.msg.warning('开始时间必须早于结束时间！');
      return;
    } else if (this.period === StatisticsPeriod.month && this.trendStartDate.getFullYear() !== this.trendEndDate.getFullYear()) {
      this.msg.warning('时间段不能跨越年份!');
      return;
    }
    this.statisticsService.getPurchaseTrendStatisticsAnalysis(this.trendStartDate, this.trendEndDate, this.period)
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success && res.data !== null) {
          this.trendAmountData = [];
          this.trendQuantityData = [];
          const suffix = ' ' + this.i18nService.fanyi('app.' + (this.period === StatisticsPeriod.month ? 'month' : 'year'));
          const data = res.data;
          const length = (this.period === StatisticsPeriod.month ? this.trendEndDate.getMonth() - this.trendStartDate.getMonth() :
            this.trendEndDate.getFullYear() - this.trendStartDate.getFullYear()) + 1;
          const start = this.period === StatisticsPeriod.month ? (this.trendStartDate.getMonth() + 1) : this.trendStartDate.getFullYear();

          for (let _idx = 0; _idx < length; _idx++) {
            const name = (_idx + start).toString();
            const valueAmount = (data.amountColumn as ColumnChartDto).data.find(v => name === v.xpos);
            this.trendAmountData.push({ x: name + suffix, y: valueAmount ? valueAmount.ypos : 0 });

            const valueQuantity = (data.quantityColumn as ColumnChartDto).data.find(v => name === v.xpos);
            this.trendQuantityData.push({ x: name + suffix, y: valueQuantity ? valueQuantity.ypos : 0 });
          }

          console.log(this.trendAmountData);
          console.log(this.trendQuantityData);
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
      });
  }

  yuanFormat(val: number) {
    return `&yen ${val.toFixed(2)}`;
  }

  defaultFormat(val: number) {
    return val.toFixed(0).toString();
  }

  generalStatistics(event: Date[]) {
    event = event as Date[];
    if (event === null || event.length <= 1)
      return;

    this.statisticsService.getPurchaseGeneralStatisticsAnalysis(event[0], event[1])
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success && res.data !== null) {
          this.generalVendorAmountData = [];
          this.generalVendorQuantityData = [];
          this.generalMaterialAmountData = [];
          this.generalMaterialQuantityData = [];
          this.generalVendorAmountTotal = '';
          this.generalVendorQuantityTotal = '';
          this.generalMaterialAmountTotal = '';
          this.generalMaterialQuantityTotal = '';
          const data = res.data;

          let total = 0;
          if (data.vendorAmountPie && data.vendorAmountPie.data) {
            const vendorAmountPie = data.vendorAmountPie as PieChartDto;
            vendorAmountPie.data.forEach(v => total += v.ratio);
            this.generalVendorAmountTotal = this.yuanFormat(total);
            this.generalVendorAmountData = vendorAmountPie.data.map(v => {
              return { x: v.name, y: v.ratio };
            });
          }

          total = 0;
          if (data.vendorQuantityPie && data.vendorQuantityPie.data) {
            const vendorQuantityPie = data.vendorQuantityPie as PieChartDto;
            vendorQuantityPie.data.forEach(v => total += v.ratio);
            this.generalVendorQuantityTotal = this.defaultFormat(total);
            this.generalVendorQuantityData = vendorQuantityPie.data.map(v => {
              return { x: v.name, y: v.ratio };
            });
          }

          total = 0;
          if (data.materialAmountPie && data.materialAmountPie.data) {
            const materialAmountPie = data.materialAmountPie as PieChartDto;
            materialAmountPie.data.forEach(v => total += v.ratio);
            this.generalMaterialAmountTotal = this.yuanFormat(total);
            this.generalMaterialAmountData = materialAmountPie.data.map(v => {
              return { x: v.name, y: v.ratio };
            });
          }

          total = 0;
          if (data.materialQuantityPie && data.materialQuantityPie.data) {
            const materialQuantityPie = data.materialQuantityPie as PieChartDto;
            materialQuantityPie.data.forEach(v => total += v.ratio);
            this.generalMaterialQuantityTotal = this.defaultFormat(total);
            this.generalMaterialQuantityData = materialQuantityPie.data.map(v => {
              return { x: v.name, y: v.ratio };
            });
          }
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
      });
  }
}
