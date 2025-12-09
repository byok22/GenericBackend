
import { ChangeStatus } from "../../../test-catalog/interface/change-status.";
import { TestStatusEnum } from "../../../test-catalog/interface/test-status.enum";
import { Employee } from "../employees/employee.interface";
import { Equipment } from "../equipments/equipment.interface";
import { GenericUpdate } from "../generic-update.interface";
import { User } from "../UsersInterfaces/user.interface";
import { Attachment } from "./attachment.interface";
import { Samples } from "./samples.interface";
import { Specification } from "./specification.interface";

// test.interface.ts
export interface TestDto {
    id?: number;
    customerId?: string;
    name?: string;
    description?: string;
    start?: string;
    end?: string;
    specifications?: Specification[];
    equipments?: Equipment[];
    samples?: Samples;
    specialInstructions?: string;
    enginner?: Employee;
    technicians?: Employee[];
    idRequest?: number;
    profile?: Attachment
    attachments?: Attachment[];
    lastupdated?: string;
    lastUpdatedBy?: number;
    lastUpdatedMessage?: string;
    createdAt?: string;
    createdBy?: number;
    status?: TestStatusEnum;
    changeStatusTest?: ChangeStatus[];
    updates?: GenericUpdate[];
    attachmentsCount?: number;
    techniciansCount?: number;
    specificationsCount?: number;
    equipmentsCount?: number;

  }
  