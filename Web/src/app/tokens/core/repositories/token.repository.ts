import { Injectable } from '@angular/core';
import { HttpService } from 'src/core/http-service/http.service';
import { Observable } from 'rxjs';
import { TokenSearch } from '../models/token-search';
import { TokenSettings } from '../models/token-settings';
import { TokenResponse } from '../models/token-response';

const API_URL = '/tokens';

@Injectable()
export class TokenRepository {

  constructor(
    private readonly httpService: HttpService
  ) {}

  public findAll(): Observable<TokenSearch[]> {
    return this.httpService.get(API_URL);
  }

  public create(token: TokenSettings): Observable<TokenResponse> {
    const body: string = JSON.stringify(token);
    return this.httpService.post(API_URL, body);
  }

  public revoke(id: number): Observable<void> {
    const url = `${API_URL}/${id}`;
    return this.httpService.delete(url);
  }

}