import { BasicKpi } from "../../../interfaces/basic-kpi.interface"
import { TableColumn } from "./table-column"
import { TableFilter } from "./table-filter"

export interface GenericTableConfig<T> {
    title?:string;
    dataKey?: string;
    data: T[];
    kpi?: BasicKpi[];
    pagination:boolean;
    rowsPerPage: number;
    rowsPerPageOptions:number[];
    columns:TableColumn[];
    globalFilterFields?:string[];
    groupby?:string;
 }
