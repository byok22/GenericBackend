import { GenericMenuInterface } from '../interfaces/generic-menu-item.interface';
import { IBuilderGenericMenu } from './ibuilder-generic-menu.interface';

export class GenericBorrameBuilder {


  

  /**
   *
   */
  constructor(public menuItem:GenericMenuInterface[]) {
   
    
  }

  Reset(): void {
    this.menuItem = [];
  }

  SetDropDown(dropDown: GenericMenuInterface): void {
    this.menuItem.push(dropDown);
  }

  SetMultiSelectDropDown(multiselectDropDown: GenericMenuInterface): void {
    this.menuItem.push(multiselectDropDown);
  }

  SetCalendar(calendar: GenericMenuInterface): void {
    this.menuItem.push(calendar);
  }

  SetButton(button: GenericMenuInterface): void {
    this.menuItem.push(button);
  }

  SetTextBox(textBox: GenericMenuInterface): void {
    this.menuItem.push(textBox);
  }

  Generate(): void {
    this.menuItem.sort((a, b) => a.order - b.order);
  }
}
