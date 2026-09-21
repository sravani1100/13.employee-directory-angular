import {
  Component,
  EventEmitter,
  Input,
  Output,
  OnChanges,
  SimpleChanges
} from '@angular/core';

import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  FormsModule,
  AbstractControl,
  ValidatorFn,
  ValidationErrors
} from '@angular/forms';

import { CommonModule } from '@angular/common';
import { combineLatest, forkJoin, startWith } from 'rxjs';

import { Status } from '../../models/enums/status.enum';
import { EmployeeService } from '../../shared/services/employee.service';
import { LocationService } from '../../shared/services/location.service';
import { ProjectService } from '../../shared/services/project-service';
import { RoleService } from '../../shared/services/role-service';
import { DepartmentService } from '../../shared/services/department-service';

import { LocationModel } from '../../models/location/location.model';
import { EmployeeRequestDTO } from '../../models/employee/employee-request.model';
import { EmployeeResponseDTO } from '../../models/employee/employee-response.model';
import { ProjectModel } from '../../models/project/project.model';
import { RoleModel } from '../../models/role/role.model';
import { DepartmentModel } from '../../models/department/department.model';
import { EMPLOYEE_MESSAGES } from '../../shared/constants/employee';
import { dateValidator, mobileLengthValidator, noConsecutiveSpacesValidator, startWithValidator } from '../../shared/validators/validators';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './employee-form.html',
  styleUrl: './employee-form.css'
})
export class EmployeeFormComponent implements OnChanges {

  @Input()
  employee!: EmployeeRequestDTO;

  @Input()
  isEdit = false;

  @Input()
  isView = false;

  @Output()
  save = new EventEmitter();

  @Output()
  close = new EventEmitter();

  employeeForm!: FormGroup;

  locations: LocationModel[] = [];
  departments: DepartmentModel[] = [];
  roles: RoleModel[] = [];
  projects: ProjectModel[] = [];
  managers: EmployeeResponseDTO[] = [];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private locationService: LocationService,
    private roleService: RoleService,
    private projectService: ProjectService,
    private departmentService: DepartmentService
  ) {}

  ngOnInit(): void {
    this.createForm();
    this.loadDropdownData();

    combineLatest([
      this.employeeForm.get('locationId')!.valueChanges.pipe(
        startWith(this.employeeForm.get('locationId')!.value)
      ),
      this.employeeForm.get('departmentId')!.valueChanges.pipe(
        startWith(this.employeeForm.get('departmentId')!.value)
      )
    ]).subscribe(([locationId, departmentId]) => {
      this.loadFilteredRoles(locationId, departmentId);
    });
  }

  loadDropdownData(): void {
    forkJoin({
      locations: this.locationService.getLocations(),
      departments: this.departmentService.getAllDepartments(),
      projects: this.projectService.getProjects(),
      managers: this.employeeService.getEmployees()
    }).subscribe({
      next: (result) => {
        this.locations = result.locations;
        this.departments = result.departments;
        this.roles = [];
        this.projects = result.projects;
        this.managers = result.managers;

        this.patchEmployeeData();
      },
      error: (err) => {
        console.error(
          EMPLOYEE_MESSAGES.DROPDOWN_FAILED,
          err
        );
      }
    });
  }

  createForm(): void {
    this.employeeForm = this.fb.group(
      {
        employeeNumber: [''],

        firstName: [
          '',
          [
            Validators.required,
            Validators.maxLength(20),
            Validators.pattern(/^[A-Za-z ]+$/),
            noConsecutiveSpacesValidator()
          ]
        ],

        lastName: [
          '',
          [
            Validators.required,
            Validators.maxLength(20),
            Validators.pattern(/^[A-Za-z ]+$/),
            noConsecutiveSpacesValidator()
          ]
        ],

        email: [
          '',
          [
            Validators.required,
            Validators.pattern(/^[a-zA-Z0-9._%+-]+@gmail\.(com|in)$/)
          ]
        ],

        mobileNumber: [
          '',
          [
            Validators.required,
            startWithValidator(),
            mobileLengthValidator()
          ]
        ],

        dateOfBirth: [null],

        joiningDate: [
          '',
          Validators.required
        ],

        status: [
          Status.Active,
          Validators.required
        ],

        locationId: [
          null,
          Validators.required
        ],

        departmentId: [
          null,
          Validators.required
        ],

        roleId: [
          null,
          Validators.required
        ],

        projectId: [null],

        managerId: [null]
      },
      {
        validators: dateValidator()
      }
    );
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['employee'] && this.employee) {
      this.patchEmployeeData();
    }
  }

  patchEmployeeData(): void {
    if (!this.employee) {
      return;
    }

    if (
      this.locations.length === 0 ||
      this.departments.length === 0 ||
      this.projects.length === 0
    ) {
      return;
    }

    this.employeeForm.patchValue(
      {
        employeeNumber: this.employee.employeeNumber,
        firstName: this.employee.firstName,
        lastName: this.employee.lastName,
        email: this.employee.email,
        mobileNumber: this.employee.mobileNumber,
        dateOfBirth: this.employee.dateOfBirth,
        joiningDate: this.employee.joiningDate,
        status: this.employee.status,
        locationId: this.employee.locationId,
        departmentId: this.employee.departmentId,
        projectId: this.employee.projectId,
        managerId: this.employee.managerId
      },
      {
        emitEvent: false
      }
    );

    if (
      this.employee.departmentId &&
      this.employee.locationId
    ) {
      this.roleService
        .getRolesByDepartmentAndLocation(
          this.employee.departmentId,
          this.employee.locationId
        )
        .subscribe({
          next: (roles) => {
            this.roles = roles;

            const roleExists = roles.some(
              r => r.roleId === this.employee.roleId
            );

            this.employeeForm.patchValue(
              {
                roleId: roleExists
                  ? this.employee.roleId
                  : null
              },
              {
                emitEvent: false
              }
            );
          },
          error: () => {
            this.roles = [];

            this.employeeForm.patchValue(
              {
                roleId: null
              },
              {
                emitEvent: false
              }
            );
          }
        });
    }

    if (this.isView) {
      this.employeeForm.disable();
    } else {
      this.employeeForm.enable();

      this.employeeForm
        .get('employeeNumber')
        ?.disable();
    }
  }

  saveEmployee(): void {
    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    const email = this.employeeForm.value.email;
    const mobileNumber = this.employeeForm.value.mobileNumber;

    this.employeeService
      .getEmployees()
      .subscribe((employees: EmployeeResponseDTO[]) => {

        const emailExists = employees.some(emp =>
          emp.email.toLowerCase() === email.toLowerCase() &&
          emp.employeeNumber !== this.employee.employeeNumber
        );

        const mobileExists = employees.some(emp =>
          emp.mobileNumber === mobileNumber &&
          emp.employeeNumber !== this.employee.employeeNumber
        );

        this.employeeForm.get('email')?.setErrors(null);
        this.employeeForm.get('mobileNumber')?.setErrors(null);

        if (emailExists) {
          this.employeeForm.get('email')?.setErrors({
            emailExists: true
          });
        }

        if (mobileExists) {
          this.employeeForm.get('mobileNumber')?.setErrors({
            mobileExists: true
          });
        }

        if (emailExists || mobileExists) {
          return;
        }

        const selectedLocation = this.locations.find(
          l => l.locationId == this.employeeForm.value.locationId
        );

        const selectedRole = this.roles.find(
          r => r.roleId == this.employeeForm.value.roleId
        );

        const selectedProject = this.projects.find(
          p => p.projectId == this.employeeForm.value.projectId
        );

        const selectedDepartment = this.departments.find(
          d => d.departmentId == this.employeeForm.value.departmentId
        );

        const employee: EmployeeRequestDTO = {
          employeeId: this.employee?.employeeId ?? 0,
          employeeNumber: this.employee?.employeeNumber ?? '',
          firstName: this.employeeForm.value.firstName,
          lastName: this.employeeForm.value.lastName,
          dateOfBirth: this.employeeForm.value.dateOfBirth,
          email: this.employeeForm.value.email,
          mobileNumber: this.employeeForm.value.mobileNumber,
          joiningDate: this.employeeForm.value.joiningDate,
          status: this.employeeForm.value.status,
          managerId: this.employeeForm.value.managerId,
          locationId: this.employeeForm.value.locationId,
          roleId: this.employeeForm.value.roleId,
          projectId: this.employeeForm.value.projectId ?? null,
          departmentId: this.employeeForm.value.departmentId,
          locationName: selectedLocation?.locationName ?? '',
          roleName: selectedRole?.roleName ?? '',
          projectName: selectedProject?.projectName ?? '',
          departmentName: selectedDepartment?.departmentName ?? '',
          password: this.employee?.password ?? 'N/A'
        };

        this.save.emit(employee);
      });
  }

  loadFilteredRoles(
    locationId: number,
    departmentId: number
  ): void {

    if (!locationId || !departmentId) {
      this.roles = [];

      this.employeeForm
        .get('roleId')
        ?.setValue(null, {
          emitEvent: false
        });

      return;
    }

    this.roleService
      .getRolesByDepartmentAndLocation(
        departmentId,
        locationId
      )
      .subscribe({
        next: (roles) => {
          this.roles = roles;

          if (roles.length === 0) {
            this.employeeForm
              .get('roleId')
              ?.setValue(null, {
                emitEvent: false
              });

            return;
          }

          const currentRoleId =
            this.employeeForm.get('roleId')?.value;

          const roleExists = roles.some(
            r => r.roleId === currentRoleId
          );

          if (!roleExists) {
            this.employeeForm
              .get('roleId')
              ?.setValue(null, {
                emitEvent: false
              });
          }
        },
        error: () => {
          this.roles = [];

          this.employeeForm
            .get('roleId')
            ?.setValue(null, {
              emitEvent: false
            });
        }
      });
  }

  closeForm(): void {
    this.close.emit();
  }

  get f() {
    return this.employeeForm.controls;
  }
}