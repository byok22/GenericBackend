import { FormGroup, Validators } from "@angular/forms";
import { SelectOption } from "../../interfaces/select-option.interface";

export interface GenericFormInterface<T> {
  tittle: string;
  data?: T;
  editAdd: string;
  fields: GenericFormFieldsInterface[];
  customFromGroup?: FormGroup;
  customFromGroupCopy?: FormGroup; // Añadido para la copia del FormGroup
  submitFunction?: any;
  deleteFunction?: any;
  customButton?: string;
}

export interface GenericFormFieldsInterface {
  field: string;
  value?: string | number | Date | boolean | Blob;
  label: string;
  type: string;
  options?: SelectOption[];
  show?: boolean;
  enable?: boolean;
  validationRequired: boolean;
  required: boolean;
  validatorType?: Validators;
  order: number;
  funcion?: any;
  onInputChange?: any;
}