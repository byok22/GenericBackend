import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { SelectOption } from '../../shared/interfaces/select-option.interface';

import { EmployeeType } from '../../shared/enums/employee-type.enum';

@Injectable({
  providedIn: 'root'
})
export class DropdownsService   {

  constructor(private http: HttpClient) {
    
  }

  getEmployeeTypes(): Observable<SelectOption[]> {

    
    const selecs: SelectOption[] =
      [
        {
          id: EmployeeType.Engineer,
          text: 'Engineer'
        },
        {
          id: EmployeeType.Technician,
          text: 'Technician'
        },
        {
          id: EmployeeType.All,
          text: 'All'
        }
      ];
    return of(selecs);
  }

 

}
