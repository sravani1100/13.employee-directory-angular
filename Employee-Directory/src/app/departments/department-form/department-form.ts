import {
  Component,
  EventEmitter,
  OnInit,
  Output
} from '@angular/core';

import { CommonModule } from '@angular/common';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators
} from '@angular/forms';

import { DepartmentModel } from '../../models/department/department.model';
import { DepartmentService } from '../../shared/services/department-service';
import { noConsecutiveSpacesValidator } from '../../shared/validators/validators';

@Component({
  selector: 'app-department-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './department-form.html',
  styleUrl: './department-form.css'
})
export class DepartmentFormComponent implements OnInit {

  @Output()
  close = new EventEmitter();

  @Output()
  save = new EventEmitter();

  departmentForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private departmentService: DepartmentService
  ) {}

  ngOnInit(): void {
    this.departmentForm = this.fb.group({
      departmentName: [
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

  adddepartment(): void {

    if (this.departmentForm.invalid) {
      this.departmentForm.markAllAsTouched();
      return;
    }

    const departmentName =
      this.departmentForm.value.departmentName.trim();

    this.departmentService
      .getAllDepartments()
      .subscribe(departments => {

        const exists = departments.some(
          p =>
            p.departmentName.toLowerCase() ===
            departmentName.toLowerCase()
        );

        if (exists) {
          this.departmentForm
            .get('departmentName')
            ?.setErrors({
              duplicate: true
            });

          return;
        }

        const department: DepartmentModel = {
          departmentId: 0,
          departmentName: departmentName
        };

        this.save.emit(department);
      });
  }

  get departmentName() {
    return this.departmentForm.get('departmentName');
  }

  closeForm(): void {
    this.close.emit();
  }
}