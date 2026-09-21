import {
  Component,
  OnInit,
  signal,
  computed
} from '@angular/core';

import * as XLSX from 'xlsx';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Sidebar } from '../../shared/sidebar/sidebar';
import { Searchbar } from '../../shared/searchbar/searchbar';
import { AlphabetFilterComponent } from '../alphabet-filter/alphabet-filter';
import { FilterPanelComponent } from '../../shared/filter-panel/filter-panel';
import { EmployeeFormComponent } from '../employee-form/employee-form';
import { EmployeeListComponent } from '../employee-list/employee-list';
import { EmployeeService } from '../../shared/services/employee.service';
import { Status } from '../../models/enums/status.enum';
import { EmployeeRequestDTO } from '../../models/employee/employee-request.model';
import { EmployeeResponseDTO } from '../../models/employee/employee-response.model';
import { EmployeeDetailsModel } from '../../models/employee/employee-details.model';
import { ToastService } from '../../shared/services/toast-service';
import { ConfirmModalComponent } from '../../shared/confirm-modal/confirm-modal/confirm-modal';
import { UserService } from '../../shared/services/user';

import { COMMON_MESSAGES } from '../../shared/constants/common';
import { EMPLOYEE_MESSAGES } from '../../shared/constants/employee';

@Component({
  selector: 'app-employee',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    Sidebar,
    Searchbar,
    AlphabetFilterComponent,
    FilterPanelComponent,
    EmployeeFormComponent,
    EmployeeListComponent,
    ConfirmModalComponent
  ],
  templateUrl: './employee.html',
  styleUrl: './employee.css'
})
export class EmployeeComponent implements OnInit {

  employees = signal<EmployeeResponseDTO[]>([]);

  selectedFilters = signal({
    status: [] as number[],
    locationIds: [] as number[],
    departmentIds: [] as number[]
  });

  selectedLetter = signal<string | null>(null);

  searchText = signal('');

  confirmVisible = false;
  confirmMessage = '';
  private confirmAction: (() => void) | null = null;

  filteredEmployees = computed(() => {
    let data = [...this.employees()];
    const filters = this.selectedFilters();

    // Status
    if (filters.status.length > 0) {
      data = data.filter(emp =>
        filters.status.includes(emp.status)
      );
    }

    // Location
    if (filters.locationIds.length > 0) {
      data = data.filter(emp =>
        emp.locationId != null &&
        filters.locationIds.includes(emp.locationId)
      );
    }

    // Department
    if (filters.departmentIds.length > 0) {
      data = data.filter(emp =>
        emp.departmentId != null &&
        filters.departmentIds.includes(emp.departmentId)
      );
    }

    // Alphabet
    const letter = this.selectedLetter();

    if (letter) {
      data = data.filter(emp =>
        emp.firstName
          .toUpperCase()
          .startsWith(letter.toUpperCase())
      );
    }

    // Search
    const search = this.searchText()
      .trim()
      .toLowerCase();

    if (search) {
      data = data.filter(emp =>
        emp.employeeNumber?.toLowerCase().includes(search) ||
        emp.firstName?.toLowerCase().includes(search) ||
        emp.lastName?.toLowerCase().includes(search) ||
        emp.email?.toLowerCase().includes(search) ||
        emp.location?.toLowerCase().includes(search) ||
        emp.department?.toLowerCase().includes(search) ||
        emp.roleName?.toLowerCase().includes(search)
      );
    }
    return data;
  });

  showForm = signal(false);
  isEdit = signal(false);
  isView = signal(false);

  addEmployee = signal<EmployeeRequestDTO | null>(null);

  constructor(
    private employeeService: EmployeeService,
    private userService: UserService,
    private toastService: ToastService
  ) {}

  isAdmin = computed(() => {
    return this.userService.user()?.role === 'Admin';
  });

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.employeeService
      .getEmployees()
      .subscribe({
        next: (data) => {
          this.employees.set(data);
        },
        error: (err) => {
          console.error(err);
        }
      });
  }

  openAddEmployee(): void {
    this.isEdit.set(false);
    this.isView.set(false);

    this.addEmployee.set({
      employeeId: 0,
      employeeNumber: '',
      firstName: '',
      lastName: '',
      dateOfBirth: null,
      email: '',
      mobileNumber: '',
      joiningDate: '',
      status: Status.Active,
      managerId: null,
      locationId: 0,
      locationName: '',
      departmentId: 0,
      departmentName: '',
      roleId: 0,
      roleName: '',
      projectId: null,
      projectName: 'N/A',
      password: ''
    });

    this.showForm.set(true);
  }

  editEmployee(employee: EmployeeResponseDTO): void {
    this.isEdit.set(true);
    this.isView.set(false);
    this.showForm.set(true);

    this.employeeService
      .getEmployeeById(employee.employeeId)
      .subscribe({
        next: (data) => {
          this.addEmployee.set({
            employeeId: data.employeeId,
            employeeNumber: data.employeeNumber,
            firstName: data.firstName,
            lastName: data.lastName,
            email: data.email,
            mobileNumber: data.mobileNumber,
            dateOfBirth: data.dateOfBirth,
            joiningDate: data.joiningDate,
            status: data.status,
            managerId: data.managerId,
            locationId: data.locationId,
            locationName: data.locationName,
            departmentId: data.departmentId,
            departmentName: data.departmentName,
            roleId: data.roleId,
            roleName: data.roleName,
            projectId: data.projectId,
            projectName: data.projectName ?? 'N/A',
            password: ''
          });
        }
      });
  }

  viewEmployee(employee: EmployeeResponseDTO): void {
    this.isEdit.set(false);
    this.isView.set(true);
    this.showForm.set(true);

    this.employeeService
      .getEmployeeById(employee.employeeId)
      .subscribe({
        next: (data: EmployeeDetailsModel) => {
          this.addEmployee.set({
            employeeId: data.employeeId,
            employeeNumber: data.employeeNumber,
            firstName: data.firstName,
            lastName: data.lastName,
            email: data.email,
            mobileNumber: data.mobileNumber,
            dateOfBirth: data.dateOfBirth,
            joiningDate: data.joiningDate,
            status: data.status,
            managerId: data.managerId,
            locationId: data.locationId,
            locationName: data.locationName,
            departmentId: data.departmentId,
            departmentName: data.departmentName,
            roleId: data.roleId,
            roleName: data.roleName,
            projectId: data.projectId,
            projectName: data.projectName ?? 'N/A',
            password: ''
          });
        },
        error: (err) => {
          console.error(err);
        }
      });
  }

  saveEmployee(employee: EmployeeRequestDTO): void {
    if (this.isEdit()) {
      this.employeeService
        .updateEmployee(
          employee.employeeNumber,
          employee
        )
        .subscribe({
          next: () => {
            this.loadEmployees();
            this.showForm.set(false);

            this.toastService.show(
              EMPLOYEE_MESSAGES.UPDATED,
              'success'
            );
          },
          error: () => {
            this.toastService.show(
              EMPLOYEE_MESSAGES.UPDATE_FAILED,
              'error'
            );
          }
        });
    } else {
      this.employeeService
        .createEmployee(employee)
        .subscribe({
          next: () => {
            this.loadEmployees();
            this.showForm.set(false);

            this.toastService.show(
              EMPLOYEE_MESSAGES.CREATED,
              'success'
            );
          },
          error: () => {
            this.toastService.show(
              EMPLOYEE_MESSAGES.CREATE_FAILED,
              'error'
            );
          }
        });
    }
  }

  deleteEmployee(employeeNumber: string): void {
    this.openConfirmModal(
      EMPLOYEE_MESSAGES.CONFIRM_DELETE,
      () => {
        this.employeeService
          .deleteEmployee(employeeNumber)
          .subscribe({
            next: () => {
              this.loadEmployees();

              this.toastService.show(
                EMPLOYEE_MESSAGES.DELETED,
                'success'
              );
            },
            error: () => {
              this.toastService.show(
                EMPLOYEE_MESSAGES.DELETE_FAILED,
                'error'
              );
            }
          });
      }
    );
  }

  deleteSelectedEmployees(employeeNumbers: string[]): void {
    this.openConfirmModal(
      `Delete ${employeeNumbers.length} employee(s)?`,
      () => {
        employeeNumbers.forEach(employeeNumber => {
          this.employeeService
            .deleteEmployee(employeeNumber)
            .subscribe({
              next: () => {
                this.loadEmployees();
              },
              error: () => {
                this.toastService.show(
                  EMPLOYEE_MESSAGES.DELETE_MULTIPLE_FAILED,
                  'error'
                );
              }
            });
        });

        this.toastService.show(
          EMPLOYEE_MESSAGES.DELETED,
          'success'
        );
      }
    );
  }

  onLetterFilter(letter: string | null): void {
    this.selectedLetter.set(letter);
  }

  onApplyFilters(filters: {
    status: number[];
    locationIds: number[];
    departmentIds: number[];
  }): void {
    this.selectedFilters.set({
      status: [...filters.status],
      locationIds: [...filters.locationIds],
      departmentIds: [...filters.departmentIds]
    });
  }

  onResetFilters(): void {
    this.selectedFilters.set({
      status: [],
      locationIds: [],
      departmentIds: []
    });
  }

  onSearch(value: string): void {
    this.searchText.set(value);
  }

  exportToExcel(filename: string = EMPLOYEE_MESSAGES.EXPORT_FILENAME): void {
    const employees = this.filteredEmployees();

    if (employees.length === 0) {
      this.toastService.show(EMPLOYEE_MESSAGES.NO_DATA_TO_EXPORT, 'error');
      return;
    }

    const exportData = employees.map(emp => ({
      USER: `${emp.firstName} ${emp.lastName}`,
      EMAIL: emp.email,
      LOCATION: emp.location,
      DEPARTMENT: emp.department,
      ROLE: emp.roleName,
      EMP_NO: emp.employeeNumber,
      STATUS: emp.status === 1 ? 'Active' : 'Inactive',
      JOIN_DATE: emp.joiningDate
    }));

    const worksheet = XLSX.utils.json_to_sheet(exportData);
    const workbook = XLSX.utils.book_new();

    XLSX.utils.book_append_sheet(
      workbook,
      worksheet,
      'Employees'
    );

    XLSX.writeFile(
      workbook,
      filename
    );
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
    if (this.isView()) {
      this.showForm.set(false);
      return;
    }

    this.openConfirmModal(
      COMMON_MESSAGES.CONFIRM_CLOSE_FORM,
      () => {
        this.showForm.set(false);
      }
    );
  }
}