import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: 'contacts',
    loadChildren: 'app/products/products.module#ProductsModule',
  },
  {
    path: 'contacts',
    loadChildren: 'app/logs/logs.module#LogsModule',
  },
  {
    path: 'contacts',
    loadChildren: 'app/logdetailed/logs.module#LogsModule',
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule
  ],
  providers: [
  ]
})
export class AppRoutingModule { }
