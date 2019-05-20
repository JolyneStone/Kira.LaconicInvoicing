import { AjaxResult } from './../../osharp/osharp.model';
import { Injectable, Injector } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { StatisticsPeriod } from '../app.model';

@Injectable()
export class StatisticsService {
  private http: _HttpClient;

  constructor(injector: Injector) {
    this.http = injector.get(_HttpClient);
  }

  getPurchaseTrendStatisticsAnalysis(startDate: Date, endDate: Date, period: StatisticsPeriod) {
    return this.http.get<AjaxResult>('api/purchase/statistics/trendstatisticsanalysis', { startDate: startDate.toLocaleDateString(), endDate: endDate.toLocaleDateString(), period: period });
  }

  getPurchaseGeneralStatisticsAnalysis(startDate: Date, endDate: Date) {
    return this.http.get<AjaxResult>('api/purchase/statistics/generalstatisticsanalysis', { startDate: startDate.toLocaleDateString(), endDate: endDate.toLocaleDateString() });
  }

  getSaleTrendStatisticsAnalysis(startDate: Date, endDate: Date, period: StatisticsPeriod) {
    return this.http.get<AjaxResult>('api/sale/statistics/trendstatisticsanalysis', { startDate: startDate.toLocaleDateString(), endDate: endDate.toLocaleDateString(), period: period });
  }

  getSaleGeneralStatisticsAnalysis(startDate: Date, endDate: Date) {
    return this.http.get<AjaxResult>('api/sale/statistics/generalstatisticsanalysis', { startDate: startDate.toLocaleDateString(), endDate: endDate.toLocaleDateString() });
  }

  getStatisticsInventoryAnalysis(id?: string) {
    return this.http.get<AjaxResult>('api/warehouse/statistics/StatisticsInventoryAnalysis', id ? { id: id } : null);
  }

  getWarehouses() {
    return this.http.get<AjaxResult>('api/warehouse/statistics/getwarehouses');
  }
}