export interface GenericMenuInterface {
    item: any;
    labelText: string;
    order: number;
    type: 'dropdown' | 'multi-select' | 'calendar' | 'button' | 'textbox';
    customWidth?: number; // Propiedad opcional para definir el ancho
  }