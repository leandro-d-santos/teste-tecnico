import { inject, Injectable } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { HttpService } from 'src/core/http-service/http.service';

@Injectable({ providedIn: "root" })
class AuthService {

  constructor(private readonly httpService: HttpService) { }

  public async checkLogin(): Promise<boolean> {
    return new Promise(resolve => {
      this.httpService.get('/auth')
      .subscribe({
        next: () => resolve(true),
        error: () => resolve(false)
      });
    })
  }

}

export const authGuard: CanActivateFn = async (route, state) => {
  const service: AuthService = inject(AuthService);
  return await service.checkLogin();
};