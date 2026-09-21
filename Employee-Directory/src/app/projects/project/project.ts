import {
  Component,
  computed,
  OnInit,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';

import { Sidebar } from '../../shared/sidebar/sidebar';
import { Searchbar } from '../../shared/searchbar/searchbar';
import { ProjectCardComponent } from '../project-card/project-card';
import { ProjectFormComponent } from '../project-form/project-form';
import { ConfirmModalComponent } from '../../shared/confirm-modal/confirm-modal/confirm-modal';

import { ProjectModel } from '../../models/project/project.model';

import { ProjectService } from '../../shared/services/project-service';
import { ToastService } from '../../shared/services/toast-service';
import { UserService } from '../../shared/services/user';
import { PROJECT_MESSAGES } from '../../shared/constants/project';
import { COMMON_MESSAGES } from '../../shared/constants/common';

@Component({
  selector: 'app-project',
  standalone: true,
  imports: [
    CommonModule,
    Sidebar,
    Searchbar,
    ProjectCardComponent,
    ProjectFormComponent,
    ConfirmModalComponent
  ],
  templateUrl: './project.html',
  styleUrl: './project.css'
})
export class ProjectComponent implements OnInit {

  projects = signal<ProjectModel[]>([]);

  showForm = false;

  confirmVisible = false;
  confirmMessage = '';

  private confirmAction: (() => void) | null = null;

  isAdmin = computed(() => {
    return this.userService.user()?.role === 'Admin';
  });

  constructor(
    private projectService: ProjectService,
    private userService: UserService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.projectService
      .getProjects()
      .subscribe({
        next: (response: ProjectModel[]) => {
          this.projects.set(response);
        },
        error: (error) => {
          console.error(error);
        }
      });
  }

  openForm(): void {
    this.showForm = true;
  }

  saveProject(project: ProjectModel): void {
    this.projectService
      .addProject(project)
      .subscribe({
        next: () => {
          this.toastService.show(
            PROJECT_MESSAGES.CREATED,
            'success'
          );

          this.showForm = false;
          this.loadProjects();
        },

        error: (err) => {

          this.toastService.show(
            PROJECT_MESSAGES.CREATE_FAILED,
            'error'
          );
        }
      });
  }

  openConfirmModal(
    message: string,
    action: () => void
  ): void {

    this.confirmMessage = message;
    this.confirmAction = action;
    this.confirmVisible = true;
  }

  onConfirm(): void {
    this.confirmVisible = false;

    if (this.confirmAction) {
      this.confirmAction();
      this.confirmAction = null;
    }
  }

  onCancel(): void {
    this.confirmVisible = false;
    this.confirmAction = null;
  }

  closeForm(): void {
    this.openConfirmModal(
      COMMON_MESSAGES.CONFIRM_CLOSE_FORM,
      () => {
        this.showForm = false;
      }
    );
  }
}