import {
  Component,
  EventEmitter,
  Input,
  Output
} from '@angular/core';

import { CommonModule } from '@angular/common';

import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators
} from '@angular/forms';

import { RoleModel } from '../../models/role/role.model';
import { DepartmentModel } from '../../models/department/department.model';
import { LocationModel } from '../../models/location/location.model';

import { RoleService } from '../../shared/services/role-service';
import { noConsecutiveSpacesValidator } from '../../shared/validators/validators';

@Component({
  selector: 'app-role-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './role-form.html',
  styleUrl: './role-form.css'
})
export class RoleFormComponent {

  @Input()
  departments: DepartmentModel[] = [];

  @Input()
  locations: LocationModel[] = [];

  @Input()
  isView = false;

  @Output()
  save = new EventEmitter();

  @Output()
  close = new EventEmitter();

  roleForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private roleService: RoleService
  ) {}

  ngOnInit(): void {

    this.roleForm = this.fb.group({
      locationId: [
        null,
        Validators.required
      ],

      departmentId: [
        null,
        Validators.required
      ],

      roleName: [
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

  submitForm(): void {

    if (this.roleForm.invalid) {
      this.roleForm.markAllAsTouched();
      return;
    }

    const roleName =
      this.roleForm.value.roleName.trim();

    this.roleService
      .getAllRoles()
      .subscribe(roles => {

        const exists = roles.some(
          r =>
            r.roleName.toLowerCase() ===
            roleName.toLowerCase()
        );

        if (exists) {

          this.roleForm
            .get('roleName')
            ?.setErrors({
              duplicate: true
            });

          return;
        }

        const formValue = this.roleForm.value;

        const selectedLocation =
          this.locations.find(
            l =>
              l.locationId === formValue.locationId
          );

        const selectedDepartment =
          this.departments.find(
            d =>
              d.departmentId === formValue.departmentId
          );

        const role: RoleModel = {
          roleId: 0,
          roleName: roleName,
          locationId: formValue.locationId,
          departmentId: formValue.departmentId,
          locationName: selectedLocation?.locationName ?? '',
          departmentName: selectedDepartment?.departmentName ?? '',
          description: null
        };

        this.save.emit(role);
      });
  }

  closeForm(): void {
    this.roleForm.reset();
    this.close.emit();
  }

  get locationId() {
    return this.roleForm.get('locationId');
  }

  get departmentId() {
    return this.roleForm.get('departmentId');
  }

  get roleName() {
    return this.roleForm.get('roleName');
  }
}