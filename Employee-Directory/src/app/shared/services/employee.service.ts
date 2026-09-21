import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environments';
import { EmployeeResponseDTO } from '../../models/employee/employee-response.model';
import { EmployeeRequestDTO } from '../../models/employee/employee-request.model';
import { EmployeeDetailsModel } from '../../models/employee/employee-details.model';


@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  private apiUrl = environment.apiUrl;

  constructor(
    private http: HttpClient
  ) { }

  getEmployees() {
    return this.http.get<EmployeeResponseDTO[]>(
      `${this.apiUrl}/Employee`
    );
  }

  getEmployeeById(id: number) {
    return this.http.get<EmployeeDetailsModel>(
      `${this.apiUrl}/Employee/id/${id}`
    );
  }

  createEmployee(employee: EmployeeRequestDTO) {
    return this.http.post<EmployeeRequestDTO>(
      `${this.apiUrl}/Employee`,
      employee
    );
  }

  updateEmployee(employeeNumber: string, employee: EmployeeRequestDTO) {
    return this.http.put(
      `${this.apiUrl}/Employee/${employeeNumber}`,
      employee
    );
  }

  deleteEmployee(employeeNumber: string) {
    return this.http.delete(
      `${this.apiUrl}/Employee/${employeeNumber}`
    );
  }
}