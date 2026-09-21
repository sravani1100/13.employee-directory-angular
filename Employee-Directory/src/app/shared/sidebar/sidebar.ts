import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SIDEBAR_FOOTER, SIDEBAR_MENU } from './sidebar.data';
import { RouterModule } from '@angular/router';
import { sidebarService } from '../services/sidebar';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule,
   RouterModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar {

  constructor(public sidebarService: sidebarService) {}

  menuItems = SIDEBAR_MENU;
  footerItem = SIDEBAR_FOOTER;

  toggleSidebar(): void {
    this.sidebarService.isCollapsed =
      !this.sidebarService.isCollapsed;
  }

  showFooterCard = true; 

  onActionClick() {
    this.showFooterCard = false; 
  }
}
