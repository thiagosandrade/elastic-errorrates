import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';

const routes: Routes = [
  {
    path: '',
    component: AppComponent
  },
  {
    path: 'logs',
    loadChildren: "../app/logs/index/logs.module#LogsModule"
  },
  {
    path: 'logs/detailed',
    loadChildren: "../app/logs/detailed/logsdetailed.module#LogsDetailedModule"
  },
  {
    path: 'logs/dashboard',
    loadChildren: "../app/logs/dashboard/dashboard.module#DashboardModule"
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule
  ],
  providers: [
  ]
})
export class AppRoutingModule { }
