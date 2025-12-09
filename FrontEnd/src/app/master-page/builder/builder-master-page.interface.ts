import { IBody, IMasterPage } from "./master-page.interface";

export interface IBuilderMasterPage{
    Reset(): void;
    setBody(body: IBody): void;  
    Generate(): IMasterPage;
}