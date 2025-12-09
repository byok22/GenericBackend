import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, from, of } from "rxjs";
import { enviroments } from "../../../environments/enviroments";
import { GetLocalStorage } from "../../shared/functions/localstorage";
import { Role } from "../../shared/interfaces/role.interface";

@Injectable()
export class RoleService {

  
  private baseUrl = enviroments.baseUrl;
  constructor(private _http: HttpClient) {}
  

  GetRolesLowersThatUserByFKUser(fkuser: number): Observable<any> {
    const token = GetLocalStorage("token");
    const headers = new HttpHeaders().append('Authorization', `Bearer ${token}`)
    return this._http.get<any[]>(`${this.baseUrl}/Role/GetRolesLowersThatUserByFKUser?fkUser=${fkuser}`, { headers });
  }
  GetRoles(available:number = -1){
    const token = GetLocalStorage("token");
    const headers = new HttpHeaders().append('Authorization', `Bearer ${token}`)
    return this._http.get<any[]>(`${this.baseUrl}/Role/GetRoles?available=${available}`, { headers });
  
  }


  getRoles(): Observable<Role[]> {
     const token = GetLocalStorage("token");
    const headers = new HttpHeaders().append('Authorization', `Bearer ${token}`)
    return this._http.get<any[]>(`${this.baseUrl}/api/Roles/all`, { headers });
  
  }
}
