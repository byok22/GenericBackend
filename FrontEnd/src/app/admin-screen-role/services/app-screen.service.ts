import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { AppScreen } from "../../shared/interfaces/app-screen.interface";
import { enviroments } from "../../../environments/enviroments";



@Injectable()
export class AppScreenService {

  private baseUrl = enviroments.baseUrl;
       constructor(private _http: HttpClient) {}

  GetAllAppScreensAvailable(): Observable<AppScreen[]> {
    return this._http.get<AppScreen[]>(`${this.baseUrl}/api/appScreens/all?available=1`);
  }

   GetAllAppScreensAvailable2(): Observable<AppScreen[]> {
    return this._http.get<AppScreen[]>(`${this.baseUrl}/api/appScreens/all?available=1`);
  }
}
