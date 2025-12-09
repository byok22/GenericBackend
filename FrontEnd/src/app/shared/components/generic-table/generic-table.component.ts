import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { PrimengModule } from '../../modules/primeng.module';
import { Table } from 'primeng/table';
import { timer } from 'rxjs';
import { GenericTableConfig } from './interfaces/generic-table-config';
import { BasicKpiComponent } from '../basic-kpi/basic-kpi.component';
import { ExportToFormatService } from '../../services/export-to-format.service.service';

@Component({
  selector: 'generic-table',
  standalone: true,
  imports: [
    CommonModule,
    PrimengModule,
    BasicKpiComponent
  ],
  templateUrl: './generic-table.component.html',
  styleUrl: './generic-table.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GenericTableComponent<T extends Object> implements OnInit {
  @Input() theTable!: GenericTableConfig<T>; 
  searchValue: string | undefined;
  loading: boolean = true;
  @ViewChild('table') table!: Table;

  //For Add
  @Input()
  showDetails?: boolean = true;
  
  //For Approval
  @Input()
  Approval?: boolean = true;

  //For Preview
  @Input()
  Preview?: boolean = true;

  //Emit the row
  @Output()
  public output = new EventEmitter<T>();

  //Emit the approve
  @Output()
  public Approve = new EventEmitter<T>();

  //Emit the prev
  @Output()
  public prev = new EventEmitter<T>();

  @Output()
  public genericButtonEvent = new EventEmitter<T>();


  //Show the row detais
  show(row: T = {} as T) {
    //this.loading = true;
    try {
      const rowObject: T = { ...row };
      this.output.emit(rowObject);
    } catch (error) {
      console.error('show: ', error);
    }
    // Agregar un retraso antes de establecer loading en false
    timer(500).subscribe(() => {
      this.loading = false;
    });
  }

  ApproveRow(row: T = {} as T) {
    //this.loading = true;
    try {
      const rowObject: T = { ...row };
      this.Approve.emit(rowObject);
    } catch (error) {
      console.error('show: ', error);
    }
    // Agregar un retraso antes de establecer loading en false
    timer(500).subscribe(() => {
      this.loading = false;
    });
  }

  
  PreviewButton(row: T = {} as T) {
    //this.loading = true;
    try {
      const rowObject: T = { ...row };
      this.prev.emit(rowObject);
    } catch (error) {
      console.error('show: ', error);
    }
    // Agregar un retraso antes de establecer loading en false
    timer(500).subscribe(() => {
      this.loading = false;
    });
  }

  //Show the row detais
  genericButtonSend(row: T = {} as T) {
    //this.loading = true;
    try {
      const rowObject: T = { ...row };
      this.genericButtonEvent.emit(rowObject);
    } catch (error) {
      console.error('show: ', error);
    }
    // Agregar un retraso antes de establecer loading en false
    timer(500).subscribe(() => {
      this.loading = false;
    });
  }

 


  constructor(
    private exportToFormatService: ExportToFormatService,
  ) {}

  ngOnInit() {
      // Puedes agregar lógica de inicialización aquí si es necesario
      this.loading = false;
  }

  clear(table: any, searchInput: any) {
    this.loading = false;
    timer(500).subscribe(() => {
      table.clear();
      searchInput.value = '';
      //this.sortField = "";
     // this.sortOrder = 0;
  
      // Obtener los datos filtrados
      //const filteredData = this.table.filteredValue as T[];
  
      // Emitir los datos filtrados
      //this.filteredData.emit(filteredData);
     // this.loading = true;
     // this.loading = false;
    });
  }
  exportToCSV(): void {
    //call to Service
    this.exportToFormatService.exportToCSV(this.theTable.data);
  }
 

}