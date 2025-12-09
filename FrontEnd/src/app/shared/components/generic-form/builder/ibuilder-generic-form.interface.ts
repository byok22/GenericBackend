import { FormGroup } from "@angular/forms";
import { GenericFormFieldsInterface, GenericFormInterface } from "../generic-form.interface";

export interface IBuilderGenericForm<T>{
    Reset():void;
    SetTitle(tittle: string):void;
    SetField(field:GenericFormFieldsInterface): void;
    SetFormGroup(customFormGroup: FormGroup): void;
    SetSubmitFunction(oneFunction: any): void;
    SetDeleteFunction(oneFunction: any): void;
    SetEditAdd(editAdd:string):void;
    Generate(): GenericFormInterface<T>;
}