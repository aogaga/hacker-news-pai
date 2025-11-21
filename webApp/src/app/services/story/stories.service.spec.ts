// src/app/services/story/stories.service.spec.ts
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { StoriesService } from './stories.service';
import { environment } from '../../environments/environment';

describe('StoriesService', () => {
  let service: StoriesService;
  let httpMock: HttpTestingController;
  const baseUrl = environment.baseUrl;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [StoriesService]
    });
    service = TestBed.inject(StoriesService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getStories should GET default page and pageSize', () => {
    const mockStories: any[] = [{ id: 1, title: 'One' }, { id: 2, title: 'Two' }];

    service.getStories().subscribe(stories => {
      expect(stories).toEqual(mockStories);
    });

    const req = httpMock.expectOne(`${baseUrl}?page=1&pageSize=20`);
    expect(req.request.method).toBe('GET');
    req.flush(mockStories);
  });

  it('getStories should allow custom page and pageSize', () => {
    const mockStories: any[] = [{ id: 3, title: 'Three' }];

    service.getStories(2, 10).subscribe(stories => {
      expect(stories).toEqual(mockStories);
    });

    const req = httpMock.expectOne(`${baseUrl}?page=2&pageSize=10`);
    expect(req.request.method).toBe('GET');
    req.flush(mockStories);
  });

  it('searchStories should call /search with encoded term and default paging', () => {
    const term = 'hello world/?&';
    const encoded = encodeURIComponent(term);
    const mockStories: any[] = [{ id: 4, title: 'Found' }];

    service.searchStories(term).subscribe(stories => {
      expect(stories).toEqual(mockStories);
    });

    const req = httpMock.expectOne(`${baseUrl}/search?term=${encoded}&page=1&pageSize=20`);
    expect(req.request.method).toBe('GET');
    req.flush(mockStories);
  });

});
