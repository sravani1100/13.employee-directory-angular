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

import { LocationModel } from '../../models/location/location.model';

import { LocationService } from '../../shared/services/location.service';
import { noConsecutiveSpacesValidator } from '../../shared/validators/validators';

@Component({
  selector: 'app-location-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './location-form.html',
  styleUrl: './location-form.css'
})
export class LocationFormComponent implements OnInit {

  @Output()
  save = new EventEmitter();

  @Output()
  close = new EventEmitter();

  locationForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private locationService: LocationService
  ) {}

  ngOnInit(): void {
    this.locationForm = this.fb.group({
      locationName: [
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

  addLocation(): void {

    if (this.locationForm.invalid) {
      this.locationForm.markAllAsTouched();
      return;
    }

    const locationName =
      this.locationForm.value.locationName.trim();

    this.locationService
      .getLocations()
      .subscribe(locations => {

        const exists = locations.some(
          l =>
            l.locationName.toLowerCase() ===
            locationName.toLowerCase()
        );

        if (exists) {
          this.locationForm
            .get('locationName')
            ?.setErrors({
              duplicate: true
            });

          return;
        }

        const location: LocationModel = {
          locationId: 0,
          locationName: locationName
        };

        this.save.emit(location);
      });
  }

  get locationName() {
    return this.locationForm.get('locationName');
  }

  closeForm(): void {
    this.close.emit();
  }
}