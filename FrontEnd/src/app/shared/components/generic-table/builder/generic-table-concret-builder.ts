import { BasicKpi } from "../../../interfaces/basic-kpi.interface";
import { GenericTableConfig } from "../interfaces/generic-table-config";
import { TableColumn } from "../interfaces/table-column";
import { IBuilderGenericTable } from "./builder-generic-table.interface";


export class GenericTableConcretBuilder<T> implements IBuilderGenericTable<T> {
    _genericTable!: GenericTableConfig<T>;

    constructor() {
        this.Reset();
    }

    Reset = (): void => {
        this._genericTable = {
            title: "",
            dataKey: "",
            data: [],
            kpi: [],
            pagination: false,
            rowsPerPage: 10,
            rowsPerPageOptions: [],
            columns: [],
            globalFilterFields: [],
            groupby: ""
        };
    };

    SetTitle = (title: string): void => {
        this._genericTable.title = title;
    };

    SetDataKey = (dataKey: string): void => {
        this._genericTable.dataKey = dataKey;
    };


    /**
     * Array del tipo <T> que contrendra la tabla
     * @param data 
     */
    SetData = (data: T[]): void => {
        this._genericTable.data = data;
    };


    /**
     * Mostrara los KPIS en la tabla
     * @param kpi :BasicKpi[] - Array de Kpis 
     */
    SetKpis = (kpi: BasicKpi[]): void => {
        this._genericTable.kpi = kpi;
    };

    /**
     * Paginar o mostrart todo
     * @param pagination : boolean
     */

    SetPagination = (pagination: boolean): void => {
        this._genericTable.pagination = pagination;
    };
    /**
     * Cantidad de Rows que contendra la pagina
     * @param rowsPerPage 
     */
    SetRowsPerPage = (rowsPerPage: number): void => {
        this._genericTable.rowsPerPage = rowsPerPage;
    };

    /**
     * Asigna los Rows que se mostraran por pagina 
     * @param rowsPerPageOptions -- Arreglo de enteros 
     * @example [5, 10, 20]
     */
    SetRowsPerPageOptions = (rowsPerPageOptions: number[]): void => {
        this._genericTable.rowsPerPageOptions = rowsPerPageOptions;
    };
    
    /**
     * Asigna las columnas que se mostraran en la tabla, estas son las mismas que se importaran
     * @param columns -TableColumn[]  arreglo de columnas 
     */
    SetColumns = (columns: TableColumn[]): void => {
        this._genericTable.columns = columns;
    };

    /**
     * Agrega los campos por los cual permitira filtrar la tabla
     * @param globalFilterFields 
     */
    SetGlobalFilterFields = (globalFilterFields: string[]): void => {
        this._genericTable.globalFilterFields = globalFilterFields;
    };

    SetGroupBy = (groupby: string): void => {
        this._genericTable.groupby = groupby;
    };

    Generate = (): GenericTableConfig<T> => {
        const result = this._genericTable;
        this.Reset();
        return result;
    };
    
    GetTable = (): GenericTableConfig<T> => {
        return this._genericTable;
    };
}
