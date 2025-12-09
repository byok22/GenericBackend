import { Injectable } from "@angular/core";
import { VERSION } from "../../../configs/versions";

@Injectable({
  providedIn: 'root'
})
export class VersionService {
    private version:string = VERSION;   
    
    get():string{
        return this.version;
    }
}