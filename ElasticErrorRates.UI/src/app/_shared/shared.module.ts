import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiDashboardService } from './api/dashboard/api.service';
import { ApiLogService } from './api/log/api.service';
import { ModalComponent } from './modal/modal.component';
import { ModalService } from './modal/modal.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DivFlexibleComponent } from './divFlexible/divFlexible.component';
import { KeepHtmlPipe } from './keepHtmlPipe/keep-html.pipe';
import { LoadingSpinnerComponent } from './LoadingSpinner/loadingSpinner.component';
import { ChartGeneratorComponent } from './chartGenerator/chartgenerator.component';
import { MatNativeDateModule, MatDatepickerModule, MatFormFieldModule, MatButtonModule, MatInputModule, MatRippleModule } from '@angular/material';
import { DatePickerService } from './datepicker-material/datePicker.service';
import { DatepickerMaterialComponent } from './datepicker-material/datepicker-material.component';
import { DatePipe } from '@angular/common';
import { SignalRService } from './signalR/signalR.service';
import { SignalRMessage } from './signalR/signalR.message';
import { ChartFooterComponent } from './chartFooter/chartFooter.component';
import { DailyRateFooterComponent } from './dailyRateFooter/dailyRateFooter.component';

@NgModule({
   imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MatNativeDateModule, MatDatepickerModule, MatFormFieldModule, MatButtonModule, MatInputModule, MatRippleModule
   ],
   declarations: [
        ModalComponent,
        DivFlexibleComponent,
        KeepHtmlPipe,
        LoadingSpinnerComponent,
        ChartGeneratorComponent,
        DatepickerMaterialComponent,
        ChartFooterComponent,
        DailyRateFooterComponent
   ],
   exports: [
        ModalComponent,
        DivFlexibleComponent,
        KeepHtmlPipe,
        LoadingSpinnerComponent,
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
        SignalRService
    ]
})
export class SharedModule { 
  
}
