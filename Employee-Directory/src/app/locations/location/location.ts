import {
  Component,
  computed,
  OnInit,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';

import { Sidebar } from '../../shared/sidebar/sidebar';
import { Searchbar } from '../../shared/searchbar/searchbar';
import { LocationCardComponent } from '../location-card/location-card';
import { LocationFormComponent } from '../location-form/location-form';
import { ConfirmModalComponent } from '../../shared/confirm-modal/confirm-modal/confirm-modal';

import { LocationModel } from '../../models/location/location.model';

import { LocationService } from '../../shared/services/location.service';
import { ToastService } from '../../shared/services/toast-service';
import { UserService } from '../../shared/services/user';
import { LOCATION_MESSAGES } from '../../shared/constants/location';
import { COMMON_MESSAGES } from '../../shared/constants/common';

@Component({
  selector: 'app-location',
  standalone: true,
  imports: [
    CommonModule,
    Sidebar,
    Searchbar,
    LocationCardComponent,
    LocationFormComponent,
    ConfirmModalComponent
  ],
  templateUrl: './location.html',
  styleUrl: './location.css'
})
export class LocationComponent implements OnInit {

  locations = signal<LocationModel[]>([]);

  showForm = false;

  confirmVisible = false;
  confirmMessage = '';

  private confirmAction: (() => void) | null = null;

  isAdmin = computed(() => {
    return this.userService.user()?.role === 'Admin';
  });

  constructor(
    private locationService: LocationService,
    private userService: UserService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadLocations();
  }

  loadLocations(): void {
    this.locationService
      .getLocations()
      .subscribe({
        next: (response) => {
          this.locations.set(response);
        },
        error: (err) => {
          console.log(err);
        }
      });
  }

  openForm(): void {
    this.showForm = true;
  }

  saveLocation(location: LocationModel): void {
    this.locationService
      .addLocation(location)
      .subscribe({
        next: () => {
          this.toastService.show(
           LOCATION_MESSAGES.CREATED,
            'success'
          );

          this.showForm = false;
          this.loadLocations();
        },

        error: (err) => {

          this.toastService.show(
            LOCATION_MESSAGES.CREATE_FAILED,
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