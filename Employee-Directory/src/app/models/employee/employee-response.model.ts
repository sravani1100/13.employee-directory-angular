import { Status } from "../enums/status.enum";

export interface EmployeeResponseDTO {
  employeeId: number;
  employeeNumber: string;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  roleName?: string;
  mobileNumber: string;

  departmentId?: number;
  department?: string;

  locationId?: number;
  location: string;

  joiningDate: string;
  status: Status;
  manager?: string;
  projects: string[];
}