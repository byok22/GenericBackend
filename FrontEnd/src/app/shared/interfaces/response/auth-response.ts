import { UserConfigInterface } from "../users-config.interface";

export interface AuthResponse {
    user:UserConfigInterface;
    token:string
}
