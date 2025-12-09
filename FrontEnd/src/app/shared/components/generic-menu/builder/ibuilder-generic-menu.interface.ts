import { GenericMenuInterface } from "../interfaces/generic-menu-item.interface";

export interface IBuilderGenericMenu{
    Reset(): void;
    /**
     * 
     * @param dropDown 
     * @example
     *  statusItem: GenericMenuInterface = {
            item: {
            selectedOption: this.selectedStatus,
            options: this.statusDD, onChange: (event: string) => {
                this.selectedStatus = event;
                this.hideTable.set(true);
                console.log('Selected option changed:', event);

            }
            },
            labelText: 'Status',
            order: 1,
            type: 'dropdown'
        }
     */
    SetDropDown(dropDown: GenericMenuInterface): void;
    SetMultiSelectDropDown(multiselectDropDown: GenericMenuInterface): void;
    SetCalendar(calendar: GenericMenuInterface): void;
    SetButton(button: GenericMenuInterface): void;
    SetTextBox(textBox: GenericMenuInterface): void;
    Generate():void;
}