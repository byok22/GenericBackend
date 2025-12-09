import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';


export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService); // Ahora ya no dará error circular
  const router = inject(Router);
  
  const token = localStorage.getItem('token');

  let request = req;

  // Adjuntar token si existe
  if (token) {
    request = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  return next(request).pipe(
    catchError((error: HttpErrorResponse) => {
      
      // Si es 401 y NO es la petición de login ni la de refresh
      if (error.status === 401 && !request.url.includes('/login') && !request.url.includes('/refresh')) {
        
        return authService.refreshToken().pipe(
          switchMap((res: any) => {
            // Guardamos nuevos tokens
            localStorage.setItem('token', res.token);
            // Opcional: si el refresh rota, guardar el nuevo refresh token
            if (res.refreshToken) {
                localStorage.setItem('refreshToken', res.refreshToken);
            }

            // Reintentamos la petición ORIGINAL con el nuevo token
            const newRequest = req.clone({
              setHeaders: { Authorization: `Bearer ${res.token}` }
            });
            return next(newRequest);
          }),
          catchError((err) => {
            // Si falla el refresh, logout forzoso
            authService.logout();
            return throwError(() => err);
          })
        );
      }

      return throwError(() => error);
    })
  );
};