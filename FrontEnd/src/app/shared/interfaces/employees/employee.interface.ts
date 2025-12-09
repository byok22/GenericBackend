import { EmployeeType } from "../../enums/employee-type.enum";

// employee.interface.ts
export interface Employee {
    id?: number;
    employeeNumber: string;
    name: string;
    employeeType: EmployeeType;
    employeeTypeDescription?: string;
  }