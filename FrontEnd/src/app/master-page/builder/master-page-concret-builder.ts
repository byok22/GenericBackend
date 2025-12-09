import { IBuilderMasterPage } from "./builder-master-page.interface";
import { IBody, IMasterPage } from "./master-page.interface";

export class MasterPageConcretBuilder implements IBuilderMasterPage{
    private _masterPage: IMasterPage ={
        body: {
            collapsed:false,
            screenWidth:0
        }
    };
    /**
     *
     */
    constructor() {
        
        
    }
    Reset(): void {
        this._masterPage = {
            body:{
                collapsed:false,
                screenWidth:0
            }
        };
    }
    setBody(body: IBody): void {
       this._masterPage.body = body;
    }
    Generate(): IMasterPage {
        return this._masterPage;
    }

}