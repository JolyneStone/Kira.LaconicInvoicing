import { Component, Output, EventEmitter, Input, OnChanges } from '@angular/core';
import { FilterRule, EntityProperty, FilterOperate, FilterOperateEntry } from '@shared/osharp/osharp.model';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { List } from 'linqts';

@Component({
  selector: 'admin-security-filter-rule',
  templateUrl: './filter-rule.component.html',
  styles: [`
    .rule-box{margin:3px 3px;}
    .rule-box nz-select,
      .rule-box nz-input-number,
      .rule-box input[nz-input],
      .rule-box nz-date-picker {width:150px;float:left; margin-right:8px;}
    .f-left{float:left;}
    .k-input{padding:0;line-height:normal;}
    `]
})
export class FilterRuleComponent implements OnChanges {

  @Input() rule: FilterRule;
  @Input() properties: EntityProperty[];
  @Output() remove: EventEmitter<FilterRule> = new EventEmitter<FilterRule>();

  operateEntries: FilterOperateEntry[] = [];
  property: EntityProperty;

  constructor(
    private osharp: OsharpService
  ) { }

  ngOnChanges(): void {
    if (this.rule) {
      this.fieldChange(this.rule.field, true);
    } else {
      this.operateEntries = [];
    }
  }
  removeRule() {
    this.remove.emit(this.rule);
  }

  fieldChange(field: string, first: boolean = false) {
    if (this.properties.length == 0 || !field) {
      return;
    }
    this.property = new List(this.properties).FirstOrDefault(m => m.name === field);
    if (this.property == null) {
      return;
    }
    if (!first) {
      this.rule.value = null;
    }
    switch (this.property.typeName) {
      case 'System.Boolean':
        this.operateEntries = this.getOperateEntries([FilterOperate.equal, FilterOperate.notEqual]);
        if (!this.rule.value) {
          this.rule.value = 'false';
        }
        break;
      case 'System.Guid':
        this.operateEntries = this.getOperateEntries([FilterOperate.equal, FilterOperate.notEqual]);
        if (!this.rule.value) {
          this.rule.value = '';
        }
        break;
      case 'System.Int32':
        if (this.property.valueRange.length == 0) {
          //数值类型
          this.operateEntries = this.getOperateEntries([FilterOperate.equal, FilterOperate.notEqual, FilterOperate.less, FilterOperate.lessOrEqual, FilterOperate.greater, FilterOperate.greaterOrEqual]);
          if (!this.rule.value) {
            this.rule.value = '0';
          }
        } else {
          //枚举类型
          this.operateEntries = this.getOperateEntries([FilterOperate.equal, FilterOperate.notEqual]);
          if (!this.rule.value) {
            this.rule.value = this.property.valueRange[0].id;
          }
        }
        break;
      case 'System.DateTime':
        this.operateEntries = this.getOperateEntries([FilterOperate.equal, FilterOperate.notEqual, FilterOperate.less, FilterOperate.lessOrEqual, FilterOperate.greater, FilterOperate.greaterOrEqual]);
        if (!this.rule.value) {
          // this.rule.Value = new Date().toLocaleString();
          console.log(this.rule.value);
        }
        break;
      case 'System.String':
        this.operateEntries = this.getOperateEntries([FilterOperate.equal, FilterOperate.notEqual, FilterOperate.startsWith, FilterOperate.endsWith, FilterOperate.contains, FilterOperate.notContains]);
        if (!this.rule.value) {
          this.rule.value = '';
        }
        break;
      default:
        this.operateEntries = this.getOperateEntries([FilterOperate.equal, FilterOperate.notEqual, FilterOperate.less, FilterOperate.lessOrEqual, FilterOperate.greater, FilterOperate.greaterOrEqual, FilterOperate.startsWith, FilterOperate.endsWith, FilterOperate.contains, FilterOperate.notContains]);
        if (!this.rule.value) {
          this.rule.value = '';
        }
        break;
    }
    if (!this.rule.operate || !new List(this.operateEntries).Any(m => m.operate == this.rule.operate)) {
      this.rule.operate = this.operateEntries[0].operate;
    }
  }

  private getOperateEntries(operates: FilterOperate[]): FilterOperateEntry[] {
    let entries: FilterOperateEntry[] = [];
    for (let index = 0; index < operates.length; index++) {
      const operate = operates[index];
      entries.push(new FilterOperateEntry(operate));
    }
    return entries;
  }

  onTagsChangeEvent(e) {
    if (!e || e.length == 0) {
      this.osharp.error(`${this.property.display}不能为空`);
      this.rule.value = null;
      return;
    }
    this.rule.value = e[0];
  }
}
