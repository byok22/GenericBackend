import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnChanges, SimpleChanges, ViewChild, ViewContainerRef, Compiler, Type, NgModuleRef } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { GenericFormFieldsInterface } from '../../generic-form.interface';


@Component({
  selector: 'dynamic-custom',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  template: '<ng-container #dynamicContainer></ng-container>',
  styleUrls: ['./dynamic-custom.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DynamicCustomComponent implements OnChanges {

  @Input() field!: GenericFormFieldsInterface;
  @Input() htmlString!: string;
  @Input() data!: any;
  @Input() customForm!: FormGroup;
  
  @ViewChild('dynamicContainer', { read: ViewContainerRef, static: true }) dynamicContainer!: ViewContainerRef;

  constructor(private compiler: Compiler) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['htmlString'] && this.htmlString) {
      this.loadDynamicTemplate(this.htmlString);
    }
  }

  private async loadDynamicTemplate(template: string) {
    const dynamicComponentClass = Component({ template })(class {});

    const dynamicModuleClass = this.createDynamicModule(dynamicComponentClass);

    const moduleFactory = await this.compiler.compileModuleAsync(dynamicModuleClass);
    const moduleRef = moduleFactory.create(this.dynamicContainer.injector);

    const factory = moduleRef.componentFactoryResolver.resolveComponentFactory(dynamicComponentClass);
    if (factory) {
      this.dynamicContainer.clear();
      const componentRef = this.dynamicContainer.createComponent(factory);
      Object.assign(componentRef.instance, this.data, { customForm: this.customForm, field: this.field });
    } else {
      console.error('Component factory is null');
    }
  }

  createDynamicModule(dynamicComponent: Type<any>): Type<any> {
   /* @NgModule({
      declarations: [dynamicComponent], // Ensure dynamicComponent is a class reference
      imports: [CommonModule, ReactiveFormsModule],
    })*/
    class DynamicHtmlModule {}
  
    return DynamicHtmlModule;
  }
}


