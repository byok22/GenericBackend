import { GenericMenuInterface } from '../interfaces/generic-menu-item.interface';
import { IBuilderGenericMenu } from './ibuilder-generic-menu.interface';

export class GenericMenuConcreteBuilder implements IBuilderGenericMenu {
  private _genericMenu: GenericMenuInterface[] = [];

  Reset(): void {
    this._genericMenu = [];
  }

  SetDropDown(dropDown: GenericMenuInterface): void {
    this._genericMenu.push(dropDown);
  }

  SetMultiSelectDropDown(multiselectDropDown: GenericMenuInterface): void {
    this._genericMenu.push(multiselectDropDown);
  }

  SetCalendar(calendar: GenericMenuInterface): void {
    this._genericMenu.push(calendar);
  }

  SetButton(button: GenericMenuInterface): void {
    this._genericMenu.push(button);
  }

  SetTextBox(textBox: GenericMenuInterface): void {
    this._genericMenu.push(textBox);
  }
  SetTexArea(textArea: GenericMenuInterface): void{
    this._genericMenu.push(textArea);
  }

  Generate(): GenericMenuInterface[] {
    return this._genericMenu.sort((a, b) => a.order - b.order);
  }
}
