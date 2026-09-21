import { Injectable } from '@angular/core';
import { DepartmentModel } from '../../models/department/department.model';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environments';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn:'root'
})
export class DepartmentService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient){}

  getDepartments(): Observable<DepartmentModel[]> {

    return this.http.get<DepartmentModel[]>(
      `${this.apiUrl}/Department`
    );
  
  }

  getAllDepartments(): Observable<DepartmentModel[]>{
    
    return this.http.get<DepartmentModel[]>(
      `${this.apiUrl}/Department/Departments`
    );
  }

  addDepartment(department: DepartmentModel): Observable<DepartmentModel> {

    return this.http.post<DepartmentModel>(
      `${this.apiUrl}/Department`,
      department
    );
  
  }

  deleteDepartment(departmentId: number): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/Department/${departmentId}`
    );
  
  }
}