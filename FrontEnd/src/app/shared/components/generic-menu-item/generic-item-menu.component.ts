import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnChanges, SimpleChanges, ViewEncapsulation } from '@angular/core';
import { PrimengModule } from '../../modules/primeng.module';
import { MultiSelectDropdownComponent } from '../multi-select-dropdown/multi-select-dropdown.component';
import { GenericMenuInterface } from './interfaces/generic-menu-item.interface';
import { SelectDropdownComponent2 } from '../select-dropdown-2/select-dropdown-2.component';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'generic-item-menu',
  standalone: true,
  imports: [
    CommonModule,
    PrimengModule,
    SelectDropdownComponent2,
    MultiSelectDropdownComponent,
  ],
  styleUrl: './generic-item-menu.component.css',
  templateUrl: './generic-item-menu.component.html',
  changeDetection: ChangeDetectionStrategy.Default,
  encapsulation: ViewEncapsulation.None
})
export class GenericMenuItemComponent implements OnChanges {
  @Input() item!: GenericMenuInterface;  

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['item']) {
      // Detect and handle changes to the item input
      console.log('Item changed:', this.item);
    }



  }
 }
