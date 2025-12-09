import { signal, Type } from "@angular/core";

import { GenericMenuInterface } from "../../shared/components/generic-menu-item/interfaces/generic-menu-item.interface";
import { GenericTableConcretBuilder } from "../../shared/components/generic-table/builder/generic-table-concret-builder";
import { GenericTableConfig } from "../../shared/components/generic-table/interfaces/generic-table-config";
import { TableColumn } from "../../shared/components/generic-table/interfaces/table-column";
import { GenericFormInterface } from "../../shared/components/generic-form/generic-form.interface";
import { GenericFormConcretBuilder } from "../../shared/components/generic-form/builder/generic-form-concret-builder";
import { BasicKpi } from "../../shared/interfaces/basic-kpi.interface";

/**
 * Interface for a generic page that includes a menu, table, and form.
 */
export interface GenericPageTableMenuForm<T> {
  // Table

  /**
   * Builder for the table, containing the configuration.
   */
  builderTable: GenericTableConcretBuilder<any>;

  // Menu

  /**
   * Array containing the items that make up the menus.
   */
  menuItems: GenericMenuInterface[];

  // Table

  /**
   * Generic configuration for the table.
   *
   * @type {GenericTableConfig<T>}
   *
   * @example
   * // Example initialization for a User type
   * tableConfig!: GenericTableConfig<User>;
   */
  tableConfig: GenericTableConfig<any>;

  /**
   * Contains the data for the table.
   *
   * @type {T[]}
   *
   * @example
   * // Example initialization
   * dataTable: T[] = [];
   */
  dataTable: any[];

  /**
   * Indicates whether the table is new and needs columns to be generated or if it already exists and only the data is modified.
   *
   * @type {ReturnType<typeof signal<boolean>>}
   *
   * @example
   * // Example initialization
   * public newTable = signal(true);
   */
  newTable: ReturnType<typeof signal<boolean>>;

  /**
   * Signal to hide the table.
   *
   * @type {ReturnType<typeof signal<boolean>>}
   *
   * @example
   * // Example initialization
   * public hideTable = signal(true);
   */
  hideTable: ReturnType<typeof signal<boolean>>;

  // Form

  /**
   * Contains a writable signal with the information of a form record.
   *
   * @type {ReturnType<typeof signal<T>>}
   *
   * The generic type `T` can represent any type that corresponds to the form data structure.
   * For instance, if `T = User`, the initialization might look like:
   *
   * @example
   * // Example initialization when T is User
   * public dataForm = signal<User>({
   *   id: 1,
   *   employeeAccount: '',
   *   userName: '',
   *   email: ''
   * });
   */
  dataForm: ReturnType<typeof signal<T>>;

  /**
   * Includes a writable signal with temporary information for storing form data.
   *
   * @type {ReturnType<typeof signal<T>>}
   */
  dataFormTemp: ReturnType<typeof signal<T>>;

  /**
   * Indicates whether the form is in edit or add mode.
   *
   * Values: 'Edit' or 'Add'
   *
   * @type {ReturnType<typeof signal<string>>}
   *
   * @example
   * // Example initialization
   * public EditAdd = signal<string>('');
   */
  EditAdd: ReturnType<typeof signal<string>>;

  /**
   * Determines whether the form is displayed maximized.
   *
   * @type {boolean}
   *
   * @example
   * // Example initialization
   * public displayMaximizable: boolean = false;
   */
  displayMaximizable: boolean;

  /**
   * Generic form interface.
   *
   * @type {GenericFormInterface<T>}
   *
   * @example
   * // Example initialization
   * genericForm: GenericFormInterface<Test> = {
   *   title: '',
   *   fields: [],
   *   customFormGroup: undefined,
   *   editAdd: '',
   *   data: this.dataForm()
   * };
   */
  genericForm: GenericFormInterface<T>;

  /**
   * Builder for the generic form, which configures the form and automatically adds fields and data.
   *
   * @type {GenericFormConcretBuilder<T>}
   *
   * @example
   * // Example initialization with your entity type
   * builderForm = new GenericFormConcretBuilder<T>();
   */
  builderForm: GenericFormConcretBuilder<T>;

  /**
   * Signal to know if the form is submitted.
   *
   * @type {ReturnType<typeof signal<boolean>>}
   *
   * @example
   * // Example initialization
   * public submit = signal(false);
   */
  submit: ReturnType<typeof signal<boolean>>;

  // Methods

   /**
   * Configures the menu, adding dropdowns, selects, submit buttons, and defaults.
   * @example
   * ConfigMenu(): void {
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
        order: 5,
        type: 'button'
      });

      this.menuItems = builder.Generate();
    }
   * 
   */
    ConfigMenu(): void;

  /**
   * Fills the menu elements such as selects/dropdowns.
   * @example
   *  FillMenu(): void {
        this.userService.getProfiles().subscribe({
          next: (status) => {
            this.statusDD = status;
            this.selectedStatus = status[0]?.id || '1'; // Asegúrate de que selectedBuilding tiene un valor válido
            this.statusItem.item.options = this.statusDD;
            this.statusItem.item.selectedOption = this.selectedStatus;
          }
        });
      }
   */
  FillMenu(): void;

 

  /**
   * Configures the table that displays the information.
   *
   * @param config - The configuration for columns, data, and KPIs.
   * 
   * @example
   * this.builderTable.Reset();
      this.builderTable.SetTitle("Users Table");
      this.builderTable.SetDataKey("id");
      this.builderTable.SetData(this.dataTable);
      this.builderTable.SetKpis(this.GetKpis());
      this.builderTable.SetPagination(true);
      this.builderTable.SetRowsPerPage(10);
      this.builderTable.SetRowsPerPageOptions([5, 10, 20]);
      this.builderTable.SetColumns(this.getColumns());
      this.builderTable.SetGlobalFilterFields(["employeeAccount", "userName"]);
      //builder.SetGroupBy("category");
      // builder.Generate();

      this.tableConfig = this.builderTable.Generate();
   */
  ConfigTable(config: GenericTableConfig<T>): void;

  /**
   * Retrieves the KPIs to be displayed above the table.
   * 
   * @example
   *  GetKpis(): BasicKpi[] {

      return [
        { title: "Total", total: this.dataTable.length.toString() },
      ];
    }
   */
  GetKpis():  BasicKpi[];

  /**
   * Retrieves the columns for the table.
   *
   * @returns {TableColumn[]} The columns of the table.
   * 
   * @example
   * 
   * getColumns(): TableColumn[] {
    // Definir columnas manualmente
    const manualColumns: TableColumn[] = [
      { field: 'id', header: 'ID' },

    ];

    // Obtener campos de datos
    const data = this.dataTable;
    const columnFields = Object.keys(data[0]);

    // Eliminar campos que ya están definidos manualmente
    const manualFields = manualColumns.map(col => col.field);
    const filteredColumnFields = columnFields.filter(field => !manualFields.includes(field));

    // Crear columnas a partir de los campos de datos restantes
    const dataColumns: TableColumn[] = filteredColumnFields.map(field => ({
      field,
      header: this.capitalizeFirstLetter(field)
    }));

    // Concatenar columnas manuales con columnas de datos
    let columns: TableColumn[] = [...manualColumns, ...dataColumns];

    // Campos para ocultar
    const fieldsToHide = [""];

    // Agregar propiedad showHeader para ocultar columnas
    columns = columns.map(column => ({
      ...column,
      showHeader: !fieldsToHide.includes(column.field)
    }));

    return columns;
  }
   */
  getColumns(): TableColumn[];

  /**
   * Obtains the table with various arguments.
   *
   * @param {...any[]} args - Arguments for obtaining the table.
   */
  GetTable(...args: any[]): void;

  /**
   * Configures the form by adding elements using the builder.
   * 
   * @example
   * async ConfigForm() {         

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
              value: this.dataForm().id
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
              value: this.dataForm().userName
            });
            

            this.builderForm.SetFormGroup(
              this.fb.group({
                id: [0],
                userName: '',                
              })
            );
            this.builderForm.SetSubmitFunction(
              () => {

                this.SubmitRequests();
              }
            );
            this.builderForm.SetTitle('Users');
            this.genericForm = this.builderForm.Generate();

          }
   */
  ConfigForm(): void;

  /**
   * Submits or updates the form data.
   */
  SubmitRequests(): void;

  /**
   * Displays a modal for the given item.
   *
   * @param {T} item - The item for which to display the modal.
   */
  getModal(item: any): void;
}
