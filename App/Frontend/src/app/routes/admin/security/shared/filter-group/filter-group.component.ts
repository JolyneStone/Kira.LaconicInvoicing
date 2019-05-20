import { Component, Input, OnChanges, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FilterGroup, FilterRule, FilterOperate, AjaxResult, AjaxResultType, EntityProperty, FilterOperateEntry } from '@shared/osharp/osharp.model';
import { OsharpService } from '@shared/osharp/services/osharp.service';


@Component({
  selector: 'admin-security-filter-group',
  templateUrl: './filter-group.component.html',
  styles: [`
  .group-box{margin:2px;padding:3px; border:dashed 2px #ddd;}
  .group-box nz-select{margin-right:5px;}
  .group-operate{margin:3px 0;}
  `]
})
export class FilterGroupComponent implements OnChanges {

  @Input() group: FilterGroup;
  @Input() entity: string;
  @Output() remove: EventEmitter<FilterGroup> = new EventEmitter<FilterGroup>();

  entityProperties: EntityProperty[] = [];
  groupOperateEntries: FilterOperateEntry[] = [new FilterOperateEntry(FilterOperate.and), new FilterOperateEntry(FilterOperate.or)];

  constructor(
    private osharp: OsharpService,
    private http: HttpClient
  ) {
    this.group = new FilterGroup();
    this.group.operate = FilterOperate.or;
  }

  ngOnChanges() {
    if (this.group && (!this.group.level || this.group.level == 1)) {
      FilterGroup.Init(this.group);
    }
    if (this.group) {
      if (!this.entity) {
        this.entityProperties = [];
        return;
      }
      this.http.get<AjaxResult>("api/admin/entityinfo/ReadProperties?typeName=" + this.entity).subscribe(res => {
        if (res.type != AjaxResultType.success) {
          this.osharp.error(res.content);
          return;
        }
        this.entityProperties = res.data;
      });
    }
  }
  addGroup() {
    if (!this.entity) {
      this.osharp.error("请选择左边的一行再进行操作");
      return;
    }
    let subGroup = new FilterGroup();
    subGroup.level = this.group.level + 1;
    this.group.groups.push(subGroup);
  }
  addRule() {
    if (!this.entity) {
      this.osharp.error("请选择左边的一行再进行操作");
      return;
    }
    let rule = new FilterRule(null, null);
    if (this.entityProperties.length > 0) {
      rule.field = this.entityProperties[0].name;
    }
    this.group.rules.push(rule);
  }
  removeRule(rule: FilterRule) {
    if (rule) {
      let index = this.group.rules.indexOf(rule);
      if (index >= 0) {
        this.group.rules.splice(index, 1);
      }
    }
  }
  /** 删除分组按钮 */
  removeGroup() {
    this.remove.emit(this.group);
  }

  /**删除子分组的事件响应 */
  removeSubGroup(subGroup: FilterGroup) {
    if (subGroup) {
      let index = this.group.groups.indexOf(subGroup);
      if (index >= 0) {
        this.group.groups.splice(index, 1);
      }
    }
  }
}
