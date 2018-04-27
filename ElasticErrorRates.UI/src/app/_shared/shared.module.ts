import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from './api/api.service';
import { ModalComponent } from './modal/modal.component';
import { ModalService } from './modal/modal.service';
import { FormsModule } from '@angular/forms';
import { DivFlexibleComponent } from './divFlexible/divFlexible.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  declarations: [
    ModalComponent,
    DivFlexibleComponent
],
  exports: [
    ModalComponent,
    DivFlexibleComponent
  ],
  providers: [
    ApiService,
    ModalService
  ]
})
export class SharedModule { 
  
}
