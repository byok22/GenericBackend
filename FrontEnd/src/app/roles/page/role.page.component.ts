//#region  Imports
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit, signal, WritableSignal } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { GenericFormComponent } from '../../shared/components/generic-form/generic-form.component';
import { GenericMenuComponent } from '../../shared/components/generic-menu/generic-menu.component';
import { GenericTableComponent } from '../../shared/components/generic-table/generic-table.component';
import { GenericTitleComponent } from '../../shared/components/generic-title/generic-title.component';
import { PrimengModule } from '../../shared/modules/primeng.module';
import { DatePipe } from '@angular/common';
import { GenericMenuInterface } from '../../shared/components/generic-menu-item/interfaces/generic-menu-item.interface';
import { SelectOption } from '../../shared/interfaces/select-option.interface';
import { GenericMenuConcreteBuilder } from '../../shared/components/generic-menu/builder/generic-menu-concret-builder';
import { GenericTableConfig } from '../../shared/components/generic-table/interfaces/generic-table-config';
import { TableBuilderFactoryService } from '../../shared/components/generic-table/service/table-builder-factory-service.service';
import { BasicKpi } from '../../shared/interfaces/basic-kpi.interface';
import { TableColumn } from '../../shared/components/generic-table/interfaces/table-column';
import { GenericFormInterface } from '../../shared/components/generic-form/generic-form.interface';
import { GenericFormConcretBuilder } from '../../shared/components/generic-form/builder/generic-form-concret-builder';
import { GenericStatus } from '../../shared/enums/generic-status.enum';
import { FormBuilder } from '@angular/forms';
//import { RolesService } from '../../../Roles/services/Roles.service';
import { MessageService } from 'primeng/api';

import { GenericTableConcretBuilder } from '../../shared/components/generic-table/builder/generic-table-concret-builder';
import { RoleDto } from '../interfaces/role-dto';
import { RoleCatalogoService } from '../services/role-catalogo.service';
import { DropdownsService } from '../../common/service/dropdowns.service';
import { GenericPageTableMenuForm } from '../../common/interfaces/generic-page-table-menu-form';


//#endregion Imports

//#region  Inits
@Component({
  selector: 'role-page',
  standalone: true,
  imports: [
    CommonModule, 
    //GenericMenuComponent, 
    GenericTableComponent, 
    HttpClientModule, 
    GenericTitleComponent, 
    GenericFormComponent, 
    PrimengModule, 
    FontAwesomeModule
  ],
  providers: [DatePipe, DropdownsService, MessageService],
  templateUrl: './role.page.component.html',
  styleUrl: './role.page.component.css',
  changeDetection: ChangeDetectionStrategy.Default
})
export class RolesPageComponent implements OnInit, GenericPageTableMenuForm<RoleDto> {

  builderTable: GenericTableConcretBuilder<RoleDto>;

  constructor(
    private service : RoleCatalogoService,
    //private RoleService: RolesService,
    private serviceTable: TableBuilderFactoryService,  
    private fb: FormBuilder,
    private _message: MessageService ,  
  ) {
    this.FillMenu();
    this.ConfigMenu();
    //this.GetDropdowns();
    this.builderTable = this.serviceTable.createBuilder<RoleDto>();
  }

  // GetDropdowns() {
  //   this.roleService.getRolesDropdown().subscribe({
  //     next: (role) => {
  //       this.roleDropdown = role;
  //     }
  //   });
  // }

  roleDropdown: SelectOption[] = [];

  dataForm: WritableSignal<RoleDto> = signal({
    pkRole: 0,
    roleName: '',
    available: false
  });

  dataFormTemp: WritableSignal<RoleDto> = signal({
    pkRole: 0,
    roleName: '',
    available: false
  });

  ngOnInit(): void {
    this.showSpinner =true;    
              setTimeout(() => {
                this.GetTable(this.selectedStatus);
    
              }, 1000);
    this.ConfigForm();
  }

  //#region  Variables

  showSpinner:Boolean =false;

  //Menu
  menuItems: GenericMenuInterface[] = [];

  //Table
  tableConfig!: GenericTableConfig<RoleDto>;
  dataTable: RoleDto[] = [];
  hideTable = signal(true);
  public newTable = signal(true);
  public dataRole = signal<RoleDto>({
    pkRole: 0,
    roleName: '',
    available: false
  });

  public dataRoleFormTemp: RoleDto = {
    pkRole: 0,
    roleName: '',
    available: false
  };

  public RoleTemp = signal<RoleDto>({
    pkRole: 0,
    roleName: '',
    available: false
  });

  public dataRoles = signal<RoleDto[]>([]);

  public EditAdd = signal<string>('');
  public displayMaximizable: boolean = false;

  //Form
  genericForm: GenericFormInterface<RoleDto> = {
    tittle: '',
    fields: [],
    customFromGroup: undefined,
    editAdd: '',
    data: this.dataRole()
  }

  testForm: GenericFormInterface<RoleDto> = {
    tittle: '',
    fields: [],
    customFromGroup: undefined,
    editAdd: '',
    data: this.RoleTemp()
  };

  builderForm = new GenericFormConcretBuilder<RoleDto>();
  builderTestForm = new GenericFormConcretBuilder<RoleDto>();
  statuses: SelectOption[] = this.getEnumSelectOptions(GenericStatus);
  public submit = signal(false);

  //#endregion

  //#region  Menu
  statusDD: SelectOption[] = [];
  selectedStatus: string = '';

  statusItem: GenericMenuInterface = {
    item: {
      selectedOption: this.selectedStatus,
      options: this.statusDD, onChange: (event: string) => {
        this.selectedStatus = event;
        this.hideTable.set(true);
        console.log('Selected option changed:', event);
      }
    },
    labelText: 'Status',
    order: 1,
    type: 'dropdown'
  }

  FillMenu(): void {
    // this.service.getRoles().subscribe({
    //   next: (status) => {
    //     this.statusDD = status;
    //     this.selectedStatus = status[0]?.id || '1';
    //     this.statusItem.item.options = this.statusDD;
    //     this.statusItem.item.selectedOption = this.selectedStatus;
    //   }
    // });
  }

  ConfigMenu(): void {
    const builder = new GenericMenuConcreteBuilder();
    builder.Reset();
    builder.SetDropDown(this.statusItem);
    builder.SetButton({
      item: {
        onClick: () => {
          this.GetTable(this.selectedStatus);
        }
      },
      labelText: 'Find',
      order: 1,
      type: 'button'
    });
    this.menuItems = builder.Generate();
  }
  //#endregion

  //#region  Table

  ConfigTable() {
    this.builderTable.Reset();
    this.builderTable.SetTitle("Role Table");
    this.builderTable.SetDataKey("id");
    this.builderTable.SetData(this.dataTable);
    this.builderTable.SetKpis(this.GetKpis());
    this.builderTable.SetPagination(true);
    this.builderTable.SetRowsPerPage(10);
    this.builderTable.SetRowsPerPageOptions([5, 10, 20]);
    this.builderTable.SetColumns(this.getColumns());
    this.builderTable.SetGlobalFilterFields(["roleName"]);
    this.tableConfig = this.builderTable.Generate();
  }

  GetKpis(): BasicKpi[] {
    return [
      { title: "Total", total: this.dataTable.length.toString() },
    ];
  }

  getColumns(): TableColumn[] {
    const manualColumns: TableColumn[] = [
      { field: 'pkRole', header: 'ID' },
      { field: 'roleName', header: 'Roles Name' },
      { field: 'available', header: 'Available' }
    ];

    const data = this.dataTable;
    const columnFields = Object.keys(data[0]);

    const manualFields = manualColumns.map(col => col.field);
    const filteredColumnFields = columnFields.filter(field => !manualFields.includes(field));

    const dataColumns: TableColumn[] = filteredColumnFields.map(field => ({
      field,
      header: this.capitalizeFirstLetter(field)
    }));

    let columns: TableColumn[] = [...manualColumns, ...dataColumns];

    const fieldsToHide = [""];

    columns = columns.map(column => ({
      ...column,
      showHeader: !fieldsToHide.includes(column.field)
    }));

    return columns;
  }

  capitalizeFirstLetter(word: string): string {
    if (!word) return word;
    return word[0].toUpperCase() + word.substr(1).toLowerCase();
  }

  GetTable(status: string | any, ...args: any[]): void {
    try {
      this.service.GetAllRole().subscribe({
        next: (roleRequest) => {
          if (roleRequest.length < 1) {
            roleRequest = [{
              pkRole: 0,
              roleName: 'No role Found',
              available: false
            } as RoleDto];
          }

          const transformedUserRequest = roleRequest.map(request => ({
            ...request
          }));

          this.dataTable = transformedUserRequest;

          if (this.newTable()) {
            this.ConfigTable();
            this.newTable.set(false);
            this.hideTable.set(false);
            return;
          }
          this.ConfigTable();
          this.builderTable.SetData(this.dataTable);
          this.hideTable.set(false);
        },
        error: (error) => {
          console.error(error);
        },
        complete: () => {this.showSpinner=false }
      });
    } catch (error) {
      this.showSpinner=false
      console.error('Error fetching data', error);
      throw error;
    }
  }

  //#endregion

  //#region Form

  ConfigForm() {
    this.dataRoleFormTemp = this.dataRole();
    this.builderForm.Reset();
    this.builderForm.SetEditAdd(this.EditAdd().toString());

    this.builderForm.SetField({
      field: 'id',
      label: 'id',
      order: 1,
      required: false,
      type: 'text',
      validationRequired: false,
      enable: false,
      show: false,
      value: this.dataRole().pkRole
    });

    this.builderForm.SetField({
      field: 'roleName',
      label: 'Role Name',
      order: 2,
      required: true,
      type: 'text',
      validationRequired: true,
      enable: true,
      show: true,
      value: this.dataRole().roleName,
      onInputChange: (event: string) => {
        this.dataRoleFormTemp.roleName = event;
      }
    });

    this.builderForm.SetField({
      field: 'available',
      label: 'Available',
      order: 7,
      required: true,
      type: 'checkbox',
      validationRequired: true,
      enable: true,
      show: true,
      value: this.dataRole().available,
      onInputChange: (event: boolean) => {
        this.dataRoleFormTemp.available = event;
      }
    });

    this.builderForm.SetFormGroup(
      this.fb.group({
        pkRole: [this.dataRole().pkRole],
        roleName: [this.dataRole().roleName],
        available: [this.dataRole().available]
      })
    );

    this.builderForm.SetSubmitFunction(() => {
      this.SubmitRequests();
    });

    this.builderForm.SetTitle('Role Form');
    this.genericForm = this.builderForm.Generate();
  }

  SubmitRequests(): void {
      console.log('Se hizo Submit');
      console.log(this.dataRole());
  
      //  Verifica que el formulario exista y sea válido
      if (!this.genericForm.customFromGroup || this.genericForm.customFromGroup.invalid) {
        // Marca todos los campos como tocados para que se vean los errores
        this.genericForm.customFromGroup?.markAllAsTouched();
        return; // No continúes si el formulario es inválido
      }
      //  Formulario válido, continuar
      const formValues = this.genericForm.customFromGroup.value;
  
        this.dataRole.set({
          pkRole: formValues.pkRole,
          roleName: formValues.roleName,
          available: formValues.available
        });
        
      this.submit.set(true);   
  
      console.log('Se hizo Submit');
      console.log(this.dataRole());
  
      if (this.EditAdd() == 'Add') {
        this.service.createRole(this.dataRole()).subscribe({
          next: (response) => {
            this._message.add({
              severity: 'success',
              summary: 'Add!',
              //detail: `Status ${response.message} Added`,
              life: 2000
            });
  
            setTimeout(() => {
              this.GetTable(this.selectedStatus);
            }, 1000);
          },
          error: () => {},
          complete: () => {}
        });
      } else {
        this.service.updateRole(this.dataRoleFormTemp).subscribe({
          next: (response) => {
            this._message.add({
              severity: 'success',
              summary: 'Edit!',
              //detail: `Status ${response.message} Updated`,
              life: 2000
            });
            this.GetTable(this.selectedStatus);
          },
          error: () => {},
          complete: () => {}
        });
      }
  
      this.displayMaximizable = false;
      this.dataRoles = signal<RoleDto[]>([]);
      console.log(this.genericForm.data);
    }

  getModal(item: RoleDto = {} as RoleDto) {
    this.submit.set(false);

    if (item.pkRole == 0 || item.pkRole == undefined) {
      this.EditAdd.set('Add')
    } else {
      this.EditAdd.set('Edit')
    }

    if (this.EditAdd() == 'Edit') {
      this.dataRole.set(item);
      this.ConfigForm();
      this.displayMaximizable = true;
      let tests: RoleDto;
      this.service.getRoleById(item.pkRole).subscribe({
        next: (data) => {
          tests = data;
          this.dataRole = signal<RoleDto>(tests);
        },
        error: (error) => {
          console.error(error);
          this.ConfigForm();
        },
        complete: () => {
          this.ConfigForm();
        }
      });
    } else {
      const dataRoleTemp: RoleDto = {
        pkRole: 0,
        roleName: '',
        available: false
      }

      this.dataRole.set(dataRoleTemp);
      this.ConfigForm();
      this.displayMaximizable = true;
    }
  }

  //#endregion

  //#region Extras

  getEnumSelectOptions(enumType: any): SelectOption[] {
    return Object.values(enumType).map(value => ({
      id: this.generateRandomId(),
      text: value as string
    }));
  }

  generateRandomId(): string {
    return Math.random().toString(36).substr(2, 9);
  }

  //#endregion
}
