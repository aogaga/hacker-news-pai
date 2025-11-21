import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {StoryComponent} from './story/story.component';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, StoryComponent, CommonModule],
  templateUrl: './app.component.html',
  standalone: true,
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'webApp';
}
