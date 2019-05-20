import { Component, HostListener, Input } from '@angular/core';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'app-logo',
  template: `
  <div class="g-logo-container-{{size}}">
    <div class="g-logo-pic"></div>
    <div class="g-logo-text"></div>
  </div>
  `,
  styles: [
    `
      .g-logo-pic {
        position: relative;
        width: 100%;
        height: 100%;
        background: url('/assets/logo.jpg');
        background-repeat: no-repeat;
        background-size: cover;
      }

      .g-logo-text {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        width: 100%;
        height: 100%;
        color: #000;
        mix-blend-mode: lighten;
        z-index: 99;
        background-color: #fff;
        background-repeat: no-repeat;
        background-image: url('/assets/logo-text.png');
        background-size: cover;
      }

      .g-logo-container-small {
        position: relative;
        width: 70px;
        height: 40px;
        margin: auto;
      }

      .g-logo-container-middle {
        position: relative;
        width: 100px;
        height: 60px;
        margin: auto;
      }

      .g-logo-container-large {
        position: relative;
        width: 500px;
        height: 300px;
        margin: auto;
      }
    `
  ]
})
export class LogoComponent {
  @Input() public size: string;

  constructor(
  ) { }
}
