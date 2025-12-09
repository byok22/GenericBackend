import { Injectable } from '@angular/core';
import { ExportToCsv } from 'export-to-csv';


@Injectable({
  providedIn: 'root'
})
export class ExportToFormatService {

  constructor() { }
  exportToCSV(objectoCSV:any):void{
    const options = {
      fieldSeparator: ',',
      quoteStrings: '"',
      decimalSeparator: '.',
      showLabels: true,
      showTitle: false,
      useTextFile: false,
      useBom: true,
      useKeysAsHeaders: true,
    };
    const csvExporter = new ExportToCsv(options);
    csvExporter.generateCsv(objectoCSV);
  }

}
