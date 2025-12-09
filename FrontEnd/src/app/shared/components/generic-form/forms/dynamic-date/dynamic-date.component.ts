import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { GenericFormFieldsInterface } from '../../generic-form.interface';

@Component({
  selector: 'app-dynamic-date',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './dynamic-date.component.html',
  styleUrls: ['./dynamic-date.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DynamicDateComponent implements OnInit {
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
  onDateChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.customForm.get(this.field.field)?.setValue(input.value);
    if(typeof(this.field.onInputChange)=== 'function'){
      this.field.onInputChange(input.value);
    }
  }

  hasError(controlName: string, errorName: string) {
    const control = this.customForm.get(controlName);
    return control?.invalid && (control?.dirty || control?.touched) && control?.hasError(errorName);
  }
}
