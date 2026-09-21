import { Component } from '@angular/core';
import { Sidebar } from '../shared/sidebar/sidebar';
import { Searchbar } from '../shared/searchbar/searchbar';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [Sidebar,
            Searchbar],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class DashboardComponent {}
