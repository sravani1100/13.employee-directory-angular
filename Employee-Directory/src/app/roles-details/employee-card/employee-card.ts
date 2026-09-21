import { Component, Input } from '@angular/core';
import { EmployeeResponseDTO } from '../../models/employee/employee-response.model';

@Component({
  selector: 'app-employee-card',
  standalone: true,
  imports: [],
  templateUrl: './employee-card.html',
  styleUrl: './employee-card.css',
})
export class EmployeeCardComponent {
  @Input() employee!: EmployeeResponseDTO;
}
