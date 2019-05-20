import { Routes, RouterModule } from '@angular/router';
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
import { NgModule } from '@angular/core';

const routes: Routes = [
  {
    path: 'form',
    children: [
      { path: 'basic-form', component: BasicFormComponent },
      { path: 'step-form', component: StepFormComponent },
      { path: 'advanced-form', component: AdvancedFormComponent },
    ],
  },
  {
    path: 'list',
    children: [
      { path: 'table-list', component: ProfileTableListComponent },
      { path: 'basic-list', component: ProfileBasicListComponent },
      { path: 'card-list', component: ProfileCardListComponent },
      {
        path: '',
        component: ProfileListLayoutComponent,
        // children: [
        //   { path: 'articles', component: ProfileListArticlesComponent },
        //   { path: 'projects', component: ProfileListProjectsComponent },
        //   { path: 'applications', component: ProfileListApplicationsComponent },
        // ],
      },
    ],
  },
  {
    path: 'profile',
    children: [
      { path: 'basic', component: ProfileProfileBaseComponent },
      { path: 'advanced', component: ProfileProfileAdvancedComponent },
    ],
  },
  {
    path: 'result',
    children: [
      { path: 'success', component: ProfileResultSuccessComponent },
      { path: 'fail', component: ProfileResultFailComponent },
    ],
  },
  {
    path: 'account',
    children: [
      {
        path: 'center',
        component: ProfileAccountCenterComponent,
        // children: [
        //   { path: '', redirectTo: 'articles', pathMatch: 'full' },
        // ],
      },
      {
        path: 'settings',
        component: ProfileAccountSettingsComponent,
        children: [
          { path: '', redirectTo: 'base', pathMatch: 'full' },
          {
            path: 'base',
            component: ProfileAccountSettingsBaseComponent,
            data: { titleI18n: 'app.account.settings' },
          },
          {
            path: 'security',
            component: ProfileAccountSettingsSecurityComponent,
            data: { titleI18n: 'app.account.settings' },
          },
          // {
          //   path: 'binding',
          //   component: ProfileAccountSettingsBindingComponent,
          //   data: { titleI18n: 'app.account.settings' },
          // },
          // {
          //   path: 'notification',
          //   component: ProfileAccountSettingsNotificationComponent,
          //   data: { titleI18n: 'app.account.settings' },
          // },
        ],
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProfileRoutingModule {}