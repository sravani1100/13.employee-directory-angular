import {
  Component,
  EventEmitter,
  Output,
  OnInit
} from '@angular/core';

import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  ValidatorFn,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';

import { CommonModule } from '@angular/common';

import { ProjectModel } from '../../models/project/project.model';
import { ProjectService } from '../../shared/services/project-service';
import { noConsecutiveSpacesValidator } from '../../shared/validators/validators';

@Component({
  selector: 'app-project-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './project-form.html',
  styleUrl: './project-form.css'
})
export class ProjectFormComponent implements OnInit {

  @Output()
  close = new EventEmitter();

  @Output()
  save = new EventEmitter();

  projectForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {

    this.projectForm = this.fb.group({
      projectName: [
        '',
        [
          Validators.required,
          Validators.maxLength(20),
          Validators.pattern(/^[A-Za-z ]+$/),
          noConsecutiveSpacesValidator()
        ]
      ]
    });
  }

  addProject(): void {

    if (this.projectForm.invalid) {
      this.projectForm.markAllAsTouched();
      return;
    }

    const projectName =
      this.projectForm.value.projectName.trim();

    this.projectService
      .getProjects()
      .subscribe(projects => {

        const exists = projects.some(
          p =>
            p.projectName.toLowerCase() ===
            projectName.toLowerCase()
        );

        if (exists) {
          this.projectForm
            .get('projectName')
            ?.setErrors({
              duplicate: true
            });

          return;
        }

        const project: ProjectModel = {
          projectId: 0,
          projectName: projectName
        };

        this.save.emit(project);
      });
  }

  get projectName() {
    return this.projectForm.get('projectName');
  }

  closeForm(): void {
    this.close.emit();
  }
}