import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProductsRoutingModule } from './products-routing.module';
import { IndexComponent } from './index/index.component';
import { SharedModule } from '../_shared/shared.module';
import { ApiService } from '../_shared/api/api.service';

@NgModule({
  imports: [
    SharedModule,
    CommonModule,
    ProductsRoutingModule
  ],
  declarations: [
    IndexComponent
  ],
  providers: [
    ApiService
  ]
})
export class ProductsModule { }
