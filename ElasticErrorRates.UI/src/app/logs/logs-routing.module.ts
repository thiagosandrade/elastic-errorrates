import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IndexComponent } from './index/index.component';
import { DetailedComponent } from './detailed/detailed.component';

const routes: Routes = [
  { path: 'logs',  component: IndexComponent },
  { path: 'logdetailed',  component: DetailedComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LogsRoutingModule { }
