import {  Injectable } from '@angular/core';
import { enviroments } from '../../../environments/enviroments';
import { HttpClient } from '@angular/common/http';
import { GenericResponse } from '../interfaces/response/generic-response';
import { Observable, tap } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class HealthService {

  baseUrl = enviroments.baseUrl;
  

  
    constructor(private http: HttpClient
      
    ) { }
  
    health(): Observable<GenericResponse> {
      return this.http.get<GenericResponse>(`${this.baseUrl}/health`)
        .pipe(
          tap(response => {
            return response;
           
          })
        );
    }

}
