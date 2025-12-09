import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, Type, ViewChild, ViewContainerRef } from '@angular/core';
import { DynamicSelectComponent } from './forms/dynamic-select/dynamic-select.component';
import { DynamicInputComponent } from './forms/dynamic-input/dynamic-input.component';
import { DynamicCheckBoxComponent } from './forms/dynamic-check-box/dynamic-check-box.component';
import { FormControl, FormGroup } from '@angular/forms';
import { GenericFormInterface } from './generic-form.interface';
import { CommonModule } from '@angular/common';
import { DynamicDateComponent } from './forms/dynamic-date/dynamic-date.component';
import { DynamicFileComponent } from './forms/dynamic-file/dynamic-file.component';
import { DynamicCustomComponent } from './forms/dynamic-custom/dynamic-custom.component';
import { DynamicTextAreaComponent } from './forms/dynamic-text-area/dynamic-text-area.component';
import { GenericUpdate } from '../../interfaces/generic-update.interface';
import { PrimengModule } from '../../modules/primeng.module';
import { GenericUpdateFormComponent } from "../generic-update-form/generic-update-form.component";

@Component({
  selector: 'generic-form',
  standalone: true,
  imports: [
    CommonModule,
    PrimengModule,
    //GenericUpdateFormComponent
  ],
  templateUrl: './generic-form.component.html',
  styleUrls: ['./generic-form.component.scss']
})
export class GenericFormComponent<T> implements AfterViewInit, OnInit {

  
  @Input() genericForm: GenericFormInterface<T> = {
    tittle: '',
    fields: [],
    customFromGroup: undefined,
    editAdd: '',
    customButton: ''
  };

  @Input() deletedEnabled?: boolean;

  @Input() saveUpdate?: boolean;

  currentUpdate: GenericUpdate = {
    id: 0,
    updatedAt: new Date().toISOString(),
    message: ''
  };

  @Output() messageUpdates = new EventEmitter<GenericUpdate>();

  maximizeUpdate: boolean = false;

  @ViewChild('dynamicInputContainer', { read: ViewContainerRef })
  dynamicInputContainer!: ViewContainerRef;

  supportedDynamicComponents = [
    {
      name: 'text',
      component: DynamicInputComponent
    },
    {
      name: 'textArea',
      component: DynamicTextAreaComponent
    },
    {
      name: 'number',
      component: DynamicInputComponent
    },
    {
      name: 'select',
      component: DynamicSelectComponent
    },
    {
      name: 'checkbox',
      component: DynamicCheckBoxComponent
    },
    {
      name: 'date',
      component: DynamicDateComponent
    },
    {
      name: 'file',
      component: DynamicFileComponent
    },
    {
      name: 'custom',
      component: DynamicCustomComponent
    }
  ];

  ngAfterViewInit(): void {
    this.registerDynamicFields();
  }

  ngOnInit(): void {
    this.buildForm();
  }

  public toggleMode(): void {
    if (this.genericForm.editAdd === 'Add') {
      this.genericForm.editAdd = 'Edit';
    } else {
      this.genericForm.editAdd = 'Add';
    }
  }

  public buildForm() {
    const formGroupFields: any = {};
    for (const field of this.genericForm.fields) {
      formGroupFields[field.field] = new FormControl(field.value || "");
    }
    this.genericForm.customFromGroup = new FormGroup(formGroupFields);
  }

  private cloneFormGroup(formGroup: FormGroup): FormGroup {
    const formGroupFields: any = {};
    Object.keys(formGroup.controls).forEach(key => {
      formGroupFields[key] = new FormControl(formGroup.controls[key].value);
    });
    return new FormGroup(formGroupFields);
  }

  public registerDynamicFields() {
    this.genericForm.fields.forEach(obj => {
      const componentConfig = this.supportedDynamicComponents.find(c => c.name === obj.type);
      if (componentConfig) {
        let dynamicComponentType: Type<any>;

        switch (componentConfig.name) {
          case 'text':
          case 'number':
            dynamicComponentType = DynamicInputComponent;
            break;
          case 'textArea':
            dynamicComponentType = DynamicTextAreaComponent;
            break;
          case 'select':
            dynamicComponentType = DynamicSelectComponent;
            break;
          case 'checkbox':
            dynamicComponentType = DynamicCheckBoxComponent;
            break;
          case 'date':
            dynamicComponentType = DynamicDateComponent;
            break;
          case 'file':
            dynamicComponentType = DynamicFileComponent;
            break;
          case 'custom':
            dynamicComponentType = DynamicCustomComponent;
            break;
          default:
            console.error(`Unsupported dynamic component type: ${componentConfig.name}`);
            return;
        }

        const dynamicComponent = this.dynamicInputContainer.createComponent(dynamicComponentType);
        dynamicComponent.instance.field = obj;

        if (this.genericForm.customFromGroup) {
          dynamicComponent.instance.customForm = this.genericForm.customFromGroup;
        } else {
          console.error('customFromGroup is undefined. Make sure it is properly initialized.');
        }

        if (dynamicComponentType === DynamicCustomComponent) {
          dynamicComponent.instance.htmlString = obj.value ? obj.value.toString() : '';
          dynamicComponent.instance.data = this.genericForm.data;
        }
      }
    });
  }

  public clearForm(): void {
    if (this.genericForm.customFromGroup) {
      this.genericForm.customFromGroup.reset();
    }
    if (this.genericForm.editAdd === 'Edit') {
      this.genericForm.fields.forEach((field) => {
        if (field.value !== undefined) {
          this.genericForm.customFromGroup?.get(field.field)?.setValue(field.value);
        }
      });
    }
  }

  updateMessage($event: GenericUpdate) {   
    this.maximizeUpdate = false;
    this.messageUpdates.emit($event);
   
    if (this.genericForm.customFromGroup) {
      this.genericForm.submitFunction?.(this.genericForm.customFromGroup);
    }
  }

  delete() 
  {
    if (this.genericForm.deleteFunction) 
    {
      this.genericForm.deleteFunction(this.currentUpdate.id);
    }    
  }

  onSubmit(): void {
    if (this.genericForm.submitFunction) {
      if (this.saveUpdate) {
        this.maximizeUpdate = true;
      } else {
        if (this.genericForm.customFromGroup) {
          this.genericForm.submitFunction(this.genericForm.customFromGroup);
        }
      }
    }
  }
}