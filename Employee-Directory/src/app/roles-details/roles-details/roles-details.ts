import { Component, OnInit, signal } from '@angular/core';
import { Sidebar } from '../../shared/sidebar/sidebar';
import { Searchbar } from '../../shared/searchbar/searchbar';
import { EmployeeResponseDTO } from '../../models/employee/employee-response.model';
import { ActivatedRoute } from '@angular/router';
import { EmployeeService } from '../../shared/services/employee.service';
import { EmployeeCardComponent } from '../employee-card/employee-card';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-roles-details',
  imports: [CommonModule,Sidebar,
      Searchbar,
      EmployeeCardComponent],
  templateUrl: './roles-details.html',
  styleUrl: './roles-details.css',
})
export class RolesDetailsComponent implements OnInit{
  roleName = '';
  employees = signal<EmployeeResponseDTO[]>([]);

  constructor(
    private route: ActivatedRoute,
    private employeeService: EmployeeService
  ) {}

  ngOnInit(): void {

    this.route.queryParams.subscribe(params => {
  
      this.roleName = params['role'];
  
      if (this.roleName) {
        this.loadEmployees();
      }
  
    });
  
  }

  loadEmployees(): void {

    this.employeeService.getEmployees()
      .subscribe({
  
        next: (response: EmployeeResponseDTO[]) => {
  
          const filtered = response.filter(
            employee =>
              employee.roleName?.trim().toLowerCase() ===
              this.roleName.trim().toLowerCase()
          );
          
          this.employees.set(filtered);
  
        },
  
        error: err => console.error(err)
  
      });
  
  }
}
