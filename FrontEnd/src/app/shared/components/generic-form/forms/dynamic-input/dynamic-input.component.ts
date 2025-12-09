import { Component, Input, OnInit, AfterViewInit, ChangeDetectorRef, ChangeDetectionStrategy, Output, EventEmitter } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { GenericFormFieldsInterface } from '../../generic-form.interface';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'dynamic-input',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './dynamic-input.component.html',
  styleUrls: ['./dynamic-input.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DynamicInputComponent implements OnInit, AfterViewInit {


  constructor(private cdr: ChangeDetectorRef) {}
  @Input() 
  customForm!: FormGroup;

  @Input()
  field!: GenericFormFieldsInterface;

  

  ngOnInit() {
    // Initial setup logic if needed
  }

  ngAfterViewInit() {
    setTimeout(() => {
      if (this.customForm.get(this.field.field)) {
        this.customForm.patchValue({ [this.field.field]: this.field.value });
        this.cdr.markForCheck(); // Manually trigger change detection
      }
    });
  }

  hasError(controlName: string, errorName: string) {
    const control = this.customForm.get(controlName);
    return control?.invalid && (control?.dirty || control?.touched) && control?.hasError(errorName);
  }
  onInputChange($event: any) {
    

    if(typeof(this.field.onInputChange)=== 'function'){

      if($event.target.value ){
        const value = $event.target.value;
        this.field.onInputChange(value);
      }   

     

    }
   
      
   
  }
}
