/**
 * Interfaces que Sirve para cualquier elemento que estara en el menu, por lo regular es un elemento del array del menu
 * @type 'dropdown' | 'multi-select' | 'calendar' | 'button' | 'textbox'
 * @example 
 *  processItemMenu: GenericMenuInterface ={
        item:{
            selectedOption: this.selectedProcess,
            options: this.processDD, onChange: (event: string) => {
              this.selectedProcess = event;
              this.hideTable.set(true);
              console.log('Selected option changed:', event);
    
            }
          },
          labelText: 'Process',
          order: 1,
          type: 'dropdown'
          
    
        }
 * @example2 
        
 */
export interface GenericMenuInterface{
    item: any;
    labelText: string;
    order:number;
    type: 'dropdown' | 'multi-select' | 'calendar' | 'button' | 'textbox';
    customWidth?: number;
}