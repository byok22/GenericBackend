export interface Role{
  roleID:number;
  roleName:string;
  available:boolean;
  fkLastUpdater?:number;  
}