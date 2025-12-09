import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { CommonModule } from '@angular/common';
import { ToastModule } from 'primeng/toast';
import { PrimengModule } from '../../modules/primeng.module';
import { AppScreenService } from '../../../app-screens/services/app-screen.service';
import { AuthService } from '../../services/auth.service'; // Asegúrate de que la ruta sea correcta
import { VersionService } from '../../services/version.service'; // Asegúrate de que la ruta sea correcta

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [CookieService, MessageService, AppScreenService],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ToastModule, PrimengModule]
})
export class LoginComponent implements OnInit {
  title: string = '';
  version: string = '0.0.0';
  supportEmail: string = '';
  loginForm!: FormGroup;

  constructor(
    private router: Router,
    public formBuilder: FormBuilder,
    private loginService: AuthService,
    private cookieService: CookieService,
    private messageService: MessageService,
    private versionService: VersionService,
    private appScreenService: AppScreenService
  ) { }

  ngOnInit() {
    this.version = this.versionService.get();
    this.loginForm = this.formBuilder.group({
      ntuser: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  login() {
    if (this.loginForm.valid) {
      this.loginService.login(this.loginForm.value).subscribe({
        next: ({ isAuthenticated, user, token, refreshToken, message }) => {
          if (!isAuthenticated || !user?.ntUser) {
            this.messageService.add({
              severity: 'error',
              summary: 'Autenticación Fallida',
              detail: message || 'Usuario/Contraseña incorrectos',
              key: 'tr',
            });
            return;
          }

          // Guardar usuario y token
          localStorage.setItem('user', JSON.stringify(user));
          localStorage.setItem('token', token);

          if (refreshToken) {
            localStorage.setItem('refreshToken', refreshToken);
        }

          // Obtener Menú y Permisos
          this.appScreenService.getSideMenu().subscribe({
            next: (data: any[]) => {
              // 1. Guardar el menú completo para el componente de navegación
              localStorage.setItem('sideMenu', JSON.stringify(data));

              // 2. Generar lista plana de permisos basada en el menú
              const allMenuItems = this.flattenMenu(data);
              
              // IMPORTANTE: Aquí extraemos el identificador único de la pantalla.
              // Si tu backend devuelve una propiedad 'screenCode' o 'permission', úsala.
              // Si no, estamos usando 'name' o 'href' como fallback.
              const permissions = allMenuItems.map(item => {
                  // Retorna el código que configuraste en tus rutas (data: { permission: '...' })
                  // Ejemplo: return item.screenCode || item.name; 
                  return item.name; // Ajusta esto a tu propiedad real
              });

              localStorage.setItem('permissions', JSON.stringify(permissions));

              this.router.navigate(['/presentation/dashboard']);
            },
            error: (error) => {
              console.error('Error obteniendo menú', error);
              // En caso de error al obtener menú, decidir si dejar pasar o regresar
              this.router.navigate(['/login']);
            },
          });
        },
        error: (err) => {
          console.log(err);
          const errorMessage =
            err.error?.message || 'Ocurrió un error inesperado durante el login.';
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: errorMessage,
            key: 'tr',
          });
        },
      });
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'Campos requeridos',
        detail: 'Por favor ingresa usuario y contraseña',
        key: 'tr',
      });
    }
  }

  /**
   * Función recursiva para convertir el árbol de menú en una lista plana.
   * Esto permite extraer todos los permisos anidados, sin importar la profundidad.
   */
  flattenMenu(menuItems: any[]): any[] {
    let flatList: any[] = [];

    menuItems.forEach(item => {
      // Agregamos el ítem actual a la lista plana
      flatList.push(item);

      // Si el ítem tiene hijos (submenús), llamamos a la función recursivamente
      if (item.childrens && item.childrens.length > 0) {
        const childrenFlat = this.flattenMenu(item.childrens);
        flatList = flatList.concat(childrenFlat);
      }
    });

    return flatList;
  }

  loginGuest() {
    this.cookieService.set('ntUser', 'Guest');
    this.cookieService.set('role', 'Guest');
    // this.router.navigate(['/presentation']);
  }
}