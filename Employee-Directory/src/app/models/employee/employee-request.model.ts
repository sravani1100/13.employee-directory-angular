import { Status } from "../enums/status.enum";

// export interface EmployeeRequestDTO {
//   employeeId: number;
//   employeeNumber: string;
 
//   firstName: string;
//   lastName: string;
//   dateOfBirth?: Date | null;

//   email: string;
//   mobileNumber?: string | null;

//   joiningDate: Date;

//   status: Status;

//   managerId?: number | null;
//   managerName?: string;

//   locationName: string;
//   projectName: string;
//   roleName: string;

//   password?: string;
// }
export interface EmployeeRequestDTO {


  employeeId:number;
  
  employeeNumber:string;  
  
  firstName:string;  
  lastName:string;  
  
  dateOfBirth:string|null;  
  email:string;  
  mobileNumber:string;  
  joiningDate:string;
  status:number;
  
  managerId:number|null;
  
  locationId:number;
  locationName:string;
  
  roleId:number;
  roleName:string;

  departmentId: number|null;
  departmentName: string | null;
    
  projectId:number|null;
  projectName:string;  
  
  password:string;
  
  }