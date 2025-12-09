import { FontAwesomeIcons } from "../enums/font-aswesome-icons.enum";

export interface NavItem {
    name: string;
    icon?: FontAwesomeIcons;
    href: string;
    childrens?: NavItem[];
    expanded?: boolean;
    external?: boolean;

  }

  