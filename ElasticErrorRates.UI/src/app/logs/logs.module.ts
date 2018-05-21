import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InfiniteScrollModule} from 'ngx-infinite-scroll';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import {
  MatButtonModule,
  MatInputModule,
  MatRippleModule,
  MatTooltipModule,
} from '@angular/material';

import { LogsRoutingModule } from './logs-routing.module';
import { IndexComponent } from './index/index.component';
import { SharedModule } from '../_shared/shared.module';
import { ApiService } from '../_shared/api/api.service';
import { DetailedComponent } from './detailed/detailed.component';
import { DashboardComponent } from './dashboard/dashboard.component';

@NgModule({
  imports: [
    BrowserAnimationsModule,
    SharedModule,
    CommonModule, 
    MatButtonModule,
    MatRippleModule,
    MatInputModule,
    MatTooltipModule,
    LogsRoutingModule,
    InfiniteScrollModule
  ],
  declarations: [
    IndexComponent,
    DetailedComponent,
    DashboardComponent
],
  providers: [
    ApiService
  ]
})
export class LogsModule { }
