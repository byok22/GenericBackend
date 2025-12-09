import { UserDto } from "./UsersInterfaces/user-dto";
import { User } from "./UsersInterfaces/user.interface";


export interface LdapLoginResponseDto {
    isAuthenticated: boolean;
    user?: UserDto
    token: string; // Si estás generando un token JWT, por ejemplo
    refreshToken?: string; // Agregado para el Refresh Token
    message: string;
    errorType?: string;
}
