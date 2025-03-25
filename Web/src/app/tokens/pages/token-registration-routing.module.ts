import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { authGuard } from 'src/core/auth/services/auth.service';
import { TokenRegistrationComponent } from './token-registration.component';

const routes: Route[] = [
  {
    path: '',
    component: TokenRegistrationComponent,
    canActivate: [authGuard]
  }
]

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class TokenRegistrationRoutingModule {}