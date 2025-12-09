import { inject, Injectable } from '@angular/core';
import { UserDto } from '../interfaces/UsersInterfaces/user-dto';
// IMPORTANTE: Importar HttpBackend para saltarse el interceptor en la llamada de refresh
import { HttpClient, HttpBackend } from '@angular/common/http'; 
import { catchError, map, Observable, of, tap } from 'rxjs';
import { enviroments } from '../../../environments/enviroments';
import { LdapLoginResponseDto } from '../interfaces/ldap-login.interface';
import { Router } from '@angular/router';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = enviroments.baseUrl;
  user: UserDto | undefined;
  
  private router = inject(Router);
  private http: HttpClient; // Cliente normal
  private httpBackendClient: HttpClient; // Cliente SIN interceptores

  constructor(
    private handler: HttpBackend, // 1. Inyectamos el backend handler
    private httpInternal: HttpClient ,// Inyección normal (opcional si usas handler),
     private tokenService: TokenService
  ) { 
    this.http = new HttpClient(handler); // Inicializamos un cliente limpio
    this.httpBackendClient = new HttpClient(handler);
  }

  // --- LOGIN ---
  login(data: UserDto): Observable<LdapLoginResponseDto> {
    // Aquí sí usamos this.httpInternal o un cliente con interceptores si fuera necesario, 
    // pero para login suele ser una llamada limpia.
    return this.httpBackendClient.post<LdapLoginResponseDto>(`${this.baseUrl}/api/Auth/login`, data)
      .pipe(
        tap(response => {
          this.user = response.user;
          console.log("Login Successfully");
          
          localStorage.setItem('user', JSON.stringify(response.user));
          localStorage.setItem('token', response.token ?? '');
          
          // 2. FALTABA ESTO: Guardar el Refresh Token
          // Asegúrate de que tu interfaz LdapLoginResponseDto tenga este campo
          if(response.refreshToken) {
            localStorage.setItem('refreshToken', response.refreshToken); 
          }
        })
      );
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return token != null && !this.isTokenExpired(token);
  }

  /*isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    if (!token) {
      return false;
    }

    // Decode the token to check its expiration
    const tokenPayload = JSON.parse(atob(token.split('.')[1]));
    return tokenPayload.exp > Date.now() / 1000;
  }
*/


  /**
   * Obtiene el rol actual del usuario desde el localStorage.
   * Asume que guardaste el usuario como JSON string con una propiedad 'Role' o 'role'.
   */
  getUserRole(): string {
    const userStr = localStorage.getItem('user');
    if (userStr) {
      const user = JSON.parse(userStr);
      // Ajusta 'Role' según cómo venga de tu backend (p.ej. 'role', 'idRole', etc)
      return user.Role || user.role || ''; 
    }
    return '';
  }
  
  getUserPermissions(): string[]{
    const permissions = JSON.parse(localStorage.getItem('permissions'));
    return permissions;
  }
  
  getLoggedUser(): UserDto {
    const user: UserDto ={
      id: 0,
      userName: '',
      employeeNumber: '',
      role: '',
      available: false
    }
    return user;
  }


  getToken(): string | null {
    return localStorage.getItem('token'); // o sessionStorage, según uses
  }

  isTokenExpired(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const exp = payload.exp * 1000;
      return Date.now() > exp;
    } catch (e) {
      return true;
    }
  }

  // ---+ REFRESH TOKEN ---
  refreshToken(): Observable<any> {
    const refreshToken = localStorage.getItem('refreshToken');
    
    // Usamos httpBackendClient para que ESTA petición NO pase por el AuthInterceptor
    // Si pasara por el interceptor, entraría en bucle infinito si falla.
    return this.httpBackendClient.post(`${this.baseUrl}/api/Auth/refresh`, { 
      RefreshToken: refreshToken 
    });
  }



  logout(): void {
    this.user = undefined;
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken'); // Limpiar también esto
    localStorage.removeItem('user');
    this.router.navigate(['/login']);
  }



 

  /*logout(){
      this.user = undefined;
      localStorage.clear();
  }*/

  getCurrentUser():Observable<UserDto>{

    const currentUser:UserDto={
      ntUser: '',
      available: false,
      id: 0,
      userName: '',
      employeeNumber: '',
      role: ''
    }
    if(!localStorage.getItem('token')||!localStorage.getItem('user') ) return of(currentUser);

   
    const userJson = localStorage.getItem('user');

    const obj: UserDto = JSON.parse(userJson?.toString()??'');
    return of(obj);


  }
  
    checkAuthentication(): Observable<boolean> {
      if (!localStorage.getItem('token') || !localStorage.getItem('user')) return of(false);

      const token = localStorage.getItem('token');
      if (this.tokenService.isTokenExpired(token ?? "")) return of(false);

      const userJson = localStorage.getItem('user');
      let obj: UserDto = JSON.parse(userJson?.toString() ?? '');

      return this.http.get<UserDto>(`${this.baseUrl}/config/by-userid/${obj.id}`)
        .pipe(
          tap(user => {
            this.user = user;
          }),
          map(user => !!user),
          catchError(err => of(false))
        );
    }

  getRole(): Observable<boolean> {

    if(!localStorage.getItem('token')||!localStorage.getItem('user') ) return of(false);

    const token = localStorage.getItem('token');
    const user = localStorage.getItem('user');

    let userObj: UserDto = JSON.parse(user?.toString()??"");

    if(userObj.role=='Master Admin'){
      return of(true);
    }
    return of(false);


  }

  // other methods...

}
