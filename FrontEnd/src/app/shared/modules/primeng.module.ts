import { NgModule } from '@angular/core';
import { TableModule } from 'primeng/table';
import { PaginatorModule } from 'primeng/paginator';
import { MultiSelectModule } from 'primeng/multiselect';
import { ProgressBarModule } from 'primeng/progressbar';
import { TagModule } from 'primeng/tag';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { MessagesModule } from 'primeng/messages';
import {  OverlayPanelModule } from 'primeng/overlaypanel';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { PanelModule } from 'primeng/panel';
import { MenuModule } from 'primeng/menu';
import { TieredMenuModule } from 'primeng/tieredmenu';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ToastModule } from 'primeng/toast';


@NgModule({
  exports: [   
    MultiSelectModule, 
    PaginatorModule,
    TableModule,
    ProgressBarModule,
    TagModule,
    ButtonModule,
    DialogModule, 
    CalendarModule,
    CheckboxModule ,
    MultiSelectModule,
    MessagesModule,
    OverlayPanelModule,
    CardModule,
    InputTextModule,
    PanelModule,
    MenuModule ,
    TieredMenuModule, 
    ProgressSpinnerModule,
    ToastModule,
     
    

 
       
  ]
})
export class PrimengModule { }
