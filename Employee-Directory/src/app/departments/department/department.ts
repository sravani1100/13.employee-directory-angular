import {
  Component,
  computed,
  OnInit,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';
import {
  FormsModule,
  ReactiveFormsModule
} from '@angular/forms';

import { Sidebar } from '../../shared/sidebar/sidebar';
import { Searchbar } from '../../shared/searchbar/searchbar';
import { ConfirmModalComponent } from '../../shared/confirm-modal/confirm-modal/confirm-modal';
import { DepartmentCardComponent } from '../department-card/department-card';
import { DepartmentFormComponent } from '../department-form/department-form';

import { DepartmentModel } from '../../models/department/department.model';

import { DepartmentService } from '../../shared/services/department-service';
import { ToastService } from '../../shared/services/toast-service';
import { UserService } from '../../shared/services/user';
import { DEPARTMENT_MESSAGES } from '../../shared/constants/department';
import { COMMON_MESSAGES } from '../../shared/constants/common';

@Component({
  selector: 'app-department',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    Sidebar,
    Searchbar,
    DepartmentCardComponent,
    DepartmentFormComponent,
    ConfirmModalComponent
  ],
  templateUrl: './department.html',
  styleUrl: './department.css'
})
export class DepartmentComponent implements OnInit {

  departments = signal<DepartmentModel[]>([]);

  showForm = false;

  confirmVisible = false;
  confirmMessage = '';

  private confirmAction: (() => void) | null = null;

  isAdmin = computed(() => {
    return this.userService.user()?.role === 'Admin';
  });

  constructor(
    private departmentService: DepartmentService,
    private userService: UserService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.departmentService
      .getAllDepartments()
      .subscribe({
        next: (response: DepartmentModel[]) => {
          this.departments.set(response);
        },
        error: (error) => {
          console.error(error);
        }
      });
  }

  openForm(): void {
    this.showForm = true;
  }

  saveDepartment(department: DepartmentModel): void {
    this.departmentService
      .addDepartment(department)
      .subscribe({
        next: () => {
          this.toastService.show(
            DEPARTMENT_MESSAGES.CREATED,
            'success'
          );

          this.showForm = false;
          this.loadDepartments();
        },

        error: (err) => {
          this.toastService.show(
            DEPARTMENT_MESSAGES.CREATE_FAILED,
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