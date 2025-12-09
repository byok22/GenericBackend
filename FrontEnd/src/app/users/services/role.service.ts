import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { SelectOption } from '../../shared/interfaces/select-option.interface';
import { enviroments } from '../../../environments/enviroments';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  private baseUrl = enviroments.baseUrl;

  constructor(private http: HttpClient) {}

  getRolesDropdown(): Observable<SelectOption[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);

    return this.http.get<any[]>(`${this.baseUrl}/api/Role/all`).pipe(
      map((roles: any[]) => roles
        //mostrar solo roles disponibles
        .filter(role => role.available)
        .map(role => ({
          id: role.pkRole,
          text: role.roleName
        }))
      ),
      catchError((error) => {
        console.error('Error fetching roles:', error);
        return throwError(() => error);
      })
    );
  }
}
