import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../../_shared/shared.module';

import { LoginComponent } from './login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { LoginRoutingModule } from './login-routing.module';


@NgModule({
  imports: [
    SharedModule,
    CommonModule, 
    FormsModule,
    ReactiveFormsModule,
    LoginRoutingModule,
    RouterModule
  ],
  declarations: [
    LoginComponent
],
 exports: [
 ],
  providers: [
  ]
})
export class LoginModule { }
