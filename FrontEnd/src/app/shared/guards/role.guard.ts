import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service'; // Ajusta la ruta a tu servicio
import { MessageService } from 'primeng/api'; // Opcional: para mostrar alertas

export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  // Si usas MessageService de PrimeNG, inyéctalo, si no, bórralo.
  // const messageService = inject(MessageService); 

  // 1. Verificar si el usuario está autenticado primero
  if (!authService.isAuthenticated()) {
    router.navigate(['/login']);
    return false;
  }

  // 2. Obtener los roles esperados desde la configuración de la ruta
  const expectedPermission = route.data['permission'] as string | undefined;

  // 3. Obtener el rol real del usuario
  const userPermissions = authService.getUserPermissions();

  // Si no hay permiso esperado configurado, permitir el acceso
  if (!expectedPermission) {
    return true;
  }

  // Verificar si el usuario tiene el permiso requerido
  if (Array.isArray(userPermissions) && userPermissions.includes(expectedPermission)) {
    return true;
  }

  // 5. Si no tiene permiso: Redirigir o mostrar error
  
  // Opción A: Redirigir al Dashboard o página de "No Autorizado"
  router.navigate(['/presentation/dashboard']);
  
  // Opción B: Mostrar un Toast (si tienes PrimeNG configurado globalmente)
  /*
  messageService.add({
    severity: 'error',
    summary: 'Acceso denegado',
    detail: 'No tienes permisos para ver esta página.'
  });
  */

  return false;
};