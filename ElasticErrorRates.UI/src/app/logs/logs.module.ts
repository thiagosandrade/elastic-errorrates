import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InfiniteScrollModule} from 'ngx-infinite-scroll';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { LogsRoutingModule } from './logs-routing.module';
import { IndexComponent } from './index/index.component';
import { SharedModule } from '../_shared/shared.module';
import { ApiLogService } from '../_shared/api/log/api-log.service';
import { DetailedComponent } from './detailed/detailed.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ApiDashboardService } from '../_shared/api/dashboard/api-dashboard.service';

import { UkDailyRateComponent } from './dashboard/components/uk-dailyrate/uk-dailyrate.component';
import { UkYesterdayRateComponent } from './dashboard/components/uk-yesterdayrate/uk-yesterdayrate.component';
import { ROIDailyRateComponent } from './dashboard/components/roi-dailyrate/roi-dailyrate.component';
import { ROIYesterdayRateComponent } from './dashboard/components/roi-yesterdayrate/roi-yesterdayrate.component';

import { UkWeekRateGraphComponent } from './dashboard/components/uk-weekrate-graph/uk-weekrate-graph.component';
import { ROIWeekrateGraphComponent } from './dashboard/components/roi-weekrate-graph/roi-weekrate-graph.component';

import { UkMonthRateGraphComponent } from './dashboard/components/uk-monthrate-graph/uk-monthrate-graph.component';
import { ROIMonthRateGraphComponent } from './dashboard/components/roi-monthrate-graph/roi-monthrate-graph.component';

import { UkErrorsRankComponent } from './dashboard/components/uk-errors-rank/uk-errors-rank.component';
import { ROIErrorsRankComponent } from './dashboard/components/roi-errors-rank/roi-errors-rank.component';
import { DatePickerService } from '../_shared/datepicker-material/datePicker.service';

@NgModule({
  imports: [
    BrowserAnimationsModule,
    SharedModule,
    CommonModule, 
    LogsRoutingModule,
    InfiniteScrollModule
  ],
  declarations: [
    IndexComponent,
    DetailedComponent,
    DashboardComponent,
    UkDailyRateComponent,
    UkYesterdayRateComponent,
    ROIDailyRateComponent,
    ROIYesterdayRateComponent,
    UkWeekRateGraphComponent,
    ROIWeekrateGraphComponent,
    UkMonthRateGraphComponent,
    ROIMonthRateGraphComponent,
    UkErrorsRankComponent,
    ROIErrorsRankComponent
],
 exports: [
    UkDailyRateComponent,
    UkYesterdayRateComponent,
    ROIDailyRateComponent,
    ROIYesterdayRateComponent,
    UkWeekRateGraphComponent,
    ROIWeekrateGraphComponent,
    UkMonthRateGraphComponent,
    ROIMonthRateGraphComponent,
    UkErrorsRankComponent,
    ROIErrorsRankComponent
 ],
  providers: [
    ApiLogService,
    ApiDashboardService,
    DatePickerService
  ]
})
export class LogsModule { }
