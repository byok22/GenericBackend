import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations'; // <-- 1. IMPORTAR
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http'; 
import { authInterceptor } from './shared/interceptor/auth.interceptor';


export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes),  provideAnimations(),  provideHttpClient(withInterceptors([authInterceptor])) ]
};


