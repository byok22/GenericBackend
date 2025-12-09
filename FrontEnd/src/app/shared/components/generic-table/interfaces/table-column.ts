export interface TableColumn {
    field: string;
    header: string;
    showHeader?: boolean;
    showSortIcon?: boolean;
    showColumnFilter?: boolean;
    type?:string;
    cellClass?:string;
    genericButton?: string;
 }
