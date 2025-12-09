import { Component, Input, Output, EventEmitter, OnInit, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { ColumnasFields } from './interfaces/columns-field-generic.interface';

@Component({
  standalone: true,
  selector: 'basic-generic-table-with-select',
  templateUrl: './basic-generic-table-with-select.component.html',
  styleUrls: ['./basic-generic-table-with-select.component.scss'],
  imports: [CommonModule, ReactiveFormsModule]
})
export class BasicGenericTableWithSelectComponent<T extends Record<string, any>> implements OnInit, OnChanges {
  @Input() data?: T[] = [];
  @Input() columns: ColumnasFields[] = [];
  @Output() selected = new EventEmitter<T>();

  form: FormGroup;
  filteredData: T[] = [];
  paginatedData: T[] = [];
  currentPage: number = 1;
  itemsPerPage: number = 10;
  totalPages: number = 1;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      searchQuery: [''],
      selectedRow: [null as T | null]
    });
  }

  ngOnInit() {
    this.form.get('searchQuery')?.valueChanges.subscribe(() => this.filterData());
  }

  ngOnChanges() {
    if (this.data) {
      this.filterData();
    }
  }

  filterData() {
    if (!this.data) return;

    // Obtener el valor del campo de búsqueda
    const searchQuery = this.form.get('searchQuery')?.value || '';
    
    // Filtrar datos basados en el campo de búsqueda
    this.filteredData = this.data.filter(item =>
      Object.values(item).some(value =>
        value.toString().toLowerCase().includes(searchQuery.toLowerCase())
      )
    );

    // Calcular el número total de páginas y la página actual
    this.totalPages = Math.ceil(this.filteredData.length / this.itemsPerPage);
    this.paginateData();
  }

  paginateData() {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    this.paginatedData = this.filteredData.slice(start, end);
  }

  selectRow(row: T | null) {
    if (row !== null) {
      this.form.get('selectedRow')?.setValue(row as any);
      this.selected.emit(row);
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.paginateData();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.paginateData();
    }
  }
}
