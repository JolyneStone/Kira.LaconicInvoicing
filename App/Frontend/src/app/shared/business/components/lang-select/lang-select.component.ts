import { I18NService } from '@core/i18n/i18n.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-lang-select',
  templateUrl: './lang-select.component.html',
  styles: []
})
export class LangSelectComponent {
  constructor(
    private i18Service: I18NService
  ) { }

  // 设置语言
  selectLanguage(lang: string) {
    this.i18Service.use(lang);
    // 更新当前记录的语言
    localStorage.setItem('currentLanguage', lang);
    location.reload();
  }
}
