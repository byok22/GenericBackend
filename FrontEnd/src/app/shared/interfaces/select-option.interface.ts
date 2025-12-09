export interface SelectOption {
    id: string;
    text: string;
  }

export interface DropDownInterface {
  selectedOption: string;
  options:SelectOption[];
}