import { EventsFullCalendar, FullCalendarInterface } from "../fullcalendar.interface";
import { IGenericFullCalendarBuilder } from "./igeneric-fullcalendar-builder";

export class GenericFullCalendarConcretBuilder<T> implements IGenericFullCalendarBuilder<T>
{
    private _genericFullCalendar!: FullCalendarInterface<T>
    Reset(): void {
        this._genericFullCalendar ={
            events:[]
        };
    }
    SetEvent(event: EventsFullCalendar<T>): void {
        this._genericFullCalendar.events.push(event)
    }
    Generate(): FullCalendarInterface<T> {
        return this._genericFullCalendar;
    }

}