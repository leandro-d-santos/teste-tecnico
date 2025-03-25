import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class HttpService {

  private readonly baseUrl = 'http://localhost:5260/api';
  private readonly headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
  constructor(private readonly httpClient: HttpClient) { }

  public get(url: string): Observable<any> {
    const fullUrl = this.buildUrl(url);
    return this.httpClient.get(fullUrl, {withCredentials: true, headers: this.headers });
  }

  public put(url: string, body: any): Observable<any> {
    const fullUrl = this.buildUrl(url);
    return this.httpClient.put(fullUrl, body, {withCredentials: true, headers: this.headers });
  }

  public post(url: string, body: any): Observable<any> {
    const fullUrl = this.buildUrl(url);
    return this.httpClient.post(fullUrl, body, {withCredentials: true, headers: this.headers });
  }

  public delete(url: string): Observable<any> {
    const fullUrl = this.buildUrl(url);
    return this.httpClient.delete(fullUrl, {withCredentials: true, headers: this.headers });
  }

  public patch(url: string, body: any): Observable<any> {
    const fullUrl = this.buildUrl(url);
    return this.httpClient.patch(fullUrl, body, {withCredentials: true, headers: this.headers });
  }

  private buildUrl(url: string): string {
    return `${this.baseUrl}${url}`;
  }

} 