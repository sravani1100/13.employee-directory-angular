import {
  Component,
  computed,
  OnInit,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Sidebar } from '../../shared/sidebar/sidebar';
import { Searchbar } from '../../shared/searchbar/searchbar';
import { FilterPanelComponent } from '../../shared/filter-panel/filter-panel';
import { RoleCardComponent } from '../role-card/role-card';
import { RoleFormComponent } from '../role-form/role-form';
import { ConfirmModalComponent } from '../../shared/confirm-modal/confirm-modal/confirm-modal';

import { RoleModel } from '../../models/role/role.model';
import { RoleCardModel } from '../../models/role/role-card.model';
import { DepartmentModel } from '../../models/department/department.model';
import { LocationModel } from '../../models/location/location.model';

import { RoleService } from '../../shared/services/role-service';
import { DepartmentService } from '../../shared/services/department-service';
import { LocationService } from '../../shared/services/location.service';
import { ToastService } from '../../shared/services/toast-service';
import { UserService } from '../../shared/services/user';
import { ROLE_MESSAGES } from '../../shared/constants/role';
import { LOCATION_MESSAGES } from '../../shared/constants/location';
import { DEPARTMENT_MESSAGES } from '../../shared/constants/department';
import { COMMON_MESSAGES } from '../../shared/constants/common';

@Component({
  selector: 'app-role',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    Sidebar,
    Searchbar,
    FilterPanelComponent,
    RoleCardComponent,
    RoleFormComponent,
    ConfirmModalComponent
  ],
  templateUrl: './role.html',
  styleUrl: './role.css'
})
export class RoleComponent implements OnInit {

  showForm = false;
  isView = false;

  departments: DepartmentModel[] = [];
  locations: LocationModel[] = [];

  roleCards = signal<RoleCardModel[]>([]);

  selectedFilters = signal({
    locationIds: [] as number[],
    departmentIds: [] as number[]
  });

  searchText = signal('');

  confirmVisible = false;
  confirmMessage = '';

  private confirmAction: (() => void) | null = null;

  isAdmin = computed(() => {
    return this.userService.user()?.role === 'Admin';
  });

  constructor(
    private roleService: RoleService,
    private locationService: LocationService,
    private departmentService: DepartmentService,
    private userService: UserService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadRoleCards();
    this.loadAllDepartments();
    this.loadAllLocations();
  }

  loadRoleCards(): void {
    this.roleService
      .getRoleCards()
      .subscribe({
        next: (data: RoleCardModel[]) => {
          this.roleCards.set(data);
        },

        error: (err) => {
          console.error(
            ROLE_MESSAGES.LOAD_FAILED,
            err
          );

          this.toastService.show(
            ROLE_MESSAGES.LOAD_FAILED,
            'error'
          );
        }
      });
  }

  loadAllLocations(): void {
    this.locationService
      .getLocations()
      .subscribe({
        next: (data) => {
          this.locations = data;
        },

        error: (err) => {
          console.error(
            LOCATION_MESSAGES.LOAD_FAILED,
            err
          );
        }
      });
  }

  loadAllDepartments(): void {
    this.departmentService
      .getAllDepartments()
      .subscribe({
        next: (data) => {
          this.departments = data;
        },

        error: (err) => {
          console.error(
            DEPARTMENT_MESSAGES.LOAD_FAILED,
            err
          );
        }
      });
  }

  saveRole(role: RoleModel): void {
    this.roleService
      .addRole(role)
      .subscribe({
        next: () => {

          this.toastService.show(
            ROLE_MESSAGES.CREATED,
            'success'
          );

          this.loadRoleCards();
          this.showForm = false;
        },

        error: (err) => {
          
          this.toastService.show(
            ROLE_MESSAGES.CREATE_FAILED,
            'error'
          );
        }
      });
  }

  filteredRoleCards = computed(() => {

    let data = [
      ...this.roleCards()
    ];

    const filters = this.selectedFilters();

    // Location filter
    if (filters.locationIds.length > 0) {
      data = data.filter(role =>
        role.locationName != null &&
        filters.locationIds.includes(role.locationId)
      );
    }

    // Department filter
    if (filters.departmentIds.length > 0) {
      data = data.filter(role =>
        role.departmentId != null &&
        filters.departmentIds.includes(role.departmentId)
      );
    }

    // Search
    const search = this.searchText()
      .trim()
      .toLowerCase();

    if (search) {
      data = data.filter(role =>
        role.roleName?.toLowerCase().includes(search) ||
        role.locationName?.toLowerCase().includes(search) ||
        role.departmentName?.toLowerCase().includes(search)
      );
    }

    return data;
  });

  onApplyFilters(filters: {
    locationIds: number[];
    departmentIds: number[];
  }): void {

    this.selectedFilters.set({
      locationIds: [
        ...filters.locationIds
      ],
      departmentIds: [
        ...filters.departmentIds
      ]
    });
  }

  onResetFilters(): void {
    this.selectedFilters.set({
      locationIds: [],
      departmentIds: []
    });
  }

  onSearch(value: string): void {
    this.searchText.set(value);
  }

  openAddRoleForm(): void {
    this.showForm = true;
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

    if (this.isView) {
      this.showForm = false;
      return;
    }

    this.openConfirmModal(
      COMMON_MESSAGES.CONFIRM_CLOSE_FORM,
      () => {
        this.showForm = false;
      }
    );
  }
}