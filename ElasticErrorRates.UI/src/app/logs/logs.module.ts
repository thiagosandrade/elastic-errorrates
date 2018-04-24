import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LogsRoutingModule } from './logs-routing.module';
import { IndexComponent } from './index/index.component';
import { SharedModule } from '../_shared/shared.module';
import { ApiService } from '../_shared/api/api.service';
import { DetailedComponent } from './detailed/detailed.component';

@NgModule({
  imports: [
    SharedModule,
    CommonModule,
    LogsRoutingModule
  ],
  declarations: [
    IndexComponent,
    DetailedComponent
],
  providers: [
    ApiService
  ]
})
export class LogsModule { }
