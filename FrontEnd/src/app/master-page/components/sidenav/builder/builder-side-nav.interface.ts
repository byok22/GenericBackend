import { NavItem } from "../interfaces/nav-item.interface";
//import { ISideNav } from "./sidenav.interface";


export interface IBuilderSideNav{
    Reset(): void;
    SetExpandWidth(width: string): void;
    SetFontColor(color:string): void;
    //SetStatusApp(status:string):void;
    SetNavItem(nav: NavItem):void;
    //Generate(): ISideNav;
}