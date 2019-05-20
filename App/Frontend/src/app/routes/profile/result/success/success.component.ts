import { Component } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'app-result-success',
  templateUrl: './success.component.html',
})
export class ProfileResultSuccessComponent {
  constructor(public msg: NzMessageService) {}
}
