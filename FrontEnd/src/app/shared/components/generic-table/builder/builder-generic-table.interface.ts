import { BasicKpi } from "../../../interfaces/basic-kpi.interface";
import { TableColumn } from "../interfaces/table-column";

export interface IBuilderGenericTable<T>{

    Reset(): void;
    SetTitle(title: string): void;
    SetDataKey(dataKey: string): void;
    SetData(data: T[]): void;
    SetKpis(kpi: BasicKpi[]): void;
    SetPagination(pagination: boolean): void;
    SetRowsPerPage(rowsPerPage: number): void;
    SetRowsPerPageOptions(rowsPerPageOptions:number[]): void;
    SetColumns(columns:TableColumn[]): void;
    SetGlobalFilterFields(globalFilterFields:string[]): void;
    SetGroupBy(groupby:string):void;
    Generate(): void;

}