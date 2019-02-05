import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DetailedComponent } from './detailed.component';

const routes: Routes = [
  { path:  '',  component: DetailedComponent }
 ];

@NgModule({
  exports: [
    RouterModule
  ],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class LogsDetailedRoutingModule { }
