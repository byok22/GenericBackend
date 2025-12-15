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
import { SiteDto } from '../interfaces/site-dto';
import { SiteCatalogoService } from '../services/site-catalogo.service';
import { DropdownsService } from '../../common/service/dropdowns.service';
import { GenericPageTableMenuForm } from '../../common/interfaces/generic-page-table-menu-form';


//#endregion Imports

//#region  Inits
@Component({
  selector: 'site-page',
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
  templateUrl: './site.page.component.html',
  styleUrl: './site.page.component.css',
  changeDetection: ChangeDetectionStrategy.Default
})
export class SitesPageComponent implements OnInit, GenericPageTableMenuForm<SiteDto> {

  builderTable: GenericTableConcretBuilder<SiteDto>;

  constructor(
    private service : SiteCatalogoService,
    private serviceTable: TableBuilderFactoryService,  
    private fb: FormBuilder,
    private _message: MessageService ,  
  ) {
    this.FillMenu();
    this.ConfigMenu();
    this.builderTable = this.serviceTable.createBuilder<SiteDto>();
  }

  siteDropdown: SelectOption[] = [];

  dataForm: WritableSignal<SiteDto> = signal({
    siteID: 0,
    siteName: '',
    available: false
  });

  dataFormTemp: WritableSignal<SiteDto> = signal({
    siteID: 0,
    siteName: '',
    available: false
  });

  ngOnInit(): void {
    this.showSpinner =true;    
              setTimeout(() => {
                this.GetTable(this.selectedStatus);
    
              }, 1000);
    this.ConfigForm();
  }

  //#region  Vars

  showSpinner:Boolean =false;

  //Menu
  menuItems: GenericMenuInterface[] = [];

  //Table
  tableConfig!: GenericTableConfig<SiteDto>;
  dataTable: SiteDto[] = [];
  hideTable = signal(true);
  public newTable = signal(true);
  public dataSite = signal<SiteDto>({
    siteID: 0,
    siteName: '',
    available: false
  });

  public dataSiteFormTemp: SiteDto = {
    siteID: 0,
    siteName: '',
    available: false
  };

  public SiteTemp = signal<SiteDto>({
    siteID: 0,
    siteName: '',
    available: false
  });

  public dataSites = signal<SiteDto[]>([]);

  public EditAdd = signal<string>('');
  public displayMaximizable: boolean = false;

  //Form
  genericForm: GenericFormInterface<SiteDto> = {
    tittle: '',
    fields: [],
    customFromGroup: undefined,
    editAdd: '',
    data: this.dataSite()
  }

  testForm: GenericFormInterface<SiteDto> = {
    tittle: '',
    fields: [],
    customFromGroup: undefined,
    editAdd: '',
    data: this.SiteTemp()
  };

  builderForm = new GenericFormConcretBuilder<SiteDto>();
  builderTestForm = new GenericFormConcretBuilder<SiteDto>();
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
    this.builderTable.SetTitle("Site Table");
    this.builderTable.SetDataKey("id");
    this.builderTable.SetData(this.dataTable);
    this.builderTable.SetKpis(this.GetKpis());
    this.builderTable.SetPagination(true);
    this.builderTable.SetRowsPerPage(10);
    this.builderTable.SetRowsPerPageOptions([5, 10, 20]);
    this.builderTable.SetColumns(this.getColumns());
    this.builderTable.SetGlobalFilterFields(["siteName"]);
    this.tableConfig = this.builderTable.Generate();
  }

  GetKpis(): BasicKpi[] {
    return [
      { title: "Total", total: this.dataTable.length.toString() },
    ];
  }

  getColumns(): TableColumn[] {
    const manualColumns: TableColumn[] = [
      { field: 'siteID', header: 'ID' },
      { field: 'siteName', header: 'Site Name' },
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
      this.service.GetAllSites().subscribe({
        next: (siteRequest) => {
          if (siteRequest.length < 1) {
            siteRequest = [{
              siteID: 0,
              siteName: 'No site Found',
              available: false
            } as SiteDto];
          }

          const transformedUserRequest = siteRequest.map(request => ({
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
    this.dataSiteFormTemp = this.dataSite();
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
      value: this.dataSite().siteID
    });

    this.builderForm.SetField({
      field: 'siteName',
      label: 'Site Name',
      order: 2,
      required: true,
      type: 'text',
      validationRequired: true,
      enable: true,
      show: true,
      value: this.dataSite().siteName,
      onInputChange: (event: string) => {
        this.dataSiteFormTemp.siteName = event;
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
      value: this.dataSite().available,
      onInputChange: (event: boolean) => {
        this.dataSiteFormTemp.available = event;
      }
    });

    this.builderForm.SetFormGroup(
      this.fb.group({
        siteID: [this.dataSite().siteID],
        siteName: [this.dataSite().siteName],
        available: [this.dataSite().available]
      })
    );

    this.builderForm.SetSubmitFunction(() => {
      this.SubmitRequests();
    });

    this.builderForm.SetTitle('Site Form');
    this.genericForm = this.builderForm.Generate();
  }

  SubmitRequests(): void {
      console.log('Se hizo Submit');
      console.log(this.dataSite());
  
      if (!this.genericForm.customFromGroup || this.genericForm.customFromGroup.invalid) {
        this.genericForm.customFromGroup?.markAllAsTouched();
        return;
      }
      const formValues = this.genericForm.customFromGroup.value;
  
        this.dataSite.set({
          siteID: formValues.siteID,
          siteName: formValues.siteName,
          available: formValues.available
        });
        
      this.submit.set(true);   
  
      console.log('Se hizo Submit');
      console.log(this.dataSite());
  
      if (this.EditAdd() == 'Add') {
        this.service.createSite(this.dataSite()).subscribe({
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
        this.service.updateSite(this.dataSiteFormTemp).subscribe({
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
      this.dataSites = signal<SiteDto[]>([]);
      console.log(this.genericForm.data);
    }

  getModal(item: SiteDto = {} as SiteDto) {
    this.submit.set(false);

    if (item.siteID == 0 || item.siteID == undefined) {
      this.EditAdd.set('Add')
    } else {
      this.EditAdd.set('Edit')
    }

    if (this.EditAdd() == 'Edit') {
      this.dataSite.set(item);
      this.ConfigForm();
      this.displayMaximizable = true;
      let tests: SiteDto;
      this.service.getSiteById(item.siteID).subscribe({
        next: (data) => {
          tests = data;
          this.dataSite = signal<SiteDto>(tests);
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
      const dataSiteTemp: SiteDto = {
        siteID: 0,
        siteName: '',
        available: false
      }

      this.dataSite.set(dataSiteTemp);
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
