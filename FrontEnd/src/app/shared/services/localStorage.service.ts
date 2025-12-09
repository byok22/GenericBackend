import { Injectable } from '@angular/core';

export interface UserStorage {
  id: number;
  nombre: string;
  email: string;
  role: string;
  ntUser: string;
}

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  private readonly USER_KEY = 'loggedInUser';

  constructor() { }

  // Guarda el usuario en el localStorage
  setUser(user: UserStorage): void {
    localStorage.setItem(this.USER_KEY, JSON.stringify(user));
  }

  // Obtiene el usuario logeado desde el localStorage
  getUser(): UserStorage | null {
    const userJson = localStorage.getItem(this.USER_KEY);
    return userJson ? JSON.parse(userJson) : null;
  }

  // Elimina el usuario del localStorage (logout)
  removeUser(): void {
    localStorage.removeItem(this.USER_KEY);
  }

  // Verifica si hay un usuario logeado
  isLoggedIn(): boolean {
    return this.getUser() !== null;
  }
}
