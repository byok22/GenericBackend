import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { SelectOption } from '../../shared/interfaces/select-option.interface';
import { GenericResponse } from '../../shared/interfaces/response/generic-response';
import { enviroments } from '../../../environments/enviroments';
import { SiteDto } from '../interfaces/site-dto';


@Injectable({
  providedIn: 'root'
})
export class SiteCatalogoService {

  constructor(private http: HttpClient) { }

  baseUrl = enviroments.baseUrl;

  GetAllSites(): Observable<SiteDto[]>{
    return this.http.get<SiteDto[]>(`${this.baseUrl}/api/Sites/all`).pipe(
      catchError((err) => {
        console.log(err);
        // Datos simulados (mock) en caso de error
        const mockSites: SiteDto[] = [
          {
            siteID: 1,
            siteName: 'Main Site',
            available: true
          },
        ];
        return of(mockSites);
      })
    )
  }

  getSiteById(id: number): Observable<SiteDto> {
    return this.http.get<SiteDto>(`${this.baseUrl}/api/Sites/${id}`).pipe(
      catchError((error) => {
        console.error('Error getting Site by id:', error);
        throw error;
      })
    );
  }

  updateSite(site: SiteDto): Observable<GenericResponse> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);

    // API exposes edit as POST on /api/Sites/edit
    return this.http.post<GenericResponse>(`${this.baseUrl}/api/Sites/edit`, site, { headers }).pipe(
      catchError((error) => {
        console.error('Error updating site:', error);
        throw error;
      })
    );
  }

  createSite(site: SiteDto): Observable<GenericResponse> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);
    
    return this.http.post<GenericResponse>(`${this.baseUrl}/api/Sites/create`, site, { headers }).pipe(
      catchError((error) => {
        console.error('Error creating site:', error);
        throw error;
      })
    );
  }

  deleteSite(id: number): Observable<GenericResponse> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);
    // Controller expects a DELETE to /api/Sites/delete with a SiteDto body.
    // Use HttpClient.request to send a DELETE with a body containing the id.
    return this.http.request<GenericResponse>('delete', `${this.baseUrl}/api/Sites/delete`, { body: { siteID: id }, headers }).pipe(
      catchError((error) => {
        console.error('Error deleting site:', error);
        throw error;
      })
    );
  }
}
