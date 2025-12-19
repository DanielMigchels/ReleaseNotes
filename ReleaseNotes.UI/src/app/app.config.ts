import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from '../interceptor/auth.interceptor';
import { provideIcons } from '@ng-icons/core';
import { heroArrowsRightLeft, heroBarsArrowDown, heroBookOpen, heroCheckCircle, heroChevronLeft, heroPencil, heroPlus, heroTrash, heroXCircle, heroXMark } from '@ng-icons/heroicons/outline';
import { heroUsersSolid } from '@ng-icons/heroicons/solid';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideIcons({ heroChevronLeft, heroPlus, heroXMark, heroTrash, heroPencil, heroUsersSolid, heroCheckCircle, heroXCircle, heroBarsArrowDown, heroBookOpen, heroArrowsRightLeft}),
  ]
};
