import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'tokens',
    loadChildren: () => {
      return import('./tokens/pages/token-registration.module').then(m => m.TokenRegistrationModule)
    }
  },
  {
    path: '',
    redirectTo: '/tokens',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
