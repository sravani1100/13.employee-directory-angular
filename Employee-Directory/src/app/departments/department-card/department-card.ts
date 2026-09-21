import { Component, Input } from '@angular/core';
import { DepartmentModel } from '../../models/department/department.model';

@Component({
  selector: 'app-department-card',
  imports: [],
  templateUrl: './department-card.html',
  styleUrl: './department-card.css',
})
export class DepartmentCardComponent {
  @Input()
  department!: DepartmentModel;
}
