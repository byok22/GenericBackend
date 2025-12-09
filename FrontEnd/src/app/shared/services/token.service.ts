import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';


@Injectable({
  providedIn: 'root'
})
export class TokenService {

  getTokenExpirationDate(token: string): Date | null {
    const decoded: any = jwtDecode(token);

    if (decoded.exp === undefined) return null;

    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);

    return date;
  }

  isTokenExpired(token: string, offsetSeconds: number = 0): boolean {
    const expirationDate = this.getTokenExpirationDate(token);

    if (expirationDate === null) return false;

    // Token está expirado si la fecha de expiración es anterior a la fecha actual
    return expirationDate.valueOf() < new Date().valueOf() + offsetSeconds * 1000;
  }
}