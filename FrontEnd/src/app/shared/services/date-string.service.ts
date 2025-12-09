import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class DateStringService {

  constructor() { }
    
    formatDateToYYYYMMDD(date: Date|undefined): string {
        if(!date){
          return '';
        }
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
      
      }

      formatToISOString(dateString: string): string {
        const date = new Date(dateString);
        if (isNaN(date.getTime())) {
          throw new Error('Invalid date string');
        }
        return date.toISOString();
    }

      
  

}
