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
//import { UsersService } from '../../../users/services/users.service';
import { MessageService } from 'primeng/api';

import { GenericTableConcretBuilder } from '../../shared/components/generic-table/builder/generic-table-concret-builder';
import { UserDto } from '../interfaces/user-dto';
import { UserService } from '../services/user.service';
import { RoleService } from '../services/role.service';
import { DropdownsService } from '../../common/service/dropdowns.service';
import { GenericPageTableMenuForm } from '../../common/interfaces/generic-page-table-menu-form';


//#endregion Imports

//#region  Inits
@Component({
  selector: 'users-page',
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
  templateUrl: './users.page.component.html',
  styleUrl: './users.page.component.css',
  changeDetection: ChangeDetectionStrategy.Default
})
export class UsersPageComponent implements OnInit, GenericPageTableMenuForm<UserDto> {

  builderTable: GenericTableConcretBuilder<UserDto>;

  constructor(
    private service: UserService,
    private roleService: RoleService,
    //private userService: UsersService,
    private serviceTable: TableBuilderFactoryService,  
    private fb: FormBuilder,
    private _message: MessageService ,  
  ) {
    this.FillMenu();
    this.ConfigMenu();
    this.GetDropdowns();
    this.builderTable = this.serviceTable.createBuilder<UserDto>();
  }

  GetDropdowns() {
    this.roleService.getRolesDropdown().subscribe({
      next: (role) => {
        this.roleDropdown = role;
      }
    });
  }

  roleDropdown: SelectOption[] = [];

  dataForm: WritableSignal<UserDto> = signal({
    id: 0,
    userName: '',
    ntUser: '',
    //employeeNumber: '',
    email: '',
    role: '',
    available: false
  });

  dataFormTemp: WritableSignal<UserDto> = signal({
    id: 0,
    userName: '',
    ntUser: '',
    //employeeNumber: '',
    email: '',
    role: '',
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
  tableConfig!: GenericTableConfig<UserDto>;
  dataTable: UserDto[] = [];
  hideTable = signal(true);
  public newTable = signal(true);
  public dataUser = signal<UserDto>({
    id: 0,
    userName: '',
    ntUser: '',
    //employeeNumber: '',
    email: '',
    role: '',
    available: false
  });

  public dataUserFormTemp: UserDto = {
    id: 0,
    userName: '',
    ntUser: '',
    //employeeNumber: '',
    email: '',
    role: '',
    available: false
  };

  public userTemp = signal<UserDto>({
    id: 0,
    userName: '',
    ntUser: '',
    //employeeNumber: '',
    email: '',
    role: '',
    available: false
  });

  public dataUsers = signal<UserDto[]>([]);

  public EditAdd = signal<string>('');
  public displayMaximizable: boolean = false;

  //Form
  genericForm: GenericFormInterface<UserDto> = {
    tittle: '',
    fields: [],
    customFromGroup: undefined,
    editAdd: '',
    data: this.dataUser()
  }

  testForm: GenericFormInterface<UserDto> = {
    tittle: '',
    fields: [],
    customFromGroup: undefined,
    editAdd: '',
    data: this.userTemp()
  };

  builderForm = new GenericFormConcretBuilder<UserDto>();
  builderTestForm = new GenericFormConcretBuilder<UserDto>();
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
    this.service.getStatus().subscribe({
      next: (status) => {
        this.statusDD = status;
        this.selectedStatus = status[0]?.id || '1';
        this.statusItem.item.options = this.statusDD;
        this.statusItem.item.selectedOption = this.selectedStatus;
      }
    });
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
    this.builderTable.SetTitle("User Table");
    this.builderTable.SetDataKey("id");
    this.builderTable.SetData(this.dataTable);
    this.builderTable.SetKpis(this.GetKpis());
    this.builderTable.SetPagination(true);
    this.builderTable.SetRowsPerPage(10);
    this.builderTable.SetRowsPerPageOptions([10, 15, 30]);
    this.builderTable.SetColumns(this.getColumns());
    this.builderTable.SetGlobalFilterFields(["userName"]);
    this.tableConfig = this.builderTable.Generate();
  }

  GetKpis(): BasicKpi[] {
    return [
      { title: "Total", total: this.dataTable.length.toString() },
    ];
  }

  getColumns(): TableColumn[] {
    const manualColumns: TableColumn[] = [
      { field: 'id', header: 'ID' },
      { field: 'userName', header: 'User Name' },
      { field: 'ntUser', header: 'NT User' },
      //{ field: 'employeeNumber', header: 'Employee Number' },
      { field: 'email', header: 'Email' },
      { field: 'role', header: 'Role' },
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
      this.service.getAllUsers().subscribe({
        next: (usersRequest) => {
          if (usersRequest.length < 1) {
            usersRequest = [{
              id: 0,
              userName: 'No Users Found',
              ntUser: '',
              //employeeNumber: '',
              email: '',
              role: '',
              available: false
            } as UserDto];
          }

          const transformedUserRequest = usersRequest.map(request => ({
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
    this.dataUserFormTemp = this.dataUser();
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
      value: this.dataUser().id
    });

    this.builderForm.SetField({
      field: 'userName',
      label: 'User Name',
      order: 2,
      required: true,
      type: 'text',
      validationRequired: true,
      enable: true,
      show: true,
      value: this.dataUser().userName,
      onInputChange: (event: string) => {
        this.dataUserFormTemp.userName = event;
      }
    });

    this.builderForm.SetField({
      field: 'ntUser',
      label: 'NT User',
      order: 3,
      required: true,
      type: 'text',
      validationRequired: false,
      enable: true,
      show: true,
      value: this.dataUser().ntUser,
      onInputChange: (event: string) => {
        this.dataUserFormTemp.ntUser = event;
      }

    });

    // this.builderForm.SetField({
    //   field: 'employeeNumber',
    //   label: 'Employee Number',
    //   order: 4,
    //   required: true,
    //   type: 'text',
    //   validationRequired: true,
    //   enable: true,
    //   show: true,
    //   value: this.dataUser().employeeNumber,
    //   onInputChange: (event: string) => {
    //     this.dataUserFormTemp.employeeNumber = event;
    //   }
    // });

    this.builderForm.SetField({
      field: 'email',
      label: 'Email',
      order: 5,
      required: true,
      type: 'text',
      validationRequired: false,
      enable: true,
      show: true,
      value: this.dataUser().email,
      onInputChange: (event: string) => {
        this.dataUserFormTemp.email = event;
      }
    });


    const selectRole = this.roleDropdown.find(role => role.text === this.dataUser().role || role.id === this.dataUser().role);

    this.builderForm.SetField({
      field: 'role',
      label: 'Role',
      order: 6,
      required: true,
      type: 'select',
      options: this.roleDropdown,  // [{ id: 2, text: 'Editor' }, ...]
      validationRequired: true,
      enable: true,
      show: true,
      value: selectRole?.id,  // <-- el valor inicial debe ser el ID del rol
       onInputChange: (event: string) => {

        const selectRole2 = this.roleDropdown.find(role => role.id == event);
        
         this.dataUserFormTemp.role = selectRole2.text;  // Guardar el ID directamente en el objeto
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
      value: this.dataUser().available,
      onInputChange: (event: boolean) => {
        this.dataUserFormTemp.available = event;
      }
    });

    this.builderForm.SetFormGroup(
      this.fb.group({
        id: [this.dataUser().id],
        userName: [this.dataUser().userName],
        ntUser: [this.dataUser().ntUser],
        //employeeNumber: [this.dataUser().employeeNumber],
        email: [this.dataUser().email],
        role: [this.dataUser().role],
        available: [this.dataUser().available]
      })
    );

    this.builderForm.SetSubmitFunction(() => {
      this.SubmitRequests();
    });

    this.builderForm.SetTitle('User Form');
    this.genericForm = this.builderForm.Generate();
  }

  SubmitRequests(): void {
    console.log('Se hizo Submit');
    console.log(this.dataUser());

    //  Verifica que el formulario exista y sea válido
    if (!this.genericForm.customFromGroup || this.genericForm.customFromGroup.invalid) {
      // Marca todos los campos como tocados para que se vean los errores
      this.genericForm.customFromGroup?.markAllAsTouched();
      return; // No continúes si el formulario es inválido
    }

    //  Formulario válido, continuar
    const formValues = this.genericForm.customFromGroup.value;
      this.dataUser.set({
        id: formValues.id,
        userName: formValues.userName,
        ntUser: formValues.ntUser,
        //employeeNumber: formValues.employeeNumber,
        email: formValues.email ?? '',
        role: formValues.role,
        available: formValues.available
      });

    this.submit.set(true);
    

    console.log('Se hizo Submit');
    console.log(this.dataUser());
    
    if (this.EditAdd() == 'Add') {
      //obtener el id del rol
      const selectRole = this.roleDropdown.find(role => role.id == this.dataUserFormTemp.role);
      this.dataUser().role = selectRole?.id;
      console.log('Rol enviado:', this.dataUser().role);
      this.dataUserFormTemp.id = this.dataUser().id;
      this.dataUserFormTemp.role = selectRole?.id; 

      this.service.createUser(this.dataUserFormTemp).subscribe({
        next: (response) => {
          this._message.add({
            severity: 'success',
            summary: 'Add!',
            detail: `User ${response.message} Added`,
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
    // convertir lo mismo para el update
    const selectRole = this.roleDropdown.find(role => role.text == this.dataUserFormTemp.role);
    this.dataUser().role = selectRole?.id;
    console.log('Rol enviado:', this.dataUser().role);
    this.dataUserFormTemp.id = this.dataUser().id;
    this.dataUserFormTemp.role = selectRole?.id;    
    this.service.updateUser(this.dataUserFormTemp).subscribe({
      next: (response) => {
        this._message.add({
          severity: 'success',
          summary: 'Edit!',
          //detail: `User ${response.message} Updated`,
          life: 2000
        });

        this.GetTable(this.selectedStatus);
      },
      error: () => {},
      complete: () => {}
    });
  }
    this.displayMaximizable = false;
    this.dataUsers = signal<UserDto[]>([]);
    console.log(this.genericForm.data);
  }

  getModal(item: UserDto = {} as UserDto) {
    this.submit.set(false);

    if (item.id == 0 || item.id == undefined) {
      this.EditAdd.set('Add')
    } else {
      this.EditAdd.set('Edit')
    }

    if (this.EditAdd() == 'Edit') {
      this.dataUser.set(item);
      this.ConfigForm();
      this.displayMaximizable = true;
      let tests: UserDto;
      this.service.getUserById(item.id).subscribe({
        next: (data) => {
          tests = data;
          this.dataUser = signal<UserDto>(tests);
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
      const dataUserTemp: UserDto = {
        id: 0,
        userName: '',
        ntUser: '',
        //employeeNumber: '',
        email: '',
        role: '',
        available: false
      }

      this.dataUser.set(dataUserTemp);
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
