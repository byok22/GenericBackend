import { FontAwesomeIcons } from "../../master-page/components/sidenav/enums/font-aswesome-icons.enum";

export interface AppScreenDto {
  appScreenID: number;
  parentAppScreenID: number | null; // Puede ser nulo si es un menú raíz
  parentScreen: string;
  screen: string;
  url: string;
  sortOrder: number;
  icon: FontAwesomeIcons;
  userID: number;
  available: boolean;

  // Propiedades de solo lectura para la tabla (opcional, pero útil)
  parentScreenName?: string;
  userName?: string;
}