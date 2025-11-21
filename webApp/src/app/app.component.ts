import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {StoryComponent} from './story/story.component';
import {CommonModule} from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, StoryComponent, CommonModule, HttpClientModule],
  templateUrl: './app.component.html',
  standalone: true,
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Recent Hacker News Storis From API';
}
