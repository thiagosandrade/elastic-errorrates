import { Routes } from "@angular/router";

import { HomeComponent } from './home';
import { LoginComponent, RegisterComponent } from './account';
import { authGuard } from './_helpers';
import { ChartsComponent } from "./charts/charts.component";
import { LogsComponent } from "./logs/logs.component";
import { LogsDetailedComponent } from "./logs-detailed/logs-detailed.component";

const usersRoutes = () => import('./users/users.routes').then(x => x.USERS_ROUTES);

export const APP_ROUTES: Routes = [
    { path: '', component: HomeComponent, canActivate: [authGuard] },
    { path: 'users', loadChildren: usersRoutes, canActivate: [authGuard] },
    { path: 'account/login', component: LoginComponent },
    { path: 'account/register', component: RegisterComponent },
    { path: 'charts', component: ChartsComponent },
    { path: 'logs', component: LogsComponent },
    { path: 'logs/detailed', component: LogsDetailedComponent },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];
