import { Injectable, Injector } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd';
import { CacheService } from '@delon/cache';
import { Observable } from 'rxjs';


@Injectable()
export class BusinessService {
  private cache: CacheService;

  constructor(injector: Injector) {
    this.cache = injector.get(CacheService);
  }

  //#region 基础数据

  /**
   * 设置基础数据缓存
   * @param type
   * @param value
   */
  setCacheBaseDataValuesByType(type: string, value: any): void {
    const key = `api/basedata/basedata/getvalues/?type=${type.toUpperCase()}`;
    this.cache.remove(key);
    this.cache.set(key, Observable.create(value), { type: 's', expire: 60 * 10 });
  }

  /**
   * 设置基础数据缓存
   * @param type
   * @param value
   */
  setCacheBaseDataValueByType(type: string, value: any): void {
    const key = `api/basedata/basedata/getvalue/?type=${type.toUpperCase()}`;
    this.cache.remove(key);
    this.cache.set(key, Observable.create(value), { type: 's', expire: 60 * 10 });
  }

  /**
   * 获取基础数据
   */
  getBaseDataValuesByType(type: string): Observable<any[]> {
    const key = `api/basedata/basedata/getvalues/?type=${type.toUpperCase()}`;
    return this.cache.get<any[]>(key, { mode: 'promise', expire: 60 * 10 });
  }

  /**
   * 获取基础数据
   */
  getBaseDataValueByType(type: string): Observable<any[]> {
    const key = `api/basedata/basedata/getvalue/?type=${type.toUpperCase()}`;
    return this.cache.get<any[]>(key, { mode: 'promise', expire: 60 * 10 });
  }

  // /**
  //  * 获取编号类型集合
  //  */
  // getNumberTypes(): Promise<string[]> {
  //   const key = 'api/basedata/basedata/getnumbertypes';
  //   return this.cache.get<any[]>(key, { mode: 'promise', expire: 60 * 10 }).toPromise();
  // }

  // /**
  //  * 获取单位集合
  //  */
  // getUnitTypes(): Promise<string[]> {
  //   const key = 'api/basedata/basedata/getunittypes';
  //   return this.cache.get<any[]>(key, { mode: 'promise', expire: 60 * 10 }).toPromise();
  // }

  //#endregion
}
