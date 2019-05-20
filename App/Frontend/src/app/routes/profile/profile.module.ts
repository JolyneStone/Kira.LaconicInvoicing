import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProfileRoutingModule } from './profile-routing.module';
import { BasicFormComponent } from './form/basic-form/basic-form.component';
import { StepFormComponent } from './form/step-form/step-form.component';
import { AdvancedFormComponent } from './form/advanced-form/advanced-form.component';
import { ProfileTableListComponent } from './list/table-list/table-list.component';
import { ProfileBasicListComponent } from './list/basic-list/basic-list.component';
import { ProfileCardListComponent } from './list/card-list/card-list.component';
import { ProfileListLayoutComponent } from './list/list/list.component';
import { ProfileListArticlesComponent } from './list/articles/articles.component';
import { ProfileListProjectsComponent } from './list/projects/projects.component';
import { ProfileListApplicationsComponent } from './list/applications/applications.component';
import { ProfileProfileBaseComponent } from './profile/basic/basic.component';
import { ProfileProfileAdvancedComponent } from './profile/advanced/advanced.component';
import { ProfileResultSuccessComponent } from './result/success/success.component';
import { ProfileResultFailComponent } from './result/fail/fail.component';
import { ProfileAccountCenterComponent } from './account/center/center.component';
import { ProfileAccountSettingsComponent } from './account/settings/settings.component';
import { ProfileAccountSettingsBaseComponent } from './account/settings/base/base.component';
import { ProfileAccountSettingsSecurityComponent } from './account/settings/security/security.component';
import { ProfileAccountSettingsNotificationComponent } from './account/settings/notification/notification.component';
import { Step1Component } from './form/step-form/step1.component';
import { Step2Component } from './form/step-form/step2.component';
import { Step3Component } from './form/step-form/step3.component';
import { ProBasicListEditComponent } from './list/basic-list/edit/edit.component';
import { SharedModule } from '@shared/shared.module';
import { ProfileAccountCenterNoticesComponent } from './account/center/noticce/notice.component';

const COMPONENTS = [
  BasicFormComponent,
  StepFormComponent,
  AdvancedFormComponent,
  ProfileTableListComponent,
  ProfileBasicListComponent,
  ProfileCardListComponent,
  ProfileListLayoutComponent,
  ProfileListArticlesComponent,
  ProfileListProjectsComponent,
  ProfileListApplicationsComponent,
  ProfileProfileBaseComponent,
  ProfileProfileAdvancedComponent,
  ProfileResultSuccessComponent,
  ProfileResultFailComponent,
  ProfileAccountCenterComponent,
  ProfileAccountCenterNoticesComponent,
  ProfileAccountSettingsComponent,
  ProfileAccountSettingsBaseComponent,
  ProfileAccountSettingsSecurityComponent,
  ProfileAccountSettingsNotificationComponent,
];

const COMPONENTS_NOROUNT = [
  Step1Component,
  Step2Component,
  Step3Component,
  ProBasicListEditComponent
];

@NgModule({
  imports: [SharedModule, ProfileRoutingModule],
  declarations: [ ...COMPONENTS, ...COMPONENTS_NOROUNT ],
  entryComponents: COMPONENTS_NOROUNT,
})
export class ProfileModule { }
