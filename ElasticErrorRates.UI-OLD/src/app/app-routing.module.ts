import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AuthGuard } from './_guards';

const routes: Routes = [
  {
    path: '',
    component: AppComponent
  },
  { 
    path: 'register', 
    loadChildren: "../app/users/register/register.module#RegisterModule" 
  },
  {
    path: 'login',
    loadChildren: "../app/users/login/login.module#LoginModule" 
  },
  {
    path: 'logs',
    loadChildren: "../app/logs/index/logs.module#LogsModule"
  },
  {
    path: 'logs/detailed',
    loadChildren: "../app/logs/detailed/logsdetailed.module#LogsDetailedModule"
  },
  {
    path: 'logs/dashboard',
    loadChildren: "../app/logs/dashboard/dashboard.module#DashboardModule",
    canActivate: [AuthGuard]
  }
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
