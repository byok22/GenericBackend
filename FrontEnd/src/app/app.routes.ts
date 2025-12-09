import { Routes } from '@angular/router';
import { authGuard } from './shared/guards/auth.guard';
import { roleGuard } from './shared/guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'presentation/dashboard',
    pathMatch: 'full'
  },
  {
    path: 'presentation',
    loadComponent: () =>
      import('./master-page/master-page.component').then(
        (m) => m.MasterPageComponent
      ),
    // 1. authGuard protege toda el área de presentación (requiere estar logueado)
    canActivate: [authGuard],
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./dashboard/dashboard.component').then((m) => m.DashboardComponent)
        // Generalmente "Home" es accesible para todos, así que no ponemos roleGuard.
        // Si quisieras restringirlo también, usarías data: { permission: 'Home' }
      },
      
    
      // --- CATÁLOGOS (Hijos de "Catalogs" en el JSON) ---
   
      {
        path: 'users-catalog',
        loadComponent: () => import('./users/page/users.page.component').then(
            (m) => m.UsersPageComponent
          ),
        canActivate: [roleGuard],
        data: { permission: 'Users' } // Coincide con el JSON "name": "Users"
      },
      {
        path: 'role-catalog',
        loadComponent: () => import('./roles/page/role.page.component').then(
            (m) => m.RolesPageComponent
          ),
        canActivate: [roleGuard],
        data: { permission: 'Roles' } // Coincide con el JSON "name": "Roles"
      },    
      // --- ADMIN (Hijos de "Admin" en el JSON) ---
      {
        path: 'app-screens',
        loadComponent: () => import('./app-screens/app-screens.page').then(
            (m) => m.AppScreensPageComponent
          ),
        canActivate: [roleGuard],
        data: { permission: 'Screens' } // Coincide con el JSON "name": "Screens"
      },
      {
        path: 'app-permissions',
        loadComponent: () => import('./admin-screen-role/admin-screen-role.component').then(
            (m) => m.AdminScreenRoleComponent
          ),
        canActivate: [roleGuard],
        data: { permission: 'Permissions' } // Coincide con el JSON "name": "Permissions"
      },
      
      // Rutas comentadas o futuras
      // {
      //   path:'app-approve',
      //   loadComponent: () => import('./admin-screen-role/pending-aprove.component').then(
      //       (m) => m.AdminScreenRoleComponent
      //     )
      // },
    ]
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./shared/pages/login/login.component').then(
        (m) => m.LoginComponent
      )
  },
  {
    path: '404',
    loadComponent: () => import('./page-not-found/page-not-found.component').then(m => m.PageNotFoundComponent)
  },
  {
    path: '**',
    loadComponent: () => import('./page-not-found/page-not-found.component').then(m => m.PageNotFoundComponent)
  }
];