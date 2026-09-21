import {
  Component,
  Input,
  Output,
  EventEmitter,
  HostListener,
  OnChanges,
  SimpleChanges,
  computed
} from '@angular/core';

import { CommonModule } from '@angular/common';

import { EmployeeResponseDTO } from '../../models/employee/employee-response.model';
import { UserService } from '../../shared/services/user';

type SortDirection = 'default' | 'asc' | 'desc';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.css'
})
export class EmployeeListComponent implements OnChanges {

  @Input()
  employees: EmployeeResponseDTO[] = [];

  @Output()
  edit = new EventEmitter();

  @Output()
  view = new EventEmitter();

  @Output()
  delete = new EventEmitter();

  @Output()
  deleteSelected = new EventEmitter<string[]>();

  selectedMenuIndex: number | null = null;
  checkedEmployees: string[] = [];

  sortField: keyof EmployeeResponseDTO | null = null;
  sortDirection: SortDirection = 'default';

  originalEmployees: EmployeeResponseDTO[] = [];

  isAdmin = computed(() => {
    return this.userService.user()?.role === 'Admin';
  });

  constructor(
    private userService: UserService
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['employees']) {
      this.checkedEmployees = [];
      this.originalEmployees = [...this.employees];
    }
  }

  selectAll(checked: boolean): void {
    this.checkedEmployees = checked
      ? this.employees.map(emp => emp.employeeNumber)
      : [];
  }

  checkedEmployee(
    checked: boolean,
    employeeNumber: string
  ): void {

    if (checked) {
      if (!this.checkedEmployees.includes(employeeNumber)) {
        this.checkedEmployees.push(employeeNumber);
      }
    } else {
      this.checkedEmployees =
        this.checkedEmployees.filter(
          x => x !== employeeNumber
        );
    }
  }

  editEmployee(employee: EmployeeResponseDTO): void {
    this.selectedMenuIndex = null;
    this.edit.emit(employee);
  }

  viewEmployee(employee: EmployeeResponseDTO): void {
    this.selectedMenuIndex = null;
    this.view.emit(employee);
  }

  deleteEmployee(employeeNumber: string): void {
    this.selectedMenuIndex = null;
    this.delete.emit(employeeNumber);
  }

  deleteSelectedEmployees(): void {
    if (this.checkedEmployees.length === 0) {
      return;
    }

    this.deleteSelected.emit([
      ...this.checkedEmployees
    ]);

    this.checkedEmployees = [];
  }

  getStatus(status: number): string {
    return status === 1
      ? 'Active'
      : 'Inactive';
  }

  sortTable(field: keyof EmployeeResponseDTO): void {

    if (this.sortField !== field) {

      this.sortField = field;
      this.sortDirection = 'asc';

    } else {

      if (this.sortDirection === 'default') {
        this.sortDirection = 'asc';
      } else if (this.sortDirection === 'asc') {
        this.sortDirection = 'desc';
      } else {
        this.sortDirection = 'default';
      }
    }

    if (this.sortDirection === 'default') {
      this.employees = [
        ...this.originalEmployees
      ];

      return;
    }

    this.employees = [...this.employees].sort((a, b) => {

      let valueA: string | number | Date;
      let valueB: string | number | Date;

      if (field === 'firstName') {

        valueA = (
          a.firstName + ' ' + a.lastName
        ).toLowerCase();

        valueB = (
          b.firstName + ' ' + b.lastName
        ).toLowerCase();

      } else if (field === 'joiningDate') {

        valueA = new Date(a.joiningDate);
        valueB = new Date(b.joiningDate);

      } else {

        valueA = a[field] as string | number;
        valueB = b[field] as string | number;

      }

      if (valueA < valueB) {
        return this.sortDirection === 'asc'
          ? -1
          : 1;
      }

      if (valueA > valueB) {
        return this.sortDirection === 'asc'
          ? 1
          : -1;
      }

      return 0;
    });
  }

  toggleMenu(
    index: number,
    event: MouseEvent
  ): void {

    event.stopPropagation();

    this.selectedMenuIndex =
      this.selectedMenuIndex === index
        ? null
        : index;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {

    const target = event.target;

    if (
      !(target instanceof Element) ||
      !target.closest('.menu-container')
    ) {
      this.selectedMenuIndex = null;
    }
  }
}