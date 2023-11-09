import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';

// fake backend
import { fakeBackendInterceptor } from '@app/_helpers';

import { AppComponent } from '@app/app.component';
import { jwtInterceptor, errorInterceptor } from '@app/_helpers';
import { APP_ROUTES } from '@app/app.routes';
import { DatePipe } from '@angular/common';

bootstrapApplication(AppComponent, {
    providers: [
        DatePipe,
        provideAnimations(),
        provideRouter(APP_ROUTES),
        provideHttpClient(
            withInterceptors([
                jwtInterceptor, 
                errorInterceptor

                // fake backend
                // fakeBackendInterceptor
            ])
        )
    ]
});
