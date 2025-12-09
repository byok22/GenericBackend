
import { ChangeStatus } from "../../../test-catalog/interface/change-status.";
import { TestStatusEnum } from "../../../test-catalog/interface/test-status.enum";
import { Employee } from "../employees/employee.interface";
import { Equipment } from "../equipments/equipment.interface";
import { GenericUpdate } from "../generic-update.interface";
import { Attachment } from "./attachment.interface";
import { Samples } from "./samples.interface";
import { Specification } from "./specification.interface";

// test.interface.ts
export interface Test {
    id: number;
    customerId?: string;
    name?: string;
    description?: string;
    start?: Date;
    end?: Date;
    specifications?: Specification[];
    equipments?: Equipment[];
    samples?: Samples;
    specialInstructions?: string;
    enginner?: Employee;
    technicians?: Employee[];
    idRequest?: number;
    profile?: Attachment;
    attachments?: Attachment[];   
    lastUpdatedMessage?: string;
    createdAt?: string;
    createdBy?: number;
    status: TestStatusEnum;
    changeStatusTest?: ChangeStatus[];
    updates?: GenericUpdate[];
    attachmentsCount?: number;
    techniciansCount?: number;
    specificationsCount?: number;
    equipmentsCount?: number;


  }
  