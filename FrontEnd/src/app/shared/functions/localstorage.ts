import { jwtDecode, JwtPayload } from "jwt-decode";
import { JWT } from "../interfaces/JWTModels";

// General local storage functions
export function SetLocalStorage(key: string, value: any) {
  localStorage.setItem(key, JSON.stringify({ value: value }));
}

export function UpdateLocalStorage(key: string, value: any) {
  const storedValue = localStorage.getItem(key) ?? "{}";
  const parsedValue = JSON.parse(storedValue);
  localStorage.setItem(key, JSON.stringify({ value: value }));
}

export function GetLocalStorage(key: string) {
  const storedValue = localStorage.getItem(key);

  if (storedValue ) {
    return storedValue
  }

  if (storedValue) {
    try {
      const parsedValue = JSON.parse(storedValue);
      return parsedValue.value;
    } catch (error) {
      console.error('Error parsing local storage value:', error);
      return null;
    }
  }

  return null;
}

export function DeleteLocalStorage(key: string) {
  localStorage.removeItem(key);
}


export function SetToken(tokenKey: string, token: string) {
  if(tokenKey=='')
    tokenKey ='token'
  SetLocalStorage(tokenKey, token);
}

export function GetToken(tokenKey: string): JWT {
  const token = GetLocalStorage(tokenKey);
  if (token) {
    try {
      // 1. Decodifica el token para obtener el payload estándar
      const decodedToken = jwtDecode<JwtPayload>(token);

      // 2. Valida la expiración
      if (decodedToken.exp && Date.now() > decodedToken.exp * 1000) {
        console.log("Token has expired");
        DeleteLocalStorage(tokenKey);
        return new JWT(); // Retorna un JWT vacío
      }

      // 3. Valida "not before"
      if (decodedToken.nbf && Date.now() < decodedToken.nbf * 1000) {
        console.log("Token is not yet valid");
        DeleteLocalStorage(tokenKey);
        return new JWT(); // Retorna un JWT vacío
      }

      // 4. --- ¡ESTE ES EL PASO CLAVE! ---
      // Mapea las propiedades del payload a tu clase JWT.
      // Necesitamos usar acceso por bracket notation [] para las claves con URLs
      
      const roleClaim = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
      const nameIdClaim = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';

      // Creamos una *nueva instancia* de JWT
      return new JWT({
        // Asignamos 'sub' o 'nameidentifier' a las propiedades que definiste
        PKUser: (decodedToken[nameIdClaim] as string) || decodedToken.sub || "",
        NTUser: decodedToken.sub || "", // <-- Aquí asignamos el '3524661'
        Role: (decodedToken[roleClaim] as string) || "", // <-- Aquí asignamos 'Administrator'
        
        // Pasamos las propiedades estándar
        nbf: decodedToken.nbf,
        exp: decodedToken.exp
      });

    } catch (error) {
      console.error('Error decoding token:', error);
      DeleteLocalStorage(tokenKey);
      return new JWT(); // Retorna un JWT vacío
    }
  }

  return new JWT(); // Retorna un JWT vacío si no hay token
}

export function DeleteToken(tokenKey: string) {
  DeleteLocalStorage(tokenKey);
}
