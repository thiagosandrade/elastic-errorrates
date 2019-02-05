import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LogRoutingModule } from './log-routing.module';
import { IndexComponent } from './index.component';
import { SharedModule } from '../../_shared/shared.module';
import { ApiLogService } from '../../_shared/api/log/api-log.service';

import { ApiDashboardService } from '../../_shared/api/dashboard/api-dashboard.service';
import { DatePickerService } from '../../_shared/datepicker-material/datePicker.service';

@NgModule({
  imports: [
    SharedModule,
    CommonModule, 
    LogRoutingModule
  ],
  declarations: [
    IndexComponent
],
 exports: [
   
 ],
  providers: [
    ApiLogService,
    ApiDashboardService,
    DatePickerService
  ]
})
export class LogsModule { }
