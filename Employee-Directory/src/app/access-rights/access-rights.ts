import { Component } from '@angular/core';
import { Sidebar } from '../shared/sidebar/sidebar';
import { Searchbar } from '../shared/searchbar/searchbar';

@Component({
  selector: 'app-access-rights',
  standalone: true,
  imports: [Sidebar,
      Searchbar],
  templateUrl: './access-rights.html',
  styleUrl: './access-rights.css',
})
export class AccessRightsComponent {}
