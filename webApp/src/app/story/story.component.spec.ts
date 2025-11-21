// src/app/story/story.component.spec.ts
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { StoryComponent } from './story.component';
import { StoriesService } from '../services/story/stories.service';
import { Story } from '../models/story/story.model';

describe('StoryComponent', () => {
  let component: StoryComponent;
  let fixture: ComponentFixture<StoryComponent>;
  let storiesService: jasmine.SpyObj<StoriesService>;

  const mockStories: Story[] = [{ id: 1, title: 'One' } as any];

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('StoriesService', ['getStories', 'searchStories']);

    await TestBed.configureTestingModule({
      imports: [StoryComponent],
      providers: [{ provide: StoriesService, useValue: spy }]
    }).compileComponents();

    storiesService = TestBed.inject(StoriesService) as jasmine.SpyObj<StoriesService>;
    storiesService.getStories.and.returnValue(of(mockStories));
    storiesService.searchStories.and.returnValue(of(mockStories));

    fixture = TestBed.createComponent(StoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('ngOnInit should load stories with default page and pageSize', () => {
    expect(storiesService.getStories).toHaveBeenCalledWith(1, 20);
    expect(component.stories).toEqual(mockStories);
  });

  it('nextPage increments page and loads stories when no searchTerm', () => {
    component.page = 1;
    component.searchTerm = '';
    storiesService.getStories.calls.reset();

    component.nextPage();

    expect(component.page).toBe(2);
    expect(storiesService.getStories).toHaveBeenCalledWith(2, 20);
  });

  it('prevPage decrements page and loads stories when page > 1', () => {
    component.page = 2;
    storiesService.getStories.calls.reset();

    component.prevPage();

    expect(component.page).toBe(1);
    expect(storiesService.getStories).toHaveBeenCalledWith(1, 20);
  });

  it('loadPage calls searchStories when searchTerm is present', () => {
    component.page = 3;
    component.searchTerm = 'hello';
    storiesService.searchStories.calls.reset();

    (component as any).loadPage();

    expect(storiesService.searchStories).toHaveBeenCalledWith('hello', 3, 20);
  });

  it('searchStories method calls service and updates stories', () => {
    const found = [{ id: 99, title: 'Found' } as any];
    storiesService.searchStories.and.returnValue(of(found));

    component.searchStories('term');

    expect(storiesService.searchStories).toHaveBeenCalledWith('term', component.page, component.pageSize);
    expect(component.stories).toEqual(found);
  });
});
