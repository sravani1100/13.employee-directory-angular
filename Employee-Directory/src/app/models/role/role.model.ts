export interface RoleModel {
  roleId: number;
  roleName: string;

  locationId: number;
  locationName: string;

  departmentId: number;
  departmentName: string;
  
  description?: string | null;
}
  