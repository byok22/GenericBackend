import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AppScreenRole } from '../interfaces/app-screen-role.interface';
import { SyncPermissionsRequest } from '../interfaces/sync-permissions-request.interface';
import { GenericResponse } from '../../shared/interfaces/response/generic-response';
import { enviroments } from '../../../environments/enviroments';
import { GetLocalStorage } from '../../shared/functions/localstorage';


@Injectable({
  providedIn: 'root'
})
export class PermissionsService {

  
  private baseUrl = enviroments.baseUrl;
      constructor(private http: HttpClient) {}

  getPermissionsByRole(roleId: number): Observable<AppScreenRole[]> {
    return this.http.get<AppScreenRole[]>(`${this.baseUrl}/api/permissions/by-role/${roleId}`);
  }

  getPermissions():Observable<AppScreenRole[]>{


    const token = GetLocalStorage("token");
    const headers = new HttpHeaders().append('Authorization', `Bearer ${token}`)
    return this.http.get<AppScreenRole[]>(`${this.baseUrl}/api/permissions`, { headers });

     

  }

  syncPermissions(payload: SyncPermissionsRequest): Observable<GenericResponse> {
    return this.http.post<GenericResponse>(`${this.baseUrl}/api/permissions/sync`, payload);
  }
}
