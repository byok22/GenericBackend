import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnChanges, SimpleChanges, ViewEncapsulation } from '@angular/core';
import { PrimengModule } from '../../modules/primeng.module';
import { SelectDropdownComponent } from '../select-dropdown/select-dropdown.component';
import { MultiSelectDropdownComponent } from '../multi-select-dropdown/multi-select-dropdown.component';
import { GenericMenuInterface } from './interfaces/generic-menu-item.interface';

@Component({
  selector: 'app-generic-menu',
  standalone: true,
  imports: [
    CommonModule,
    PrimengModule,
    SelectDropdownComponent,
    MultiSelectDropdownComponent
  ],
  styleUrl: './generic-menu.component.css',
  templateUrl: './generic-menu.component.html',
  changeDetection: ChangeDetectionStrategy.Default,
  encapsulation: ViewEncapsulation.None
})
export class GenericMenuComponent implements OnChanges {
  @Input() menuItems: GenericMenuInterface[] = [];  
  
  labelEmpty: string = '&nbsp;';
  ngOnChanges(changes: SimpleChanges) {
    if (changes['menuItems']) {
      this.sortMenuItems();
    }
  }

  private sortMenuItems() {
    this.menuItems.sort((a, b) => a.order - b.order);
  }

  getElementWidth(customWidth?: number): string {
    return customWidth && customWidth >= 150 ? `${customWidth}px` : '150px';
  }
}