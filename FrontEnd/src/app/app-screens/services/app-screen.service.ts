import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { SelectOption } from '../../shared/interfaces/select-option.interface';
import { GenericResponse } from '../../shared/interfaces/response/generic-response';
import { enviroments } from '../../../environments/enviroments';
import { AppScreenDto } from '../interfaces/app-screen-dto';
import { NavItem } from '../../master-page/components/sidenav/interfaces/nav-item.interface';

@Injectable({
  providedIn: 'root'
})
export class AppScreenService {

  constructor(private _http: HttpClient) { }

  baseUrl = enviroments.baseUrl;

  
  // Método para obtener todas las pantallas (para la tabla principal)
  getAllAppScreens(status: number): Observable<AppScreenDto[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);

    return this._http.get<AppScreenDto[]>(`${this.baseUrl}/api/AppScreens/all?available=${status}`);

    

  }


   getSideMenu(): Observable<NavItem[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);

    return this._http.get<NavItem[]>(`${this.baseUrl}/api/AppScreens/get-by-ntuser`, {headers});

    

  }

  // Método para el dropdown de "Parent Screen"
  getAppScreensDropdown(): Observable<SelectOption[]> {
     const token = localStorage.getItem('token');
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);

      return this._http.get<SelectOption[]>(`${this.baseUrl}/api/AppScreens/all-dropdown`).pipe(
            map((result: any[]) => result
              //mostrar solo status disponibles
              //.filter(status => status.available)
              .map(screens => ({
                id: screens.appScreenID.toString(),
                text: screens.screen
              }))
            ),
            catchError((error) => {
              console.error('Error fetching status:', error);
              return throwError(() => error);
            })
          );
  }

  createAppScreen(screen: AppScreenDto): Observable<GenericResponse> {
    if( !screen.parentAppScreenID){
      screen.parentAppScreenID = 0;
    }
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);
   
    
    return this._http.post<GenericResponse>(`${this.baseUrl}/api/AppScreens/create`, screen, { headers }).pipe(
      catchError((error) => {
        console.error('Error creating screen:', error);
        throw error;
      })
    );
  }

  updateAppScreen(screen: AppScreenDto): Observable<GenericResponse> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);
    
  
    return this._http.post<GenericResponse>(`${this.baseUrl}/api/AppScreens/edit`, screen, { headers }).pipe(
      catchError((error) => {
        console.error('Error updating screen:', error);
        throw error;
      })
    );
  }

  getAppScreenById(id: number): Observable<AppScreenDto> {
   
    
    return this._http.get<AppScreenDto>(`${this.baseUrl}/api/AppScreens/${id}`).pipe(
      catchError((error) => {
        console.error('Error getting screen by id:', error);
        throw error;
      })
    );
  }
  
  // Reutilizado de tu UserService
  getStatus(): Observable<SelectOption[]> {
    return of([
      { id: '1', text: 'Active' },
      { id: '0', text: 'Inactive' }
    ]);
  }
}