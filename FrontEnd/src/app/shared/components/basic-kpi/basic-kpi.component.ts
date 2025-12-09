import { CommonModule } from '@angular/common';
import { Component,Input, OnInit  } from '@angular/core';
import { PrimengModule } from '../../modules/primeng.module';

@Component({
  selector: 'shared-basic-kpi',
  standalone: true,
  imports: [
    CommonModule,
    PrimengModule
  ],
  templateUrl: './basic-kpi.component.html',
  styleUrls: ['./basic-kpi.component.scss']
})
export class BasicKpiComponent implements OnInit{
  
  isPbar =0;
  @Input() title: string = '';
  @Input() total: string = '';

  label:string ='';

  ngOnInit(): void {
    if(this.total&&this.total.includes("%")){

      this.total=this.total.replace("%","");
      const number1 = Number(this.total);
      if(number1>=100){
        this.label ='100'
      }else
      {
        this.label = number1.toString();
      }
      this.isPbar=1;

    }
  }


}
