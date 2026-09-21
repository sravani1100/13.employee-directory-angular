import { Component, EventEmitter, Input, Output, OnInit, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Status } from '../../models/enums/status.enum';
import { LocationService } from '../services/location.service';
import { DepartmentService } from '../services/department-service';
import { LocationModel } from '../../models/location/location.model';
import { DepartmentModel } from '../../models/department/department.model';
import { FilterModel } from '../../models/filter/filter.model';

@Component({
  selector: 'app-filter-panel',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './filter-panel.html',
  styleUrl: './filter-panel.css'
})
export class FilterPanelComponent implements OnInit {

  Status = Status;

  statuses: string[] = Object.keys(Status)
    .filter(key => isNaN(Number(key)));

  statusValues: number[] = this.statuses.map(
    key => Status[key as keyof typeof Status]
  );

  locations: LocationModel[] = [];
  departments: DepartmentModel[] = [];

  selectedStatus: number[] = [];
  selectedLocation: number[] = [];
  selectedDepartment: number[] = [];

  openDropdown: 'status' | 'location' | 'department' | null = null;

  @Input()
  showStatus = true;

  @Output()
  applyFilters = new EventEmitter<FilterModel>();

  @Output()
  resetFilters = new EventEmitter<void>();

  constructor(
    private locationService: LocationService,
    private departmentService: DepartmentService
  ) { }

  ngOnInit() {
    this.loadLocations();
    this.loadDepartments();
  }

  loadLocations() {
    this.locationService.getLocations()
      .subscribe({
        next: (res) => {
          this.locations = res;
        },
        error: (err) => {
          console.log(err);
        }
      });
  }

  loadDepartments() {
    this.departmentService.getAllDepartments()
      .subscribe({
        next: (res) => {
          this.departments = res;
        },
        error: (err) => {
          console.log(err);
        }
      });
  }

  toggleDropdown(type: 'status' | 'location' | 'department') {
    this.openDropdown = this.openDropdown === type ? null : type;
  }

  toggleStatus(id: number) {
    this.toggleValue(this.selectedStatus, id);
    this.checkIfFiltersCleared();
  }

  toggleLocation(id: number) {
    this.toggleValue(this.selectedLocation, id);
    this.checkIfFiltersCleared();
  }

  toggleDepartment(id: number) {
    this.toggleValue(this.selectedDepartment, id);
    this.checkIfFiltersCleared();
  }

  private toggleValue(array: number[], value: number) {
    const index = array.indexOf(value);

    if (index > -1) {
      array.splice(index, 1);
    } else {
      array.push(value);
    }
  }

  getSelectedCount(type: 'status' | 'location' | 'department'): number {
    switch (type) {
      case 'status':
        return this.selectedStatus.length;

      case 'location':
        return this.selectedLocation.length;

      case 'department':
        return this.selectedDepartment.length;
    }
  }

  hasSelection(): boolean {

    if (this.showStatus) {
      return (
        this.selectedStatus.length > 0 ||
        this.selectedLocation.length > 0 ||
        this.selectedDepartment.length > 0
      );
    }

    return (
      this.selectedLocation.length > 0 ||
      this.selectedDepartment.length > 0
    );
  }

  onApply() {

    this.applyFilters.emit({

      status: this.showStatus
        ? [...this.selectedStatus]
        : [],

      locationIds: [...this.selectedLocation],
      departmentIds: [...this.selectedDepartment]

    });

    this.openDropdown = null;
  }

  onReset() {

    if (this.showStatus) {
      this.selectedStatus = [];
    }

    this.selectedLocation = [];
    this.selectedDepartment = [];

    this.resetFilters.emit();
    this.openDropdown = null;
  }

  @HostListener('document:click', ['$event'])
  closeDropdown(event: MouseEvent): void {
    const target = event.target;

    if (!(target instanceof Element) || !target.closest('.content-filters')) {
      this.openDropdown = null;
    }
  }

  private checkIfFiltersCleared() {

    const noFiltersSelected = this.showStatus
      ? this.selectedStatus.length === 0 &&
      this.selectedLocation.length === 0 &&
      this.selectedDepartment.length === 0
      : this.selectedLocation.length === 0 &&
      this.selectedDepartment.length === 0;

    if (noFiltersSelected) {
      this.resetFilters.emit();
    }
  }
}