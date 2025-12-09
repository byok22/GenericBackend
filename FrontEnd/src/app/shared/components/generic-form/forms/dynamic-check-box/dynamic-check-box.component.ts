import {  Component, Input, OnInit } from '@angular/core';
import {  FormGroup, ReactiveFormsModule } from '@angular/forms';
import { GenericFormFieldsInterface } from '../../generic-form.interface';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'dynamic-check-box',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule ,
  ],
  templateUrl: './dynamic-check-box.component.html',
  styleUrls: ['./dynamic-check-box.component.scss']
})
export class DynamicCheckBoxComponent implements OnInit {
  @Input() 
  customForm!: FormGroup;

  @Input()
  field!: GenericFormFieldsInterface;

  ngOnInit(): void {
    // Verificar si el control existe en el formulario
    if (this.customForm.get(this.field.field)) {
      this.customForm.patchValue({ [this.field.field]: this.field.value });
    }
  }

  hasError(controlName: string, errorName: string) {
    const control = this.customForm.get(controlName);
    return control?.invalid && (control?.dirty || control?.touched) && control?.hasError(errorName);
  }

  onCheckboxChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.customForm.get(this.field.field)?.setValue(input.checked);

    if(typeof(this.field.onInputChange)=== 'function'){

     
        this.field.onInputChange(input.checked);
     

     

    }

  }

}
