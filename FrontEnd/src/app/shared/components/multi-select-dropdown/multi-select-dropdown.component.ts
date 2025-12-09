import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SelectOption } from '../../interfaces/select-option.interface';
import { PrimengModule } from '../../modules/primeng.module';
import { SelectDropdownComponent } from '../select-dropdown/select-dropdown.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'multi-select-dropdown',
  standalone: true,
  imports: [ 
    PrimengModule,
    CommonModule
  ],
  templateUrl: './multi-select-dropdown.component.html',
  styleUrls: ['./multi-select-dropdown.component.css']
})
export class MultiSelectDropdownComponent implements OnInit {
  ngOnInit(): void {
    this.selectedOption = [];
  }

  @Input() label: string = '';
  @Input() options: SelectOption[] = [];
  @Output() onChange = new EventEmitter<SelectOption[]>();

  

  selectId: string = `select-${Math.random().toString(36).substring(2, 9)}`;
  selectedOption: SelectOption[] =[];

  text:string = "text"

  selectedOptionsList: SelectOption[] = [];

  onChangeSelection(): void {
   
    this.selectedOptionsList = [...this.selectedOption];
    this.onChange.emit(this.selectedOptionsList);
  }

  onModelChange(event: any): void {
   
    if (event === null) {
      this.selectedOptionsList = [];
    }
  }

}
