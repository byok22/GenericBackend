import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { enviroments } from "../../../environments/enviroments";
import { Observable } from 'rxjs';
import { Attachment } from "./test-interfaces/attachment.interface";

@Injectable({
  providedIn: 'root'
})
export abstract class HttpService {

  baseURL: string = enviroments.baseUrl;

  constructor(private http: HttpClient) { }

  uploadFile(endpoint: string, file: Blob): Promise<Attachment> {
    const formData: FormData = new FormData();
    formData.append('file', file);

    return this.http.post<any>(`${this.baseURL}${endpoint}`, formData).toPromise();
  }
}
