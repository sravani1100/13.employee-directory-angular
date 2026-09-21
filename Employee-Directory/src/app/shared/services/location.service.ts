import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environments';
import { LocationModel } from '../../models/location/location.model';

@Injectable({
  providedIn: 'root'
})
export class LocationService {

  private apiUrl = environment.apiUrl;

  constructor(
    private http: HttpClient
  ) {}

  getLocations(): Observable<LocationModel[]> {

    return this.http.get<LocationModel[]>(
      `${this.apiUrl}/Location`
    );

  }

  addLocation(location: LocationModel): Observable<LocationModel> {

    return this.http.post<LocationModel>(
      `${this.apiUrl}/Location`,
      location
    );

  }

  updateLocation(location: LocationModel): Observable<LocationModel> {

    return this.http.put<LocationModel>(
      `${this.apiUrl}/Location/${location.locationId}`,
      location
    );
  }

  deleteLocation(id: number): Observable<void>{

    return this.http.delete<void>(
      `${this.apiUrl}/Location/${id}`
    );
  }
}