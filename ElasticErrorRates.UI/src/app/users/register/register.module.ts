import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../../_shared/shared.module';

import { RegisterComponent } from './register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { RegisterRoutingModule } from './register-routing.module';

@NgModule({
  imports: [
    SharedModule,
    CommonModule, 
    FormsModule,
    ReactiveFormsModule,
    RegisterRoutingModule,
    RouterModule
  ],
  declarations: [
    RegisterComponent
],
 exports: [
  
 ],
  providers: [
  ]
})
export class RegisterModule { }
