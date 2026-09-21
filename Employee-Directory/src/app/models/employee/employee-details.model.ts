import { Status } from "../enums/status.enum";

export interface EmployeeDetailsModel {

    employeeId: number;  
    employeeNumber: string;
  
    firstName: string;  
    lastName: string;
  
    dateOfBirth: string | null;
  
    email: string;
  
    mobileNumber: string;
  
    joiningDate: string;
  
    status: Status;  
  
    managerId: number | null;  
    managerName: string | null;  
  
    locationId: number;  
    locationName: string;  

    departmentId: number | null;
    departmentName: string | null;
  
    roleId: number;  
    roleName: string;  
  
    projectId: number | null;  
    projectName: string | null;
    profileImage?: string;
  }