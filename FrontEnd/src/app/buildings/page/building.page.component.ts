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
import { MessageService } from 'primeng/api';

import { GenericTableConcretBuilder } from '../../shared/components/generic-table/builder/generic-table-concret-builder';
import { BuildingDto } from '../interfaces/building-dto';
import { BuildingCatalogoService } from '../services/building-catalogo.service';
import { DropdownsService } from '../../common/service/dropdowns.service';
import { GenericPageTableMenuForm } from '../../common/interfaces/generic-page-table-menu-form';
import { SiteCatalogoService } from '../../sites/services/site-catalogo.service';


//#endregion Imports

//#region  Inits
@Component({
  selector: 'building-page',
  standalone: true,
  imports: [
    CommonModule, 
    GenericTableComponent, 
    HttpClientModule, 
    GenericTitleComponent, 
    GenericFormComponent, 
    PrimengModule, 
    FontAwesomeModule
  ],
  providers: [DatePipe, DropdownsService, MessageService],
  templateUrl: './building.page.component.html',
  styleUrl: './building.page.component.css',
  changeDetection: ChangeDetectionStrategy.Default
})
export class BuildingsPageComponent implements OnInit, GenericPageTableMenuForm<BuildingDto> {

  builderTable: GenericTableConcretBuilder<BuildingDto>;

  constructor(
    private service : BuildingCatalogoService,
    private serviceTable: TableBuilderFactoryService,  
    private siteService: SiteCatalogoService,
    private fb: FormBuilder,
    private _message: MessageService ,  
  ) {
    this.FillMenu();
    this.ConfigMenu();
      this.GetDropdowns();
    this.builderTable = this.serviceTable.createBuilder<BuildingDto>();
  }

  buildingDropdown: SelectOption[] = [];

  dataForm: WritableSignal<BuildingDto> = signal({
    buildingID: 0,
    name: '',
    description: '',
    siteID: 0,
    available: false
  });

  dataFormTemp: WritableSignal<BuildingDto> = signal({
    buildingID: 0,
    name: '',
    description: '',
    siteID: 0,
    available: false
  });

  ngOnInit(): void {
    this.showSpinner =true;    
              setTimeout(() => {
                this.GetTable(this.selectedStatus);
    
              }, 1000);
    this.ConfigForm();
  }

  GetDropdowns() {
    this.siteService.GetAllSites().subscribe({
      next: (site) => {

        this.siteDropdown = site.map((site) => ({
          id: site.siteID.toString(),
          text: site.siteName
        }));
      }
    });
  }

  //#region  Vars

  showSpinner:Boolean =false;

  //Menu
  menuItems: GenericMenuInterface[] = [];

  //Table
  tableConfig!: GenericTableConfig<BuildingDto>;
  dataTable: BuildingDto[] = [];
  hideTable = signal(true);
  public newTable = signal(true);
  public dataBuilding = signal<BuildingDto>({
    buildingID: 0,
    name: '',
    description: '',
    siteID: 0,
    available: false
  });

  public dataBuildingFormTemp: BuildingDto = {
    buildingID: 0,
    name: '',
    description: '',
    siteID: 0,
    available: false
  };

  public BuildingTemp = signal<BuildingDto>({
    buildingID: 0,
    name: '',
    description: '',
    siteID: 0,
    available: false
  });

  public dataBuildings = signal<BuildingDto[]>([]);

  public EditAdd = signal<string>('');
  public displayMaximizable: boolean = false;

  //Form
  genericForm: GenericFormInterface<BuildingDto> = {
    tittle: '',
    fields: [],
    customFromGroup: undefined,
    editAdd: '',
    data: this.dataBuilding()
  }

  testForm: GenericFormInterface<BuildingDto> = {
    tittle: '',
    fields: [],
    customFromGroup: undefined,
    editAdd: '',
    data: this.BuildingTemp()
  };

  builderForm = new GenericFormConcretBuilder<BuildingDto>();

  statuses: SelectOption[] = this.getEnumSelectOptions(GenericStatus);
  public submit = signal(false);

  siteDropdown: SelectOption[] = [];

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
    this.builderTable.SetTitle("Building Table");
    this.builderTable.SetDataKey("id");
    this.builderTable.SetData(this.dataTable);
    this.builderTable.SetKpis(this.GetKpis());
    this.builderTable.SetPagination(true);
    this.builderTable.SetRowsPerPage(10);
    this.builderTable.SetRowsPerPageOptions([5, 10, 20]);
    this.builderTable.SetColumns(this.getColumns());
    this.builderTable.SetGlobalFilterFields(["name", "description"]);
    this.tableConfig = this.builderTable.Generate();
  }

  GetKpis(): BasicKpi[] {
    return [
      { title: "Total", total: this.dataTable.length.toString() },
    ];
  }

  getColumns(): TableColumn[] {
    const manualColumns: TableColumn[] = [
      { field: 'buildingID', header: 'ID' },
      { field: 'name', header: 'Name' },
      { field: 'description', header: 'Description' },
      { field: 'siteID', header: 'Site ID' },
      { field: 'available', header: 'Available' }
    ];

    const data = this.dataTable;
    const columnFields = Object.keys(data[0] || {});

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
      this.service.GetAllBuildings().subscribe({
        next: (buildingRequest) => {
          if (buildingRequest.length < 1) {
            buildingRequest = [{
              buildingID: 0,
              name: 'No building Found',
              description: '',
              siteID: 0,
              available: false
            } as BuildingDto];
          }

          const transformedUserRequest = buildingRequest.map(request => ({
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
    this.dataBuildingFormTemp = this.dataBuilding();
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
      value: this.dataBuilding().buildingID
    });

    this.builderForm.SetField({
      field: 'name',
      label: 'Building Name',
      order: 2,
      required: true,
      type: 'text',
      validationRequired: true,
      enable: true,
      show: true,
      value: this.dataBuilding().name,
      onInputChange: (event: string) => {
        this.dataBuildingFormTemp.name = event;
      }
    });

    this.builderForm.SetField({
      field: 'description',
      label: 'Description',
      order: 3,
      required: false,
      type: 'textArea',
      validationRequired: false,
      enable: true,
      show: true,
      value: this.dataBuilding().description,
      onInputChange: (event: string) => {
        this.dataBuildingFormTemp.description = event;
      }
    });

    const selectSite = this.siteDropdown.find(site => site.text === this.dataBuilding().siteID.toString() || site.id === this.dataBuilding().siteID.toString());

    this.builderForm.SetField({
      field: 'siteID',
      label: 'Site ID',
      order: 4,
      required: true,
      type: 'select',
      options: this.siteDropdown,
      validationRequired: true,
      enable: true,
      show: true,
      value: selectSite?.id,
      onInputChange: (event: string) => {

        const selectSite2 = this.siteDropdown.find(site => site.id == event);
        
         this.dataBuildingFormTemp.siteID = Number(selectSite2.id);  // Guardar el ID directamente en el objeto
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
      value: this.dataBuilding().available,
      onInputChange: (event: boolean) => {
        this.dataBuildingFormTemp.available = event;
      }
    });

    this.builderForm.SetFormGroup(
      this.fb.group({
        buildingID: [this.dataBuilding().buildingID],
        name: [this.dataBuilding().name],
        description: [this.dataBuilding().description],
        siteID: [this.dataBuilding().siteID],
        available: [this.dataBuilding().available]
      })
    );

    this.builderForm.SetSubmitFunction(() => {
      this.SubmitRequests();
    });

    this.builderForm.SetTitle('Building Form');
    this.genericForm = this.builderForm.Generate();
  }

  SubmitRequests(): void {
      console.log('Se hizo Submit');
      console.log(this.dataBuilding());
  
      if (!this.genericForm.customFromGroup || this.genericForm.customFromGroup.invalid) {
        this.genericForm.customFromGroup?.markAllAsTouched();
        return;
      }
      const formValues = this.genericForm.customFromGroup.value;
  
        this.dataBuilding.set({
          buildingID: formValues.buildingID,
          name: formValues.name,
          description: formValues.description,
          siteID: formValues.siteID,
          available: formValues.available
        });
        
      this.submit.set(true);   
  
      console.log('Se hizo Submit');
      console.log(this.dataBuilding());
  
      if (this.EditAdd() == 'Add') {
        this.service.createBuilding(this.dataBuilding()).subscribe({
          next: (response) => {
            this._message.add({
              severity: 'success',
              summary: 'Add!',
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
        this.service.updateBuilding(this.dataBuildingFormTemp).subscribe({
          next: (response) => {
            this._message.add({
              severity: 'success',
              summary: 'Edit!',
              life: 2000
            });
            this.GetTable(this.selectedStatus);
          },
          error: () => {},
          complete: () => {}
        });
      }
  
      this.displayMaximizable = false;
      this.dataBuildings = signal<BuildingDto[]>([]);
      console.log(this.genericForm.data);
    }

  getModal(item: BuildingDto = {} as BuildingDto) {
    this.submit.set(false);

    if (item.buildingID == 0 || item.buildingID == undefined) {
      this.EditAdd.set('Add')
    } else {
      this.EditAdd.set('Edit')
    }

    if (this.EditAdd() == 'Edit') {
      this.dataBuilding.set(item);
      this.ConfigForm();
      this.displayMaximizable = true;
      let tests: BuildingDto;
      this.service.getBuildingById(item.buildingID).subscribe({
        next: (data) => {
          tests = data;
          this.dataBuilding = signal<BuildingDto>(tests);
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
      const dataBuildingTemp: BuildingDto = {
        buildingID: 0,
        name: '',
        description: '',
        siteID: 0,
        available: false
      }

      this.dataBuilding.set(dataBuildingTemp);
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
