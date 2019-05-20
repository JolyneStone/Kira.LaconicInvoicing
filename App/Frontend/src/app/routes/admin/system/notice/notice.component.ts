import { NoticeDto } from './../../../../shared/business/app.model';
import { AuthConfig, AjaxResultType } from '@shared/osharp/osharp.model';
import { Component, OnInit, ChangeDetectorRef, Injector } from '@angular/core';
import { ComponentBase } from '@shared/osharp/services/osharp.service';
import { _HttpClient } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';
import { NoticeService } from '@shared/business/services/notice.service';

@Component({
  selector: 'app-notice',
  templateUrl: './notice.component.html',
  styles: [
    `
      .loadmore-list {
        min-height: 350px;
      }
      .loadmore {
        text-align: center;
        margin-top: 12px;
        height: 32px;
        line-height: 32px;
        margin-bottom: 12px;
      }
      .unread {
        font-weight:bold;
      }
    `
  ]
})
export class NoticeComponent extends ComponentBase implements OnInit {

  constructor(
    private http: _HttpClient,
    private msr: NzMessageService,
    private noticeService: NoticeService,
    private cdr: ChangeDetectorRef,
    injector: Injector
  ) {
    super(injector);
    super.checkAuth();
  }

  noticeDto: NoticeDto;
  loadingMore = false;
  data: NoticeDto[] = [];
  count = 0;
  size = 5;

  ngOnInit() {
    this.noticeDto = new NoticeDto();
    this.onLoadMore();
  }

  protected AuthConfig() {
    return new AuthConfig('Root.Notification.Notification.Notification', [
      'GetAll',
      'Add',
      'Delete',
      'ClearAll'
    ]);
  }

  save() {
    this.noticeService.add(this.noticeDto)
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.msr.success('发布成功');
          this.noticeDto = new NoticeDto();
        } else if (res && res.content) {
          this.msr.error(res.content);
        }
      });
  }


  delete(item: NoticeDto) {
    this.noticeService.delete(item.id)
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          this.msr.success('删除成功');
          this.data = this.data.filter(d => d.id !== item.id);
        } else if (res && res.content) {
          this.msr.error(res.content);
        }
      });
  }

  onLoadMore() {
    this.loadingMore = true;
    this.noticeService.getAll(this.count, this.size)
      .subscribe(res => {
        if (res && res.type === AjaxResultType.success) {
          if (res.data && res.data.length > 0) {
            (res.data  as Array<NoticeDto>).forEach(element => {
              this.data.push(element);
            });
            this.count += res.data.length;
          } else {
            this.msr.info('无更多公告');
          }
        } else if (res && res.content) {
          this.msr.error(res.content);
        }

        this.loadingMore = false;
      });
  }
}
