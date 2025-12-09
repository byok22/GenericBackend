import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProjectDataService {
  private selectedProjectName: string | null = null;
   private selectedProjectId: number | null = null;
  
  setProjectName(name:string):void{
    this.selectedProjectName = name;
  }
  
  getProjectName(): string | null {
    return this.selectedProjectName;
  }

  getProjectId(): number | null {
    return this.selectedProjectId;
  }

  clearProjectName(): void {
    this.selectedProjectName = null;
  }

  constructor() {
    
   }
}
