import { Component, OnInit } from '@angular/core';
import { StoriesService } from '../services/story/stories.service';
import { Story } from '../models/story/story.model';

@Component({
  selector: 'app-story',
  templateUrl: './story.component.html',
  styleUrls: ['./story.component.scss']
})
export class StoryComponent implements OnInit {

  stories: Story[] = [];
  page: number = 1;
  pageSize: number = 20;

  constructor(private storyService: StoriesService) { }

  ngOnInit() {
    this.loadStories();
    console.log(this.stories);
  }


  loadStories(): void {
    this.storyService.getStories(this.page, this.pageSize).subscribe({
      next: (data: Story[]) => this.stories = data,
      error: (err) => console.error('Error fetching stories:', err)
    });
  }
}
