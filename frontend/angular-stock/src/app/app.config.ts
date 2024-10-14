import { ApplicationConfig } from '@angular/core'
import { provideRouter } from '@angular/router'

import { routes } from './app.routes'
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async'

// ChartJS
import { provideCharts, withDefaultRegisterables } from 'ng2-charts'

// HttpClient
import { provideHttpClient } from '@angular/common/http'

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), 
    provideHttpClient(),
    provideAnimationsAsync(),
    provideCharts(
      withDefaultRegisterables(),
    ),
  ]
};
