import { StatisticsService } from './services/statistics.service';
import { DocumentManagementService } from '@shared/business/services/document.management.service';
import { DocumentService } from './services/document.service';
import { PrintService } from './services/print.service';
import { PrintManagementService } from './services/print.management.service';
import { NoticeService } from './services/notice.service';
import { SaleService } from './services/sale.service';
import { NgModule } from '@angular/core';
import { HeaderStorageComponent } from './components/storage.component';
import { HeaderNotifyComponent } from './components/notify.component';
import { HeaderIconComponent } from './components/icon.component';
import { HeaderFullScreenComponent } from './components/fullscreen.component';
import { LogoComponent } from './components/logo.component';
import { BusinessService } from './services/business.service';
import { LoadAdminMenuGuard } from './gurads/load.admin-menu.guard/load-admin-menu-guard';
import { LoadProfileMenuGuard } from './gurads/load.profile-menu.guard/load-profile-menu-guard';
import { LoadDefaultMenuGuard } from './gurads/load.default-menu.guard/load-default-menu-guard';
import { CommonModule } from '@angular/common';
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { DelonABCModule } from '@delon/abc';
import { PurchaseService } from './services/purchase.service';
import { DataListComponent } from './components/data-list/data-list.component';
import { TranslateModule } from '@ngx-translate/core';
import { DataListMutilServiceComponent } from './components/data-list-mutil-service/data-list-mutil-service.component';
import { DataListSingleServiceComponent } from './components/data-list-single-service/data-list-single-service.component';
import { WarehouseService } from './services/warehouse.service';
import { LangSelectComponent } from './components/lang-select/lang-select.component';

const DIRECTIVES = [
];

const COMPONENTS = [
  HeaderStorageComponent,
  HeaderNotifyComponent,
  HeaderIconComponent,
  HeaderFullScreenComponent,
  LogoComponent,
  DataListComponent,
  DataListMutilServiceComponent,
  DataListSingleServiceComponent,
  LangSelectComponent,
];

const ENTERCOMPONENTS = [
  DataListMutilServiceComponent,
  DataListSingleServiceComponent,
];

const SERVICES = [
  BusinessService,
  PurchaseService,
  WarehouseService,
  SaleService,
  NoticeService,
  PrintManagementService,
  PrintService,
  DocumentManagementService,
  DocumentService,
  StatisticsService
];

const GUARDS = [
  LoadAdminMenuGuard,
  LoadProfileMenuGuard,
  LoadDefaultMenuGuard
];

@NgModule({
  declarations: [
    ...DIRECTIVES,
    ...COMPONENTS,
  ],
  imports: [
    CommonModule,
    NgZorroAntdModule,
    DelonABCModule,
    // i18n
    TranslateModule,
  ],
  exports: [
    ...DIRECTIVES,
    ...COMPONENTS
  ],
  providers: [
    ...SERVICES
  ],
  entryComponents: [
    ...ENTERCOMPONENTS
  ]
})
export class BusinessModule { }