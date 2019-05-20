import { Component, OnInit, Injector, Input, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { _HttpClient } from '@delon/theme';
import { NzMessageService, NzModalRef } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';
import { isArray } from 'util';
import { BaseDataListDto } from '@shared/business/app.model';

@Component({
  selector: 'app-base-data-edit',
  templateUrl: './base-data-edit.component.html',
  styles: [],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BaseDataEditComponent extends ComponentBase implements OnInit {

  constructor(
    private http: _HttpClient,
    private msr: NzMessageService,
    private bsr: BusinessService,
    private modal: NzModalRef,
    private cdr: ChangeDetectorRef,
    injector: Injector
  ) {
    super(injector);
  }

  private _authConfig: AuthConfig;
  public selectValue: any;
  public newName: any;
  public newValue: any;
  public isAdding: boolean = false;
  public value: any;
  @Input()
  public record: any;

  protected AuthConfig(): AuthConfig {
    return this._authConfig;
  }

  ngOnInit() {
    if (this.record) {
      if (this.record.tpl === 'select') {
        this._authConfig = new AuthConfig('Root.BaseData.BaseData.BaseData', [
          this.record.read, this.record.update, this.record.add, this.record.delete
        ]);
      } else {
        this._authConfig = new AuthConfig('Root.BaseData.BaseData.BaseData', [
          this.record.read, this.record.update
        ]);
      }

      this.checkAuth().then(auth => {
        if (auth[this.record.read]) {
          if (this.record.tpl === 'select') {
            this.bsr.getBaseDataValuesByType(this.record.type).subscribe(res => {
              this.value = res;
              this.cdr.detectChanges();
            });
          }
        } else {
          this.bsr.getBaseDataValueByType(this.record.type).subscribe(res => {
            this.value = res;
            this.cdr.detectChanges();
          });
        }
      });
    }
  }

  change(event) {
    this.newValue = event;
  }

  add() {
    if (!this.isAdding) {
      this.isAdding = true;
    } else {
      if (!this.newName || !this.newValue) {
        this.isAdding = false;
        return;
      }
      const dto = {
        name: this.newName,
        code: this.newValue
      };

      this.http.post('api/basedata/basedata/addlistvalue', { type: this.record.type, baseData: dto })
        .subscribe((res: any) => {
          if (res.type === AjaxResultType.success) {
            this.value.push(dto);
            this.bsr.setCacheBaseDataValuesByType(this.record.type, this.value);
            this.isAdding = false;
            this.msr.success('添加成功');
          } else {
            this.msr.error(res.content);
          }
        });
    }
  }

  update() {
    this.value.forEach((el: any, index) => {
      if (el.code === this.selectValue) {
        const dto = { name: el.name, code: this.newValue };
        this.http.post('api/basedata/basedata/updatelistvalue', { type: this.record.type, baseData: dto })
          .subscribe((res: any) => {
            if (res.type === AjaxResultType.success) {
              this.msr.success('更新成功');
              this.value[index].code = this.newValue;
              this.bsr.setCacheBaseDataValuesByType(this.record.type, this.value);
            } else {
              this.msr.error(res.content);
            }
          });

        return false;
      }
    });
  }

  remove() {
    this.value.forEach((el: any, index) => {
      if (el.code === this.selectValue) {
        this.http.delete('api/basedata/basedata/deletelistvalue', { type: this.record.type, name: el.name })
          .subscribe((res: any) => {
            if (res.type === AjaxResultType.success) {
              this.msr.success('删除成功');
              this.value.splice(index);
              this.bsr.setCacheBaseDataValuesByType(this.record.type, this.value);
            } else {
              this.msr.error(res.content);
            }
          });

        return false;
      }
    });
  }

  ok() {
    this.modal.close(`new time: ${+new Date()}`);
    this.cancel();
  }

  cancel() {
    this.modal.destroy();
  }

}
