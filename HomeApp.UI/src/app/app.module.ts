import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './_shared/shared.module';
import { ProductsModule } from './products/products.module';
import { LogsModule } from './logs/logs.module';

@NgModule({
  declarations: [
    AppComponent
],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule.forRoot(),
    SharedModule,
    ProductsModule,
    LogsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
