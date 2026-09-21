import { Routes } from '@angular/router';
import { EmployeeComponent } from './employees/employee/employee';
import { DashboardComponent } from './dashboard/dashboard';
import { RoleComponent } from './roles/role/role';
import { AccessRightsComponent } from './access-rights/access-rights';
import { LoginComponent } from './login/login';
import { authGuard } from './guards/auth.guard-guard';
import { RolesDetailsComponent } from './roles-details/roles-details/roles-details';
import { ProjectComponent } from './projects/project/project';
import { LocationComponent } from './locations/location/location';
import { DepartmentComponent } from './departments/department/department';

export const routes: Routes = [
  {
    path: '',
    component: LoginComponent
  },

  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [
      authGuard
    ]
  },

  {
    path: 'employee',
    component: EmployeeComponent,
    canActivate: [
      authGuard
    ]
  },

  {
    path: 'employees/create',
    component: EmployeeComponent,
    canActivate: [
      authGuard
    ]
  },

  {
    path: 'role',
    component: RoleComponent
  },
  
  {
    path: 'role/role-details',
    component: RolesDetailsComponent
  },

  {
    path: 'access-rights',
    component: AccessRightsComponent,
    canActivate: [
      authGuard
    ]
  },

  {
    path: 'project',
    component: ProjectComponent
},

{
  path:'location',
  component:LocationComponent
 },

 {
  path: 'department',
  component: DepartmentComponent
 },

  {
    path: '**',
    redirectTo: 'dashboard'
  }

];
