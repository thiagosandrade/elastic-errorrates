import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InfiniteScrollModule} from 'ngx-infinite-scroll';

import { LogsDetailedRoutingModule } from './log-detailed-routing.module';
import { DetailedComponent } from './detailed.component';
import { SharedModule } from '../../_shared/shared.module';
import { ApiLogService } from '../../_shared/api/log/api-log.service';

import { ApiDashboardService } from '../../_shared/api/dashboard/api-dashboard.service';
import { DatePickerService } from '../../_shared/datepicker-material/datePicker.service';

@NgModule({
  imports: [
    InfiniteScrollModule,
    SharedModule,
    CommonModule, 
    LogsDetailedRoutingModule
  ],
  declarations: [
    DetailedComponent
],
 exports: [
   
 ],
  providers: [
    ApiLogService,
    ApiDashboardService,
    DatePickerService
  ]
})
export class LogsDetailedModule { }
