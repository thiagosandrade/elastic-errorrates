import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from './api/api.service';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
  ],
  exports: [
  ],
  providers: [
    ApiService]
})
export class SharedModule { 
  
}
