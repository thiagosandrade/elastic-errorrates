import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from './api/api.service';
import { ModalComponent } from './modal/modal.component';
import { ModalService } from './modal/modal.service';
import { FormsModule } from '@angular/forms';
import { DivFlexibleComponent } from './divFlexible/divFlexible.component';
import { KeepHtmlPipe } from './keepHtmlPipe/keep-html.pipe';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  declarations: [
    ModalComponent,
    DivFlexibleComponent,
    KeepHtmlPipe
],
  exports: [
    ModalComponent,
    DivFlexibleComponent,
    KeepHtmlPipe
  ],
  providers: [
    ApiService,
    ModalService
  ]
})
export class SharedModule { 
  
}
