import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideHttpClient } from '@angular/common/http';
bootstrapApplication(AppComponent,{
  providers: [
    provideHttpClient(),

    // <-- This is key: provides HttpClient for DI
  ],
})
  .catch((err) => console.error(err));
