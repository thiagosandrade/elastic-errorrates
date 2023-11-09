import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { SharedModule } from '../../_shared/shared.module';

import { ApiLogService } from '../../_shared/api/log/api-log.service';
import { ApiDashboardService } from '../../_shared/api/dashboard/api-dashboard.service';
import { DatePickerService } from '../../_shared/datepicker-material/datePicker.service';

import { UkDailyRateComponent } from './components/uk-dailyrate/uk-dailyrate.component';
import { UkYesterdayRateComponent } from './components/uk-yesterdayrate/uk-yesterdayrate.component';
import { ROIDailyRateComponent } from './components/roi-dailyrate/roi-dailyrate.component';
import { ROIYesterdayRateComponent } from './components/roi-yesterdayrate/roi-yesterdayrate.component';
import { UkWeekRateGraphComponent } from './components/uk-weekrate-graph/uk-weekrate-graph.component';
import { ROIWeekrateGraphComponent } from './components/roi-weekrate-graph/roi-weekrate-graph.component';
import { UkMonthRateGraphComponent } from './components/uk-monthrate-graph/uk-monthrate-graph.component';
import { ROIMonthRateGraphComponent } from './components/roi-monthrate-graph/roi-monthrate-graph.component';
import { UkErrorsRankComponent } from './components/uk-errors-rank/uk-errors-rank.component';
import { ROIErrorsRankComponent } from './components/roi-errors-rank/roi-errors-rank.component';


@NgModule({
  imports: [
    SharedModule,
    CommonModule, 
    DashboardRoutingModule
  ],
  declarations: [
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
    
 ],
  providers: [
    ApiLogService,
    ApiDashboardService,
    DatePickerService
  ]
})
export class DashboardModule { }
