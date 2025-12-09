import { Component, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProjectDataService, Project } from './services/dashboard.services';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterModule, FormsModule ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})

export class DashboardComponent {
    projects: Project[] = [];  
    filteredProjects: Project[] = [];
    searchTerm: string = '';

    constructor(private projectDataService: ProjectDataService) {}
  
    ngOnInit() {
      this.projectDataService.getProjects().subscribe({
        next: (data) => {this.projects = data; this.filteredProjects = data; 
        },
        error: (err) => console.error('Error al obtener proyectos:', err)
      });
    }

    //Buscador
    filterProjects() {
    const term = this.searchTerm.toLowerCase().trim();
    if (!term ) {
      this.filteredProjects = this.projects;
      return;
    }

    this.filteredProjects = this.projects.filter(project =>
      project.projectName.toLowerCase().includes(term)
    );
  }
  
    selectProject(project: Project ) {
      localStorage.setItem('selectedProjectName', project.projectName);
      localStorage.setItem('selectedProjectId', project.pkProject.toString());
    }
}