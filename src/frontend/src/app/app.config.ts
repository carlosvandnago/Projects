import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideAnimations } from '@angular/platform-browser/animations';
import { routes } from './app.routes';
import { clientsReducer } from './core/store/clients/clients.reducer';
import { risksReducer } from './core/store/risks/risks.reducer';
import { ClientsEffects } from './core/store/clients/clients.effects';
import { RisksEffects } from './core/store/risks/risks.effects';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    provideAnimations(),
    provideStore({ clients: clientsReducer, risks: risksReducer }),
    provideEffects([ClientsEffects, RisksEffects]),
    provideStoreDevtools({ maxAge: 25, logOnly: false })
  ]
};
