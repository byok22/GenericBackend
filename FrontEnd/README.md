# changelog 
# Version 1.0.2 - 2025-11-24


#Interceptor
  Funciona para agregar un token de autenticación a las solicitudes HTTP salientes en Angular.

Para agregar un interceptor de autenticación en Angular, sigue estos pasos:
1. Crea un archivo llamado `auth.interceptor.ts` en la carpeta `src/app/shared/interceptor/`.
2. Implementa el interceptor para agregar el token de autenticación a las solicitudes HTTP salientes. Aquí tienes un ejemplo básico:
  ```typescript
  import { HttpInterceptorFn } from '@angular/common/http';

  export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const token = localStorage.getItem('token');

    if (token) {
      const cloned = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });
      return next(cloned);
    }

    return next(req);
  };
  ```
3. Registra el interceptor en el módulo principal de la aplicación (`app.config.ts`):
```typescript

import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations'; // <-- 1. IMPORTAR
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http'; 
import { authInterceptor } from './shared/interceptor/auth.interceptor';


export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes),  provideAnimations(),  provideHttpClient(withInterceptors([authInterceptor])) ]
};



# Version 1.0.1 - 2025

# Image_Traceability

Este proyecto fue generado usando [Angular CLI](https://github.com/angular/angular-cli) versión 19.0.3.

## Requisitos previos

- Instala la versión más reciente de Node.js y npm.

## Clonar el repositorio

Descarga el repositorio desde Azure DevOps:

[Image_Traceability.Americas.GDL](https://dev.azure.com/jblprd/EMS%20OT/_git/Image_Traceability.Americas.GDL)

## Instalación de dependencias

Para instalar las dependencias del proyecto, ejecuta:

```bash
npm install --legacy-peer-deps
```

## Ejecutar el proyecto en local

Para correr el proyecto en local, ejecuta:

```bash
npm run start
```

## Agregar una nueva ruta

Para agregar una nueva ruta, edita el archivo `src/app/app.routes.ts`:

Ejemplo:

```typescript
{
  path: 'customers',
  loadComponent: () => import('./customers/page/customersPage/customers.page.component').then(a => a.CustomersPageComponent),
}
```

## Agregar una opción al menú

Para agregar una opción al menú, edita el archivo `src/app/master-page/components/sidenav/sidenav.component.ts` en el método `FillGenericMenu()`:

Ejemplo:

```typescript
{ name: 'Divisions', href: '/divisions' }
```

## Crear una nueva página o componente

1. Crea una carpeta en `src/app` con el nombre de la página, catálogo o componente.
2. Dentro de esa carpeta, crea subcarpetas llamadas `components`, `interfaces`, `page`, y `services`.

### Crear el componente de la página

En la carpeta `page`, crea un nuevo componente con el nombre de la página. Por ejemplo, para una página de empleados (`employees`), ejecuta:

```bash
ng generate component employees/page/employeesPage
```

### Crear el servicio de la página

En la carpeta `services`, crea un nuevo servicio con el nombre de la página. Por ejemplo, `employees.service.ts`, ejecuta:

```bash
ng generate service employees/services/employees
```

### Crear la interfaz de la página

En la carpeta `interfaces`, crea un archivo con el nombre de la página. Por ejemplo, `employees.interface.ts`:

```typescript
export interface Employee {
  id: number;
  name: string;
  position: string;
  // Otros campos relevantes
}
```

### Crear componentes necesarios

En la carpeta `components`, crea los componentes necesarios para la página. Por ejemplo, un componente de formulario genérico:

```bash
ng generate component employees/components/employeeForm
```

### Configurar la página

En el archivo `employees.page.component.ts`, configura la lógica de la página utilizando los componentes genéricos e implementando `GenericPageTableMenuForm<T>`:

```typescript
import { Component, OnInit } from '@angular/core';
import { GenericPageTableMenuForm } from 'path-to-generic-page-table-menu-form';
import { Employee } from '../interfaces/employees.interface';
import { EmployeesService } from '../services/employees.service';

@Component({
  selector: 'app-employees-page',
  templateUrl: './employees.page.component.html',
  styleUrls: ['./employees.page.component.scss']
})
export class EmployeesPageComponent extends GenericPageTableMenuForm<Employee> implements OnInit {
  displayMaximizable: boolean = false;
  genericForm: any; // Configuración del formulario genérico
  tableConfig: any; // Configuración de la tabla genérica
  menuItems: any[] = []; // Configuración del menú

  constructor(private employeesService: EmployeesService) {
    super();
  }

  ngOnInit(): void {
    this.initVariables();
    this.initMenu();
    this.initTable();
    this.initForm();
  }

  initVariables(): void {
    // Inicializa las variables necesarias
  }

  initMenu(): void {
    // Configura los elementos del menú
    this.menuItems = [
      { label: 'Home', icon: 'pi pi-fw pi-home', routerLink: '/' },
      { label: 'Employees', icon: 'pi pi-fw pi-users', routerLink: '/employees' }
    ];
  }

  initTable(): void {
    // Configura la tabla genérica
    this.tableConfig = {
      columns: [
        { field: 'id', header: 'ID' },
        { field: 'name', header: 'Name' },
        { field: 'position', header: 'Position' }
      ],
      data: this.employeesService.getEmployees()
    };
  }

  initForm(): void {
    // Configura el formulario genérico
    this.genericForm = {
      fields: [
        { type: 'text', name: 'name', label: 'Name' },
        { type: 'text', name: 'position', label: 'Position' }
      ]
    };
  }

  hideTable(): boolean {
    // Lógica para ocultar la tabla
    return false;
  }

  getModal(event: any): void {
    // Lógica para manejar el evento de salida de la tabla
  }

  EditAdd(): string {
    // Lógica para determinar el encabezado del diálogo
    return 'Editar/Agregar Empleado';
  }
}
```

### Configurar la plantilla HTML

En el archivo `employees.page.component.html`, utiliza los componentes genéricos:

```html
<div class="customs-card">
  <shared-generic-title [title]="'Employees'"></shared-generic-title>
  <app-generic-menu [menuItems]="menuItems"></app-generic-menu>
</div>
<p-messages [showTransitionOptions]="'500ms'" [hideTransitionOptions]="'500ms'"></p-messages>
<div class="customs-card">
  <generic-table [showDetails]="true" *ngIf="hideTable() === false" [theTable]="tableConfig" (output)="getModal($event)"></generic-table>
</div>

<p-dialog
  header="{{EditAdd()}}"
  [(visible)]="displayMaximizable"
  [modal]="true"
  [style]="{width: '30vw'}"
  [maximizable]="true"
  [draggable]="false"
  [resizable]="true"
  *ngIf="displayMaximizable"
>
  <div class="left">
    <generic-form [genericForm]="genericForm"></generic-form>
  </div>
</p-dialog>
```

## Servidor de desarrollo

Para iniciar un servidor de desarrollo local, ejecuta:

```bash
ng serve
```

Una vez que el servidor esté en funcionamiento, abre tu navegador y navega a [http://localhost:4200/](http://localhost:4200/). La aplicación se recargará automáticamente cada vez que modifiques alguno de los archivos fuente.

## Generación de código

Angular CLI incluye herramientas poderosas para la generación de código. Para generar un nuevo componente, ejecuta:

```bash
ng generate component nombre-del-componente
```

Para obtener una lista completa de los esquemas disponibles (como components, directives o pipes), ejecuta:

```bash
ng generate --help
```

## Construcción

Para construir el proyecto, ejecuta:

```bash
ng build
```

Esto compilará tu proyecto y almacenará los artefactos de construcción en el directorio `dist/`. Por defecto, la construcción de producción optimiza tu aplicación para rendimiento y velocidad.

## Ejecución de pruebas unitarias

Para ejecutar pruebas unitarias con el corredor de pruebas Karma, usa el siguiente comando:

```bash
ng test
```

## Ejecución de pruebas end-to-end

Para pruebas end-to-end (e2e), ejecuta:

```bash
ng e2e
```

Angular CLI no viene con un marco de pruebas end-to-end por defecto. Puedes elegir uno que se adapte a tus necesidades.

## Recursos adicionales

Para más información sobre el uso de Angular CLI, incluyendo referencias detalladas de comandos, visita la página de [Angular CLI Overview and Command Reference](https://angular.io/cli).

## Repositorio

El repositorio del proyecto se encuentra en [Image_Traceability.Americas.GDL](https://dev.azure.com/jblprd/EMS%20OT/_git/Image_Traceability.Americas.GDL).
