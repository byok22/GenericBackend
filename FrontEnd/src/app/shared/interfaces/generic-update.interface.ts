import { User } from "./UsersInterfaces/user.interface";

// generic-update.interface.ts
export interface GenericUpdate {
    id: number;
    updatedAt: string;
    user?: User;
    message: string;
  }
  