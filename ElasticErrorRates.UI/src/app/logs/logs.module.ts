import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LogsRoutingModule } from './logs-routing.module';
import { IndexComponent } from './index/index.component';
import { SharedModule } from '../_shared/shared.module';
import { ApiService } from '../_shared/api/api.service';

@NgModule({
  imports: [
    SharedModule,
    CommonModule,
    LogsRoutingModule
  ],
  declarations: [
    IndexComponent
  ],
  providers: [
    ApiService
  ]
})
export class LogsModule { }
