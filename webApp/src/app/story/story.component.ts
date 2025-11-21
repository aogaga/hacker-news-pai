import { Component, OnInit } from '@angular/core';
import { StoriesService } from '../services/story/stories.service';
import { Story } from '../models/story/story.model';
import {RouterOutlet} from '@angular/router';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-story',
  imports: [CommonModule, FormsModule],
  templateUrl: './story.component.html',
  standalone: true,
  styleUrls: ['./story.component.scss']
})
export class StoryComponent implements OnInit {

  stories: Story[] = [];
  page: number = 1;
  pageSize: number = 20;
  searchTerm: string = '';
  loading = true;

  constructor(private storyService: StoriesService) {}

  ngOnInit() {
    this.loadStories();
  }

  loadStories(): void {
    this.loading = true;
    this.storyService.getStories(this.page, this.pageSize).subscribe({
      next: (data: Story[]) => {
        this.stories = data;
        this.loading = false;
      },
      error: (err) => console.error('Error fetching stories:', err)
    });
  }
  searchStories(term: string): void {
      this.storyService.searchStories(term, this.page, this.pageSize).subscribe({
        next: (data: Story[]) => {
          this.stories = data;
          this.loading = false;
        },
        error: (err) => console.error('Error fetching stories:', err)
      });
  }

  prevPage(){
    if (this.page > 1) {
      this.page--;
      this.loadPage();
    }
  }

  nextPage(){
    this.page++;
    this.loadPage();
  }
  private loadPage(): void {
    if (this.searchTerm.trim()) {
      this.searchStories(this.searchTerm);
    } else {
      this.loadStories();
    }
  }

}
