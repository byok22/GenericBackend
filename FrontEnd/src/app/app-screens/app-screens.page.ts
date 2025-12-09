//#region  Imports
import { CommonModule, DatePipe } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit, signal, WritableSignal } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

// --- Imports de AppScreen ---

import { FormBuilder } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { GenericPageTableMenuForm } from '../common/interfaces/generic-page-table-menu-form';
import { GenericFormConcretBuilder } from '../shared/components/generic-form/builder/generic-form-concret-builder';
import { GenericFormComponent } from '../shared/components/generic-form/generic-form.component';
import { GenericFormInterface } from '../shared/components/generic-form/generic-form.interface';
import { GenericMenuConcreteBuilder } from '../shared/components/generic-menu/builder/generic-menu-concret-builder';
import { GenericMenuComponent } from '../shared/components/generic-menu/generic-menu.component';
import { GenericMenuInterface } from '../shared/components/generic-menu/interfaces/generic-menu-item.interface';
import { GenericTableConcretBuilder } from '../shared/components/generic-table/builder/generic-table-concret-builder';
import { GenericTableComponent } from '../shared/components/generic-table/generic-table.component';
import { GenericTableConfig } from '../shared/components/generic-table/interfaces/generic-table-config';
import { TableColumn } from '../shared/components/generic-table/interfaces/table-column';
import { TableBuilderFactoryService } from '../shared/components/generic-table/service/table-builder-factory-service.service';
import { GenericTitleComponent } from '../shared/components/generic-title/generic-title.component';
import { BasicKpi } from '../shared/interfaces/basic-kpi.interface';
import { SelectOption } from '../shared/interfaces/select-option.interface';
import { PrimengModule } from '../shared/modules/primeng.module';
import { UserService } from '../users/services/user.service';
import { AppScreenDto } from './interfaces/app-screen-dto';
import { AppScreenService } from './services/app-screen.service';
import { FontAwesomeIcons } from '../master-page/components/sidenav/enums/font-aswesome-icons.enum';



//#endregion Imports

//#region  Inits
@Component({
  selector: 'app-screens-page',
  standalone: true,
  imports: [
    CommonModule, // Activado
    GenericTableComponent,
    GenericTitleComponent,
    HttpClientModule,
    GenericFormComponent,
    PrimengModule,
    FontAwesomeModule,
    GenericMenuComponent
],
  providers: [
    DatePipe, 
    MessageService,
    AppScreenService, // Servicio de esta página
    UserService       // Servicio para el dropdown de usuarios
  ],
  templateUrl: './app-screens.page.html',
  styleUrl: './app-screens.page.scss',
  changeDetection: ChangeDetectionStrategy.Default
})
export class AppScreensPageComponent implements OnInit, GenericPageTableMenuForm<AppScreenDto> {

  builderTable: GenericTableConcretBuilder<AppScreenDto>;

  constructor(
    private service: AppScreenService,
    private userService: UserService, // Para el dropdown de FKUser
    private serviceTable: TableBuilderFactoryService,
    private fb: FormBuilder,
    private _message: MessageService,
  ) {
    this.FillMenu();
    this.ConfigMenu();
    this.GetDropdowns(); // Cargar los dropdowns para el formulario
    this.builderTable = this.serviceTable.createBuilder<AppScreenDto>();
  }

  GetDropdowns() {
    // Cargar dropdown de pantallas padre
    this.service.getAppScreensDropdown().subscribe({
      next: (screens) => {
        this.parentScreenDropdown = screens;
      }
    });

    // Cargar dropdown de usuarios (reutilizando UserService)
    this.userService.getUsersDropdown().subscribe({
      next: (users) => {
        this.userDropdown = users;
      }
    });

    // Cargar dropdown de iconos (estático desde el Enum)
    const iconValues = Object.values(FontAwesomeIcons);
    this.iconDropdown = iconValues.map(iconValue => ({
      id: iconValue, // Ej: 'faThumbtack'
      text: iconValue  // Ej: 'faThumbtack'
    }));


  }

  parentScreenDropdown: SelectOption[] = [];
  userDropdown: SelectOption[] = [];
  iconDropdown: SelectOption[] = []; // <--- AÑADE ESTA LÍNEA

  // Estructura del DTO
  dataForm: WritableSignal<AppScreenDto> = signal({
    appScreenID: 0,
    parentAppScreenID: null,
    parentScreen:'',
    screen: '',
    url: '',
    sortOrder: 0,
    icon: FontAwesomeIcons.faB,
    userID: 0,
    available: true
  });

  // Copia temporal para el formulario
  dataFormTemp: WritableSignal<AppScreenDto> = signal({ ...this.dataForm() });

  ngOnInit(): void {
    this.showSpinner = true;
    setTimeout(() => {
      this.GetTable(this.selectedStatus);
    }, 1000);
    this.ConfigForm();
  }

  //#region  Variables
  showSpinner: Boolean = false;

  //Menu
  menuItems: GenericMenuInterface[] = [];

  //Table
  tableConfig!: GenericTableConfig<AppScreenDto>;
  dataTable: AppScreenDto[] = [];
  hideTable = signal(true);
  public newTable = signal(true);
  
  public EditAdd = signal<string>('');
  public displayMaximizable: boolean = false;

  //Form
  genericForm: GenericFormInterface<AppScreenDto> = {
    tittle: '',
    fields: [],
    customFromGroup: undefined,
    editAdd: '',
    data: this.dataForm()
  }

  builderForm = new GenericFormConcretBuilder<AppScreenDto>();
  public submit = signal(false);
  //#endregion

  //#region  Menu
  statusDD: SelectOption[] = [];
  selectedStatus: string = '1'; // Default a 'Activo'

  statusItem: GenericMenuInterface = {
    item: {
      selectedOption: this.selectedStatus,
      options: this.statusDD, onChange: (event: string) => {
        this.selectedStatus = event;
        this.hideTable.set(true);
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
    this.builderTable.SetTitle("App Screens Table");
    this.builderTable.SetDataKey("appScreenID");
    this.builderTable.SetData(this.dataTable);
    this.builderTable.SetKpis(this.GetKpis());
    this.builderTable.SetPagination(true);
    this.builderTable.SetRowsPerPage(10);
    this.builderTable.SetRowsPerPageOptions([10, 15, 30]);
    this.builderTable.SetColumns(this.getColumns());
    this.builderTable.SetGlobalFilterFields(["screen", "url", "parentScreen", "userName"]);
    this.tableConfig = this.builderTable.Generate();
  }

  GetKpis(): BasicKpi[] {
    return [
      { title: "Total", total: this.dataTable.length.toString() },
    ];
  }

  getColumns(): TableColumn[] {
    // Definimos las columnas manualmente para tener control total
    const manualColumns: TableColumn[] = [
      { field: 'appScreenID', header: 'ID' },
      { field: 'screen', header: 'Screen' },
      { field: 'url', header: 'URL' },
      { field: 'parentScreen', header: 'Parent' }, // Asumiendo que el DTO de 'all' lo trae
      { field: 'sortOrder', header: 'Order' },
      { field: 'icon', header: 'Icon' },     
      { field: 'available', header: 'Available' }
    ];

    return manualColumns.map(column => ({
        ...column,
        showHeader: true // Aseguramos que todas se muestren
      }));
  }

  GetTable(status: string | any, ...args: any[]): void {
    try {
      this.service.getAllAppScreens(Number(status)).subscribe({
        next: (screensRequest) => {
          
          // Filtramos por status (available)
          //const isActive = (status == '1');
          //let filteredData = screensRequest.filter(s => s.available === isActive);

           let filteredData = screensRequest;

          if (filteredData.length < 1) {
            filteredData = []; // No mostrar nada si no hay datos
          }

          this.dataTable = filteredData;

          if (this.newTable()) {
            this.ConfigTable();
            this.newTable.set(false);
          } else {
            this.ConfigTable(); // Reconfiguramos por si las columnas cambian (aunque aquí no)
            this.builderTable.SetData(this.dataTable);
          }

          this.hideTable.set(false);
        },
        error: (error) => {
          console.error(error);
          this.showSpinner = false;
        },
        complete: () => { this.showSpinner = false }
      });
    } catch (error) {
      this.showSpinner = false
      console.error('Error fetching data', error);
      throw error;
    }
  }
  //#endregion

  //#region Form
  ConfigForm() {
    const currentData = this.dataForm();
    this.dataFormTemp.set({ ...currentData }); // Sincronizar temp

    this.builderForm.Reset();
    this.builderForm.SetEditAdd(this.EditAdd());

    // --- Campos del formulario ---

    this.builderForm.SetField({
      field: 'appScreenID', label: 'ID', order: 1, type: 'text',
      enable: false, show: false, validationRequired: false, required: false,
      value: currentData.appScreenID
    });

    this.builderForm.SetField({
      field: 'screen', label: 'Screen Name', order: 2, type: 'text',
      enable: true, show: true, validationRequired: true, required: true,
      value: currentData.screen,
      onInputChange: (event: string) => { this.dataFormTemp().screen = event; }
    });
    
    this.builderForm.SetField({
      field: 'parentAppScreenID', label: 'Parent Screen', order: 3, type: 'select',
      options: this.parentScreenDropdown,
      enable: true, show: true, validationRequired: false, required: false,
      value: currentData.parentAppScreenID?.toString() || '0', // '0' es "Ninguno"
      onInputChange: (event: string) => { 
        this.dataFormTemp().parentAppScreenID = event === '0' ? null : Number(event); 
      }
    });

    this.builderForm.SetField({
      field: 'url', label: 'URL / Route', order: 4, type: 'text',
      enable: true, show: true, validationRequired: true, required: true,
      value: currentData.url,
      onInputChange: (event: string) => { this.dataFormTemp().url = event; }
    });

    this.builderForm.SetField({
      field: 'sortOrder', label: 'Sort Order', order: 5, type: 'number', // Usar 'number'
      enable: true, show: true, validationRequired: true, required: true,
      value: currentData.sortOrder,
      onInputChange: (event: number) => { this.dataFormTemp().sortOrder = event; }
    });

    this.builderForm.SetField({
      field: 'icon',
      label: 'Icon', // Etiqueta actualizada
      order: 6,
      type: 'select', // <-- CAMBIADO
      options: this.iconDropdown, // <-- AÑADIDO
      enable: true,
      show: true,
      validationRequired: false,
      required: false,
      value: currentData.icon, // El valor (string del enum) ya coincide
      onInputChange: (event: string) => {
        // El evento es el string seleccionado, lo asignamos
        this.dataFormTemp().icon = event as FontAwesomeIcons; // <-- Actualizado
      }
    });
  

    this.builderForm.SetField({
      field: 'available', label: 'Available', order: 8, type: 'checkbox',
      enable: true, show: true, validationRequired: true, required: true,
      value: currentData.available,
      onInputChange: (event: boolean) => { this.dataFormTemp().available = event; }
    });
    
    // --- Fin Campos ---

    this.builderForm.SetFormGroup(
      this.fb.group({
        appScreenID: [currentData.appScreenID],
        screen: [currentData.screen],
        parentAppScreenID: [currentData.parentAppScreenID?.toString() || '0'],
        url: [currentData.url],
        sortOrder: [currentData.sortOrder],
        icon: [currentData.icon],
      
        available: [currentData.available]
      })
    );

    this.builderForm.SetSubmitFunction(() => {
      this.SubmitRequests();
    });

    this.builderForm.SetTitle(this.EditAdd() === 'Add' ? 'Add New App Screen' : 'Edit App Screen');
    this.genericForm = this.builderForm.Generate();
  }

  SubmitRequests(): void {
    this.submit.set(true);

    if (this.EditAdd() == 'Add') {
      this.service.createAppScreen(this.dataFormTemp()).subscribe({
        next: (response) => {
          this._message.add({
            severity: 'success', summary: 'Add!',
            detail: `Screen ${this.dataFormTemp().screen} Added`, life: 2000
          });
          this.GetTable(this.selectedStatus); // Recargar tabla
        }
      });
    } else {
      // Sincronizar el ID en caso de que no esté en el form (aunque debería)
      this.dataFormTemp().appScreenID = this.dataForm().appScreenID; 
      
      this.service.updateAppScreen(this.dataFormTemp()).subscribe({
        next: (response) => {
          this._message.add({
            severity: 'success', summary: 'Edit!',
            detail: `Screen ${this.dataFormTemp().screen} Updated`, life: 2000
          });
          this.GetTable(this.selectedStatus); // Recargar tabla
        }
      });
    }
    
    this.displayMaximizable = false; // Cerrar modal
  }

  getModal(item: AppScreenDto = {} as AppScreenDto) {
    this.submit.set(false);

    // Si `item` está vacío o sin ID, es 'Add'
    if (!item.appScreenID) {
      this.EditAdd.set('Add');
      // Usar un objeto limpio
      this.dataForm.set({
        appScreenID: 0,
        parentAppScreenID: null,
        screen: '',
        url: '',
        sortOrder: 0,
        icon: FontAwesomeIcons.faA,
        userID: 0, // O un valor default si lo tienes
        available: true,
        parentScreen: ''
      });
      this.ConfigForm();
      this.displayMaximizable = true;
      
    } else {
      // Es 'Edit'
      this.EditAdd.set('Edit');
    

      let tests: AppScreenDto;
      
      // Llamar al servicio para obtener los datos frescos del item por ID
      this.service.getAppScreenById(item.appScreenID).subscribe({
        next: (data) => {
         this.dataForm.set(data); // Actualizar la señal con los datos
        // 1. Actualizar la señal con los datos frescos
          this.dataForm.set(data);

          // 2. CONSTRUIR el formulario AHORA, usando los datos de la señal
          this.ConfigForm();

          tests = data;
          this.displayMaximizable = true;
         
           // Reconfigurar el formulario con los datos
          
        },
        error: (error) => {
          console.error(error);
          this._message.add({ severity: 'error', summary: 'Error', detail: 'Could not load screen details.'});
        }
      });
    }
  }
  //#endregion
}