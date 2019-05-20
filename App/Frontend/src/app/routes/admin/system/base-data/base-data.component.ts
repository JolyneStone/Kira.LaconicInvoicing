import { Component, OnInit, Injector } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { Router } from '@angular/router';
import { _HttpClient } from '@delon/theme';
import { Inject } from '@angular/compiler/src/core';
import { NzMessageService } from 'ng-zorro-antd';
import { BusinessService } from '@shared/business/services/business.service';
import { AuthConfig } from '@shared/osharp/osharp.model';
import { STColumn } from '@delon/abc';
import { BaseDataEditComponent } from '../base-data-edit/base-data-edit.component';

@Component({
  selector: 'app-base-data',
  templateUrl: './base-data.component.html',
  styles: []
})
export class BaseDataComponent implements OnInit {
  dict: any[] = [
    {
      order: '1',
      type: 'NUMBERTYPE',
      name: '编号类型',
      desc: '用于控制生成编号时的前缀',
      tpl: 'select',
      read: 'GetValues',
      add: 'AddListValue',
      update: 'UpdateListValue',
      delete: 'DeleteListValue'
    },
    {
      order: '2',
      type: 'UNITTYPE',
      name: '单位',
      desc: '用于单位分类',
      tpl: 'select',
      read: 'GetValues',
      add: 'AddListValue',
      update: 'UpdateListValue',
      delete: 'DeleteListValue'
    },
    {
      order: '3',
      type: 'VENDORTYPE',
      name: '供应商类型',
      desc: '用于进行供应商分类',
      tpl: 'select',
      read: 'GetValues',
      add: 'AddListValue',
      update: 'UpdateListValue',
      delete: 'DeleteListValue'
    },
    {
      order: '4',
      type: 'CUSTOMERTYPE',
      name: '客户类型',
      desc: '用于进行客户分类',
      tpl: 'select',
      read: 'GetValues',
      add: 'AddListValue',
      update: 'UpdateListValue',
      delete: 'DeleteListValue'
    },
    {
      order: '5',
      type: 'MATERIALTYPE',
      name: '原料类型',
      desc: '用于进行原料分类',
      tpl: 'select',
      read: 'GetValues',
      add: 'AddListValue',
      update: 'UpdateListValue',
      delete: 'DeleteListValue'
    },
    {
      order: '6',
      type: 'PRODUCTTYPE',
      name: '产品类型',
      desc: '用于进行产品分类',
      tpl: 'select',
      read: 'GetValues',
      add: 'AddListValue',
      update: 'UpdateListValue',
      delete: 'DeleteListValue'
    },
    {
      order: '7',
      type: 'PAYWAY',
      name: '支付方式',
      desc: '用户对支付方式进行分类',
      tpl: 'select',
      read: 'GetValues',
      add: 'AddListValue',
      update: 'UpdateListValue',
      delete: 'DeleteListValue'
    },
  ];

  columns: STColumn[] = [
    { title: '序号', index: 'order', width: '100px' },
    { title: '类型', index: 'name', width: '200px' },
    { title: '描述', index: 'desc', width: '400px' },
    {
      title: '操作', buttons: [
        {
          text: '详情',
          icon: 'edit',
          type: 'modal',
          modal: {
            component: BaseDataEditComponent
          },
          click: (record: any, modal: any) => {
          },
        }
      ]
    }
  ];
  constructor(
    private router: Router,
  ) {
  }

  ngOnInit() {
  }



}
