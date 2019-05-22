import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiDashboardService } from './api/dashboard/api-dashboard.service';
import { ApiLogService } from './api/log/api-log.service';
import { ModalComponent } from './modal/modal.component';
import { ModalService } from './modal/modal.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { DivFlexibleComponent } from './divFlexible/divflexible.component';
import { LoadingSpinnerComponent } from './loadingSpinner/loadingSpinner.component';


import { KeepHtmlPipe } from './keepHtmlPipe/keep-html.pipe';
import { ChartGeneratorComponent } from './chartGenerator/chartgenerator.component';
import { MatNativeDateModule, MatDatepickerModule, MatFormFieldModule, MatButtonModule, MatInputModule, MatRippleModule } from '@angular/material';
import { DatePickerService } from './datepicker-material/datePicker.service';
import { DatepickerMaterialComponent } from './datepicker-material/datepicker-material.component';
import { DatePipe } from '@angular/common';
import { SignalRService } from './signalR/signalR.service';
import { ChartFooterComponent } from './chartFooter/chartFooter.component';
import { DailyRateFooterComponent } from './dailyRateFooter/dailyRateFooter.component';
import { ApiUserService } from './api/user/api-user.service';
import { MessageNotifierComponent } from './messageNotifier/messageNotifier.component';
import { GrowlModule } from 'primeng/growl';
import { MessageNotifierService } from './messageNotifier/messageNotifier.service';

@NgModule({
   imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MatNativeDateModule, MatDatepickerModule, MatFormFieldModule, MatButtonModule, MatInputModule, MatRippleModule,
        GrowlModule,
   ],
   declarations: [
        ModalComponent,

        DivFlexibleComponent,
        LoadingSpinnerComponent,
        MessageNotifierComponent,

        KeepHtmlPipe,
        ChartGeneratorComponent,
        DatepickerMaterialComponent,
        ChartFooterComponent,
        DailyRateFooterComponent
   ],
   exports: [
        ModalComponent,

        DivFlexibleComponent,
        LoadingSpinnerComponent,
        MessageNotifierComponent,

        KeepHtmlPipe,
        ChartGeneratorComponent,
        MatNativeDateModule, MatDatepickerModule, MatFormFieldModule, MatButtonModule, MatInputModule, MatRippleModule,
        DatepickerMaterialComponent,
        ChartFooterComponent,
        DailyRateFooterComponent
   ],
   providers: [
        ApiLogService,
        ApiDashboardService,
        ModalService,
        DatePickerService,
        DatePipe,
        SignalRService,
        ApiUserService,
        MessageNotifierService
    ]
})
export class SharedModule { 
  
}
