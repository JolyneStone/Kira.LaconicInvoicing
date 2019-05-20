import { NoticeDto } from '@shared/business/app.model';
import { NoticeService } from './../services/notice.service';
import { Component, OnInit } from '@angular/core';
import * as distanceInWordsToNow from 'date-fns/distance_in_words_to_now';
import { NzMessageService } from 'ng-zorro-antd';
import { NoticeItem, NoticeIconList, numberToChinese } from '@delon/abc';
import { AjaxResultType } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import * as signalR from '@aspnet/signalr';
import * as signlaRProtocols from '@aspnet/signalr-protocol-msgpack';

/**
 * 菜单通知
 */
@Component({
  selector: 'header-notify',
  template: `
  <notice-icon
    [data]="data"
    [count]="count"
    [loading]="loading"
    (select)="select($event)"
    (clear)="clear($event)"
    (popoverVisibleChange)="loadData()"></notice-icon>
  `
})
export class HeaderNotifyComponent implements OnInit {

  constructor(
    private msg: NzMessageService,
    private noticeService: NoticeService,
    private router: Router
  ) { }

  data: NoticeItem[] = [
    {
      title: '通知',
      list: [],
      emptyText: '你已查看所有通知',
      emptyImage: 'assets/notice.svg',
      clearText: '清空通知',
    },
    // {
    //   title: '消息',
    //   list: [],
    //   emptyText: '您已读完所有消息',
    //   emptyImage:
    //     'https://gw.alipayobjects.com/zos/rmsportal/sAuJeJzSKbUmHfBQRzmZ.svg',
    //   clearText: '清空消息',
    // },
    // {
    //   title: '待办',
    //   list: [],
    //   emptyText: '你已完成所有待办',
    //   emptyImage:
    //     'https://gw.alipayobjects.com/zos/rmsportal/HsIsxMZiWKrNUavQUXqx.svg',
    //   clearText: '清空待办',
    // },
  ];
  count = 5;
  loading = false;
  connection: signalR.HubConnection;

  ngOnInit() {
    this.noticeService.getAllUnReadCount()
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.count = res.data;
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
      });

    // try {
    //   this.connection = new signalR.HubConnectionBuilder()
    //     .withUrl('http://localhost:7001/hubs/notice')
    //     //.withHubProtocol(new signlaRProtocols.MessagePackHubProtocol())
    //     .build();

    //   this.connection.on('Receiving', data => {
    //     console.log(data);
    //     this.count += 1;
    //   });

    //   this.connection.start();
    // } catch (err) {
    //   console.log(err);
    // }
  }


  updateNoticeData(notices: NoticeIconList[]): NoticeItem[] {
    const data = this.data.slice();
    if (data.length === 0)
      return data;
    data.forEach(i => (i.list = []));

    notices.forEach(item => {
      const newItem = { ...item };
      // if (newItem.datetime)
      //   newItem.datetime = distanceInWordsToNow(item.datetime, {
      //     locale: (window as any).__locale__,
      //   });
      if (newItem.extra && newItem.status) {
        newItem.color = {
          todo: undefined,
          processing: 'blue',
          urgent: 'red',
          doing: 'gold',
        }[newItem.status];
      }

      const d = data.find(w => w.title === newItem.type);
      if (d) {
        d.list.push(newItem);
      }
    });
    return data;
  }

  loadData() {
    if (this.loading) return;
    this.loading = true;
    this.noticeService.getAllUnRead()
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.count = res.data.length;
          this.updateNoticeData((res.data as NoticeDto[]).map(n => {
            return {
              id: n.id,
              avatar: 'assets/envelope.png',
              title: n.content.length < 10 ? n.content : (n.content.substr(0, 10) + '...'),
              //datetime: n.dateTime.toLocaleDateString(),
              type: '通知'
            }
          }));
        } else if (res && res.content) {
          this.msg.error(res.content);
        }

        this.loading = false;
      });
    // setTimeout(() => {
    //   this.data = this.updateNoticeData([
    //     {
    //       id: '000000001',
    //       avatar:
    //         'assets/envelope.png',
    //       title: '你收到了 14 份新周报',
    //       datetime: '2017-08-09',
    //       type: '通知',
    //     },
    //     {
    //       id: '000000002',
    //       avatar:
    //         'https://gw.alipayobjects.com/zos/rmsportal/OKJXDXrmkNshAMvwtvhu.png',
    //       title: '你推荐的 曲妮妮 已通过第三轮面试',
    //       datetime: '2017-08-08',
    //       type: '通知',
    //     },
    //     {
    //       id: '000000003',
    //       avatar:
    //         'https://gw.alipayobjects.com/zos/rmsportal/kISTdvpyTAhtGxpovNWd.png',
    //       title: '这种模板可以区分多种通知类型',
    //       datetime: '2017-08-07',
    //       read: true,
    //       type: '通知',
    //     },
    //     {
    //       id: '000000004',
    //       avatar:
    //         'https://gw.alipayobjects.com/zos/rmsportal/GvqBnKhFgObvnSGkDsje.png',
    //       title: '左侧图标用于区分不同的类型',
    //       datetime: '2017-08-07',
    //       type: '通知',
    //     },
    //     {
    //       id: '000000005',
    //       avatar:
    //         'https://gw.alipayobjects.com/zos/rmsportal/ThXAXghbEsBCCSDihZxY.png',
    //       title: '内容不要超过两行字，超出时自动截断',
    //       datetime: '2017-08-07',
    //       type: '通知',
    //     },
    //     {
    //       id: '000000006',
    //       avatar:
    //         'https://gw.alipayobjects.com/zos/rmsportal/fcHMVNCjPOsbUGdEduuv.jpeg',
    //       title: '曲丽丽 评论了你',
    //       description: '描述信息描述信息描述信息',
    //       datetime: '2017-08-07',
    //       type: '消息',
    //     }
    //   ]);

    //   this.loading = false;
    // }, 0);
  }

  clear(type: string) {
    this.noticeService.clearAllByUser()
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.updateNoticeData([]);
        } else if (res && res.content) {
          this.msg.error(res.content);
        }
      });
  }

  select(res: any) {
    this.router.navigateByUrl('/profile/account/center');
  }
}
