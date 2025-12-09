import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { enviroments } from '../../../environments/enviroments';
import { catchError, filter, map } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';


export interface Project {
  pkProject: number;
  projectName: string;
  fkStatus: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProjectDataService {
  private baseUrl = enviroments.baseUrl;
  constructor(private http: HttpClient) { }

  getProjects(): Observable<any[]> {
     return this.http.get<any[]>(`${this.baseUrl}/api/Project/all?fkStatus=3`)
  }
}
