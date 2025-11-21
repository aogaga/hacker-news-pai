import {Injectable} from '@angular/core';
import {HttpRequestService} from '../http/http-request.service';
import {Observable} from 'rxjs';
import {Story} from '../../models/story/story.model';
import {HttpMethod} from '../http/HttpMethod';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StoriesService {
  constructor(private http: HttpClient) { }

  private readonly baseUrl = environment.baseUrl;

  getStories(page: number = 1, pageSize: number = 20): Observable<Story[]> {
    const url = `${this.baseUrl}?page=${page}&pageSize=${pageSize}`;
    return this.http.get<Story[]>(url);
  }

  searchStories(term: string, page: number = 1, pageSize: number = 20): Observable<Story[]> {
    const url = `${this.baseUrl}/search?query=${encodeURIComponent(term)}&page=${page}&pageSize=${pageSize}`;
    return this.http.get<Story[]>(url);
  }

}
