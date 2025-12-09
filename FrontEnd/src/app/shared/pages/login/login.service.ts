import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { enviroments } from '../../../../environments/enviroments';
import { User } from '../../interfaces/UsersInterfaces/user.interface';
import { LdapLoginResponseDto } from '../../interfaces/ldap-login.interface';



@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private baseURL: string = enviroments.baseUrl;

  constructor(
    private _http: HttpClient, 
   ) {
    /*this.configService.loadConfig().then(config => {
      this.apiUrl = config.apiUrl;
    }
    );*/
  }

  login(data: User): Observable<LdapLoginResponseDto> {
    return this._http.post<LdapLoginResponseDto>(this.baseURL + '/LDap/login', data);
  }
  logout(): Observable<boolean>{

    try
    {

      localStorage.removeItem('user');
      localStorage.removeItem('token');
      return  of( true);
    }catch(e){
      return of( false);

    }
  

  }
}