import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { SelectOption } from '../../shared/interfaces/select-option.interface';
import { GenericResponse } from '../../shared/interfaces/response/generic-response';
import { enviroments } from '../../../environments/enviroments';
import { UserDto } from '../interfaces/user-dto';


@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  baseUrl = enviroments.baseUrl;

  getUsersDropdown(): Observable<SelectOption[]> {
    return this.http.get<UserDto[]>(`${this.baseUrl}/api/Users`).pipe(
      map((users: UserDto[]) => users.map(user => ({
        id: user.id.toString(),
        text: user.userName
      }))),
      catchError(() => {
        // Datos simulados (mock) en caso de error
        const mockUsers: SelectOption[] = [
          { id: '1', text: 'User A' },
          { id: '2', text: 'User B' },
          { id: '3', text: 'User C' }
        ];
        return of(mockUsers);
      })
    );
  }

  createUser(user: UserDto): Observable<GenericResponse> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);
    const body ={...user, role: user.role.toString(), nTUser: user.ntUser }
    return this.http.post<GenericResponse>(`${this.baseUrl}/api/Users/create`, body,{headers}).pipe(
      catchError((error) => {
        console.error('Error creating user:', error);
        throw error;
      })
    );
  }

  updateUser(user: UserDto): Observable<GenericResponse> {

    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);
    const body ={...user, role: user.role.toString(), nTUser: user.ntUser}
    
    return this.http.put<GenericResponse>(`${this.baseUrl}/api/Users/update`, body, {headers}).pipe(
      catchError((error) => {
        console.error('Error updating user:', error);
        throw error;
      })
    );
  }

  getUserById(id: number): Observable<UserDto> {
    return this.http.get<UserDto>(`${this.baseUrl}/api/Users/${id}`).pipe(
      catchError((error) => {
        console.error('Error getting user by id:', error);
        throw error;
      })
    );
  }

  getUserByUuid(uuid: string): Observable<UserDto> {
    return this.http.get<UserDto>(`${this.baseUrl}/api/Users/${uuid}`).pipe(
      catchError((error) => {
        console.error('Error getting user by uuid:', error);
        throw error;
      })
    );
  }

  getAllUsers(): Observable<UserDto[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);

    return this.http.get<UserDto[]>(`${this.baseUrl}/api/Users/all`, { headers }).pipe(
      catchError((err) => {
        console.log(err);
        // Datos simulados (mock) en caso de error
        const mockUsers: UserDto[] = [
          {
            id: 1,
            userName: 'User A',
            //employeeNumber: '123',
            role: 'User A',
            available: true
          },
          {
            id: 2,
            userName: 'User B',
            //employeeNumber: '456',
            role: 'User A',
            available: true
          },
          {
            id: 3,
            userName: 'User C',
            //employeeNumber: '789',
            role: 'User A',
            available: true
          }
        ];
        return of(mockUsers);
      })
    );
  }

  deleteUser(id: number): Observable<GenericResponse> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);
    return this.http.delete<GenericResponse>(`${this.baseUrl}/api/Users/${id}`,{headers}).pipe(
      catchError((error) => {
        console.error('Error deleting user:', error);
        throw error;
      })
    );
  }

  getStatus(): Observable<SelectOption[]> {
    return of([
      {
        id: '1',
        text: 'Active',
        value: '1',
        viewValue: 'Active'
      },
      {
        id: '0',
        text: 'Inactive',
        value: '0',
        viewValue: 'Inactive'
      }
    ]);
  }

  private transformToDto(user: any): UserDto {
    return {
      id: user.id,
      userName: user.userName,
      ntUser: user.ntUser,
      //employeeNumber: user.employeeNumber,
      email: user.email,
      role: user.role,
      available: user.available
    };
  }

  private transformToDtoArray(users: any[]): UserDto[] {
    return users.map(user => this.transformToDto(user));
  }
}
