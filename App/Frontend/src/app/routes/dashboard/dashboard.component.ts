import { AuthConfig } from './../../shared/osharp/osharp.model';
import { Inject } from '@angular/compiler/src/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { I18NService } from '@core/i18n/i18n.service';
import { Component, OnInit, Injector } from '@angular/core';

import * as G2 from '@antv/g2';
import * as DataSet from '@antv/data-set';
import * as moment from 'moment';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'admin-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent extends ComponentBase implements OnInit {
  dateFormat = 'yyyy/MM/dd';
  pickerRanges = {
    '今天': [moment().toDate(), moment().toDate()],
    '昨天': [moment().subtract(1, 'days').toDate(), moment().subtract(1, 'days').toDate()],
    '最近7天': [moment().subtract(6, 'days').toDate(), moment().toDate()],
    '最近30天': [moment().subtract(29, 'days').toDate(), moment().toDate()],
    '本月': [moment().startOf("month").toDate(), moment().endOf("month").toDate()],
    '上月': [moment().subtract(1, "months").startOf("month").toDate(), moment().subtract(1, "months").endOf("month").toDate()],
    '全部': [moment("1-1-1", "MM-DD-YYYY").toDate(), moment("12-31-9999", "MM-DD-YYYY").toDate()]
  };

  summaries: Summary[] = [];
  userLineChart: any;

  constructor(
    private http: HttpClient,
    private i18nService: I18NService,
    injector: Injector) {
    super(injector);
  }

  async ngOnInit() {
    await super.checkAuth();
    this.rangePickerChange(this.pickerRanges.最近30天);
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig('Root.Admin.Dashboard', [
      'SummaryData',
      'LineData',
    ]);
  }

  rangePickerChange(e) {
    if (e.length == 0) {
      return;
    }
    let start = e[0].toLocaleDateString(), end = e[1].toLocaleDateString();
    this.summaryData(start, end);
    this.userLine(start, end);
  }

  /**统计数据 */
  summaryData(start, end) {
    if (this.auth.SummaryData) {
      let url = `api/admin/dashboard/SummaryData?start=${start}&end=${end}`;
      this.http.get(url).subscribe((res: any) => {
        this.summaries = [];
        this.summaries.push({ data: `${res.users.validCount} / ${res.users.totalCount}`, text: this.i18nService.fanyi('app.dashboard.total-user-info'), bgColor: 'bg-primary' });
        this.summaries.push({ data: `${res.roles.adminCount} / ${res.roles.totalCount}`, text: this.i18nService.fanyi('app.dashboard.total-role-info'), bgColor: 'bg-success' });
        this.summaries.push({ data: `${res.modules.siteCount} / ${res.modules.adminCount} / ${res.modules.totalCount}`, text: this.i18nService.fanyi('app.dashboard.total-module-info'), bgColor: 'bg-orange' });
        this.summaries.push({ data: `${res.functions.controllerCount} / ${res.functions.totalCount}`, text: this.i18nService.fanyi('app.dashboard.total-function-info'), bgColor: 'bg-magenta' });
      });
    }
  }

  /**用户曲线 */
  userLine(start, end) {
    if (this.auth.LineData) {
      let url = `api/admin/dashboard/LineData?start=${start}&end=${end}`;
      this.http.get(url).subscribe((res: any) => {
        if (this.userLineChart != null) {
          this.userLineChart.destroy();
        }
        let dv = new DataSet().createView().source(res);
        dv.transform({ type: 'fold', fields: ['dailyCount', 'dailySum'], key: 'key', value: 'value' });
        this.userLineChart = new G2.Chart({ container: 'user-line', forceFit: true, height: 300 });
        let chart = this.userLineChart;
        chart.source(dv, { Date: { range: [0, 1] } });
        chart.tooltip({ crosshairs: { type: 'line' } });
        chart.axis('date', { label: { formatter: val => new Date(val).toLocaleDateString() } });
        chart.line().position('date*value').color('key').shape('smooth');
        chart.point().position('date*value').color('key').size(4).shape('circle').style({ stroke: '#fff', lineWidth: 1 });
        chart.render();
      });
    }
  }
}

export class Summary {
  data: string;
  text: string;
  bgColor: string = 'bg-primary';
}
