import {
    Component,
    Input
  } from '@angular/core';
  
  import { LocationModel } from '../../models/location/location.model';
  
  @Component({
    selector: 'app-location-card',
    standalone: true,
    templateUrl: './location-card.html',
    styleUrl: './location-card.css'
  })
  export class LocationCardComponent {
  
    @Input()
    location!: LocationModel;
  
  }