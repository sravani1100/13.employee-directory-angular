import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RoleCardModel } from '../../models/role/role-card.model';

@Component({
  selector: 'app-role-card',
  standalone: true,
  imports: [CommonModule,
      RouterModule,
      ],
  templateUrl: './role-card.html',
  styleUrl: './role-card.css',
})
export class RoleCardComponent {
  @Input()
  role!: RoleCardModel;
}
