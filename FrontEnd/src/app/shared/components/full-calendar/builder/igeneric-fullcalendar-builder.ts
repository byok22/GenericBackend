import { EventsFullCalendar, FullCalendarInterface } from "../fullcalendar.interface";

export interface IGenericFullCalendarBuilder<T>{
    Reset():void;    
    SetEvent(event:EventsFullCalendar<T>): void;  
    Generate(): FullCalendarInterface<T>;
}