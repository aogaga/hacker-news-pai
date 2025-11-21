import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {HttpMethod} from './HttpMethod';
import {defer, Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpRequestService {

  get apiUrl() {
    return `${environment.baseUrl}`;
  }
  constructor(private http: HttpClient) { }

  public perform<T>(method: HttpMethod, url: string, data?: any): Observable<T> {
    const routeUrl = `${this.apiUrl}${url}`;
    let response$: Observable<T>;

    switch (method) {
      case HttpMethod.GET:
        response$ = this.http.get<T>(routeUrl);
        break;
      case HttpMethod.DELETE:
        response$ = this.http.delete<T>(routeUrl);
        break;
      case HttpMethod.POST:
        response$ = this.http.post<T>(routeUrl, data);
        break;
      case HttpMethod.PUT:
        response$ = this.http.put<T>(routeUrl, data);
        break;
      case HttpMethod.PATCH:
        response$ = this.http.patch<T>(routeUrl, data);
        break;
      default:
        throw new Error(`Unimplemented method: ${method}`);
    }

    return defer(() => response$);
  }
}
