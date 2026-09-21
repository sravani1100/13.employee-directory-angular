import { Component, Input } from '@angular/core';
import { ProjectModel } from '../../models/project/project.model';

@Component({
  selector: 'app-project-card',
  standalone: true,
  imports: [],
  templateUrl: './project-card.html',
  styleUrl: './project-card.css',
})
export class ProjectCardComponent {
  @Input()
  project!: ProjectModel;
}
