import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { Component, OnInit, Injector } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd';
import { StatisticsService } from '@shared/business/services/statistics.service';
import { I18NService } from '@core/i18n/i18n.service';
import { StatisticsPeriod, ColumnChartDto, PieChartDto } from '@shared/business/app.model';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-sale-statistics',
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
  generalCustomerAmountData: any[];
  generalCustomerQuantityData: any[];
  generalProductAmountData: any[];
  generalProductQuantityData: any[];
  generalCustomerAmountTotal: string;
  generalCustomerQuantityTotal: string;
  generalProductAmountTotal: string;
  generalProductQuantityTotal: string;
  dateFormat = 'yyyy-MM-dd';
  monthFormat = 'yyyy-MM';
  yearFormat = 'yyyy';

  protected AuthConfig() {
    return new AuthConfig('Root.Sale.Sale.Statistics', [
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
      this.generalCustomerAmountData = [];
      this.generalCustomerQuantityData = [];
      this.generalProductAmountData = [];
      this.generalProductQuantityData = [];
      this.generalCustomerAmountTotal = '';
      this.generalCustomerQuantityTotal = '';
      this.generalProductAmountTotal = '';
      this.generalProductQuantityTotal = '';
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
    this.statisticsService.getSaleTrendStatisticsAnalysis(this.trendStartDate, this.trendEndDate, this.period)
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

    this.statisticsService.getSaleGeneralStatisticsAnalysis(event[0], event[1])
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success && res.data !== null) {
          this.generalCustomerAmountData = [];
          this.generalCustomerQuantityData = [];
          this.generalProductAmountData = [];
          this.generalProductQuantityData = [];
          this.generalCustomerAmountTotal = '';
          this.generalCustomerQuantityTotal = '';
          this.generalProductAmountTotal = '';
          this.generalProductQuantityTotal = '';
          const data = res.data;

          let total = 0;
          if (data.customerAmountPie && data.customerAmountPie.data) {
            const customerAmountPie = data.customerAmountPie as PieChartDto;
            customerAmountPie.data.forEach(v => {
              total += v.ratio;
              this.generalCustomerAmountData.push({ x: v.name, y: v.ratio });
            });
            this.generalCustomerAmountTotal = this.yuanFormat(total);
          }

          total = 0;
          if (data.customerQuantityPie && data.customerQuantityPie.data) {
            const customerQuantityPie = data.customerQuantityPie as PieChartDto;
            customerQuantityPie.data.forEach(v => {
              total += v.ratio;
              this.generalCustomerQuantityData.push({ x: v.name, y: v.ratio });
            });
            this.generalCustomerQuantityTotal = this.defaultFormat(total);
          }

          total = 0;
          if (data.productAmountPie && data.productAmountPie.data) {
            const productAmountPie = data.productAmountPie as PieChartDto;
            productAmountPie.data.forEach(v => {
              total += v.ratio;
              this.generalProductAmountData.push({ x: v.name, y: v.ratio });
            });
            this.generalProductAmountTotal = this.yuanFormat(total);
          }

          total = 0;
          if (data.productQuantityPie && data.productQuantityPie.data) {
            const productQuantityPie = data.productQuantityPie as PieChartDto;
            productQuantityPie.data.forEach(v => {
              total += v.ratio;
              this.generalProductQuantityData.push({ x: v.name, y: v.ratio });
            });
            this.generalProductQuantityTotal = this.defaultFormat(total);
          }
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
      });
  }
}
