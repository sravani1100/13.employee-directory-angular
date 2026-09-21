import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environments';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RoleModel } from '../../models/role/role.model';
import { RoleCardModel } from '../../models/role/role-card.model';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}


  getRoleCards(): Observable<RoleCardModel[]> {
    return this.http.get<RoleCardModel[]>(
      `${this.apiUrl}/Role/cards`
    );
  }


  addRole(role: RoleModel): Observable<RoleModel> {
    return this.http.post<RoleModel>(
      `${this.apiUrl}/Role`,
      role
    );
  }

  getAllRoles(): Observable<RoleModel[]> {
    return this.http.get<RoleModel[]>(
      `${this.apiUrl}/Role`
    );
  }

  getRolesByDepartmentAndLocation(
    departmentId: number,
    locationId: number
  ): Observable<RoleModel[]> {
  
    return this.http.get<RoleModel[]>(
      `${this.apiUrl}/Role/filter`,
      {
        params: {
          departmentId,
          locationId
        }
      }
    );
  }

  getRoleByEmployeeId(employeeId: number): Observable<RoleModel> {
    return this.http.get<RoleModel>(
      `${this.apiUrl}/Role/employee/${employeeId}`
    );
  }

  getRoleIdByName(roleName: string): Observable<number> {
    return this.http.get<number>(
      `${this.apiUrl}/Role/roleId/${encodeURIComponent(roleName)}`
    );
  }

  getRoleByName(roleName: string) {
    return this.http.get<RoleModel>(
      `${this.apiUrl}/Role/roles/by-name?roleName=${roleName}`
    );
  }
}
