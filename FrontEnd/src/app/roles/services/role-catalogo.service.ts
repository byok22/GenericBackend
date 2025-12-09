import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { SelectOption } from '../../shared/interfaces/select-option.interface';
import { GenericResponse } from '../../shared/interfaces/response/generic-response';
import { enviroments } from '../../../environments/enviroments';
import { RoleDto } from '../interfaces/role-dto';


@Injectable({
  providedIn: 'root'
})
export class RoleCatalogoService {


  constructor(private http: HttpClient) { }

  baseUrl = enviroments.baseUrl;

  GetAllRole(): Observable<RoleDto[]>{

    return this.http.get<RoleDto[]>(`${this.baseUrl}/api/Role/all`).pipe(
        catchError((err) => {
        console.log(err);
        // Datos simulados (mock) en caso de error
        const mockRole: RoleDto[] = [
          {
            pkRole: 1,
            roleName: 'Admin',
            available: true
          },
        
        ];
        return of(mockRole);
      })

    )
  }

  getRoleById(id: number): Observable<RoleDto> {
    return this.http.get<RoleDto>(`${this.baseUrl}/api/Role/${id}`).pipe(
      catchError((error) => {
        console.error('Error getting Role by id:', error);
        throw error;
      })
    );
  }

  updateRole(role: RoleDto): Observable<GenericResponse> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);

    return this.http.put<GenericResponse>(`${this.baseUrl}/api/role/update`, role).pipe(
      catchError((error) => {
        console.error('Error updating role:', error);
        throw error;
      })
    );
  }
  createRole(role: RoleDto): Observable<GenericResponse> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);
    
    return this.http.post<GenericResponse>(`${this.baseUrl}/api/Role/create`, role).pipe(
      catchError((error) => {
        console.error('Error creating role:', error);
        throw error;
      })
    );
  }




}

