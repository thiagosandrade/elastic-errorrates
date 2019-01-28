import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IndexComponent } from './index/index.component';
import { DetailedComponent } from './detailed/detailed.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  { path: 'logs',  component: IndexComponent },
  { path: 'logs/detailed',  component: DetailedComponent },
  { path: 'logs/dashboard',  component: DashboardComponent }
 ];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class LogsRoutingModule { }
