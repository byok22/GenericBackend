import { FormGroup } from "@angular/forms";
import { GenericFormFieldsInterface, GenericFormInterface } from "../generic-form.interface";
import { IBuilderGenericForm } from "./ibuilder-generic-form.interface";
import { Input } from "@angular/core";

export class GenericFormConcretBuilder<T> implements IBuilderGenericForm<T>{
    
   

    private _genericForm!: GenericFormInterface<T>;

    /**
     * Reset Configuration
     */
    Reset(): void {
        this._genericForm = {
            editAdd:'',
            tittle:'',
            customFromGroup:undefined,
             fields: []

        }
      }
      /**
       * Agrega Titulo al Form
       * @param tittle 
       * @example 'Test Prueba'
       */
    SetTitle(tittle: string): void {
       this._genericForm.tittle = tittle;
    }
    SetData(data: T){
        this._genericForm.data =data;
    }
    /**
     * Recibe un Campo Generico de Formulario y lo agrega al array que mostrara todos los campos
     * @param {GenericFormFieldsInterface} field  --Configuracion del Campo Generico
     * @example  {
        field:'calibrationDate',
        label:'Calibration Date',
        order:3,
        required:true,
        type:'date',
        validationRequired:true,
        enable:true,
        show:true,
        value:dateYYYMMDDCalibration  
      }
     */
    SetField(field: GenericFormFieldsInterface): void {
       this._genericForm.fields.push(field);
    }
    /**
     * Agrega Un Form Group
     * @param customFormGroup type FormGroup<any>
     * @example this.fb.group({
          id:[0],
          name:'',
          calibrationDate:'',
          employeeAccount:'', 
          userName:''          
        })
     */
    SetFormGroup(customFormGroup: FormGroup<any>): void {
        this._genericForm.customFromGroup=customFormGroup;
    }

    /**
     * Cuando se guarda o se agrega un nuevo registro
     * @param oneFunction - Funcion o accion que realizara 
     * @example ()=>{
          
          this.SubmitRequests();       
        }
     */
    SetSubmitFunction(oneFunction: any): void {
        this._genericForm.submitFunction= oneFunction.bind(this);
    }

    /**
     * Cuando se elimina un registro
     * @param oneFunction - Funcion o accion que realizara 
     * @example ()=>{
          
          this.DeleteRequests();       
        }
     */
    SetDeleteFunction(oneFunction: any): void {
        this._genericForm.deleteFunction = oneFunction.bind(this);
    }
    /**
     * Sirve para modificar los botones si es edit guardara si es Add permite agregar un nuevo 
     * registro
     * @param editAdd - 'Edit / Add'
     */
    SetEditAdd(editAdd: string): void {
        this._genericForm.editAdd = editAdd;
    }
    SetCustomNameButton(customName:string):void{
        this._genericForm.customButton = customName;
    }
    Generate(): GenericFormInterface<T> {
        this._genericForm.fields.sort((a, b) => a.order - b.order);
        return this._genericForm;
    }
   
    
}