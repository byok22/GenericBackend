import { AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { GenericFormFieldsInterface } from '../../generic-form.interface';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'dynamic-select',
  standalone: true,
  imports:[ReactiveFormsModule, CommonModule],
  templateUrl: './dynamic-select.component.html',
  styleUrls: ['./dynamic-select.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DynamicSelectComponent implements OnInit, AfterViewInit {
  @Input() customForm!: FormGroup;
  @Input() field!: GenericFormFieldsInterface;

  constructor(private cdr: ChangeDetectorRef) {}

  ngAfterViewInit(): void {
    setTimeout(() => {
      if (this.customForm.get(this.field.field)) {
        this.customForm.patchValue({ [this.field.field]: this.field.value });
        this.cdr.markForCheck(); 
      }
    });
  }

  ngOnInit(): void { }

  // Getter para saber si mostramos la X
  get showClearButton(): boolean {
    const control = this.customForm.get(this.field.field);
    // Mostramos si el control existe y tiene un valor (no es null ni vacío)
    return control ? !!control.value : false;
  }

  onSelectChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    // Convertimos a número si tus IDs son numéricos, o string si son texto. 
    // Aquí lo dejo genérico.
    const val = select.value; 
    
    this.updateValue(val);
  }

  // Nueva función para limpiar
  clearValue(event: Event): void {
    // Importante: stopPropagation evita que al dar click en la X se abra el select
    event.stopPropagation();
    event.preventDefault();
    
    this.updateValue(null); // O '' string vacío, según tu backend
    
    // Como estamos en OnPush y el evento no vino del input, forzamos la detección
    this.cdr.markForCheck();
  }

  // Refactoricé la actualización para reusarla en ambos casos
  private updateValue(value: any): void {
    this.customForm.get(this.field.field)?.setValue(value);
    
    if(typeof(this.field.onInputChange) === 'function'){
      this.field.onInputChange(value);
    }
  }

  hasError(controlName: string, errorName: string) {
    const control = this.customForm.get(controlName);
    return control?.invalid && (control?.dirty || control?.touched) && control?.hasError(errorName);
  }
}