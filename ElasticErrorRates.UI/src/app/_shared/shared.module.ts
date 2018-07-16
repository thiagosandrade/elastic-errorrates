import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiDashboardService } from './api/dashboard/api.service';
import { ApiLogService } from './api/log/api.service';
import { ModalComponent } from './modal/modal.component';
import { ModalService } from './modal/modal.service';
import { FormsModule } from '@angular/forms';
import { DivFlexibleComponent } from './divFlexible/divFlexible.component';
import { KeepHtmlPipe } from './keepHtmlPipe/keep-html.pipe';
import { LoadingSpinnerComponent } from './LoadingSpinner/loadingSpinner.component';
import { ChartGeneratorComponent } from './chartGenerator/chartgenerator.component';

@NgModule({
   imports: [
      CommonModule,
      FormsModule
   ],
   declarations: [
      ModalComponent,
      DivFlexibleComponent,
      KeepHtmlPipe,
      LoadingSpinnerComponent,
      ChartGeneratorComponent
   ],
   exports: [
      ModalComponent,
      DivFlexibleComponent,
      KeepHtmlPipe,
      LoadingSpinnerComponent,
      ChartGeneratorComponent
   ],
   providers: [
      ApiLogService,
      ApiDashboardService,
      ModalService
   ]
})
export class SharedModule { 
  
}
