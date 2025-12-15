import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { SelectOption } from '../../shared/interfaces/select-option.interface';
import { GenericResponse } from '../../shared/interfaces/response/generic-response';
import { enviroments } from '../../../environments/enviroments';
import { BuildingDto } from '../interfaces/building-dto';


@Injectable({
  providedIn: 'root'
})
export class BuildingCatalogoService {

  constructor(private http: HttpClient) { }

  baseUrl = enviroments.baseUrl;

  GetAllBuildings(): Observable<BuildingDto[]>{
    return this.http.get<BuildingDto[]>(`${this.baseUrl}/api/Buildings/all`).pipe(
      catchError((err) => {
        console.log(err);
        // Datos simulados (mock) en caso de error
        const mockBuildings: BuildingDto[] = [
          {
            buildingID: 1,
            name: 'Main Building',
            description: 'Main office building',
            siteID: 1,
            available: true
          },
        ];
        return of(mockBuildings);
      })
    )
  }

  getBuildingById(id: number): Observable<BuildingDto> {
    return this.http.get<BuildingDto>(`${this.baseUrl}/api/Buildings/${id}`).pipe(
      catchError((error) => {
        console.error('Error getting Building by id:', error);
        throw error;
      })
    );
  }

  updateBuilding(building: BuildingDto): Observable<GenericResponse> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);
    // API exposes edit as POST on /api/Buildings/edit
    return this.http.post<GenericResponse>(`${this.baseUrl}/api/Buildings/edit`, building, { headers }).pipe(
      catchError((error) => {
        console.error('Error updating building:', error);
        throw error;
      })
    );
  }

  createBuilding(building: BuildingDto): Observable<GenericResponse> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);
    
    return this.http.post<GenericResponse>(`${this.baseUrl}/api/Buildings/create`, building, { headers }).pipe(
      catchError((error) => {
        console.error('Error creating building:', error);
        throw error;
      })
    );
  }

  deleteBuilding(id: number): Observable<GenericResponse> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);
    // Controller expects DELETE to /api/Buildings/delete with BuildingDto body
    return this.http.request<GenericResponse>('delete', `${this.baseUrl}/api/Buildings/delete`, { body: { buildingID: id }, headers }).pipe(
      catchError((error) => {
        console.error('Error deleting building:', error);
        throw error;
      })
    );
  }

  getBuildingsBySite(siteId: number): Observable<BuildingDto[]> {
    // API doesn't expose a dedicated endpoint for buildings by site; fetch all and filter client-side
    return this.GetAllBuildings().pipe(
      map(list => list.filter(b => b.siteID === siteId)),
      catchError((error) => {
        console.error('Error getting buildings by site:', error);
        throw error;
      })
    );
  }
}
