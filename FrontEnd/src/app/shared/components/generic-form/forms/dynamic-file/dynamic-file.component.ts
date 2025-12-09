import { AfterViewInit, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { GenericFormFieldsInterface } from '../../generic-form.interface';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'dynamic-file',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './dynamic-file.component.html',
  styleUrls: ['./dynamic-file.component.scss']
})
export class DynamicFileComponent implements OnInit, AfterViewInit {
  
  constructor(private cdr: ChangeDetectorRef) {}

  @Input() 
  customForm!: FormGroup;

  @Input()
  field!: GenericFormFieldsInterface;

  ngAfterViewInit(): void {
    setTimeout(() => {
      if (this.customForm.get(this.field.field)) {
        this.customForm.patchValue({ [this.field.field]: this.field.value });
        this.cdr.markForCheck(); // Manually trigger change detection
      }
    });
  }

  ngOnInit() {
   /* if (this.customForm.get(this.field.field)) {
      this.customForm.patchValue({ [this.field.field]: this.field.value });
    }*/
  }

  hasError(controlName: string, errorName: string) {
    const control = this.customForm.get(controlName);
    return control?.invalid && (control?.dirty || control?.touched) && control?.hasError(errorName);
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.customForm.patchValue({ [this.field.field]: file });
      // Optionally, you can mark the control as touched to trigger validation
      this.customForm.get(this.field.field)?.markAsTouched();
    }
    if(typeof(this.field.onInputChange)=== 'function'){
      this.field.onInputChange(input.files);
    }
  }
}
