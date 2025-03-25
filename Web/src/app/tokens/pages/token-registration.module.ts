import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TokenRegistrationComponent } from './token-registration.component';
import { TokenRegistrationRoutingModule } from './token-registration-routing.module';
import { HttpService } from 'src/core/http-service/http.service';
import { TokenRepository } from '../core/repositories/token.repository';

@NgModule({
  imports: [
    CommonModule,
    TokenRegistrationRoutingModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  declarations: [
    TokenRegistrationComponent
  ],
  providers: [
    HttpService,
    TokenRepository
  ]
})
export class TokenRegistrationModule { }