import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FullCalendarModule } from '@fullcalendar/angular';
import { CalendarOptions } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import esLocale from '@fullcalendar/core/locales/es';
import { EventsFullCalendar, FullCalendarInterface } from './fullcalendar.interface';
import { PrimengModule } from '../../modules/primeng.module';


@Component({
  selector: 'full-calendar-app', 
  imports: [
     FullCalendarModule
  ], 
  styleUrl: './full-calendar.component.css',
  templateUrl: './full-calendar.component.html',
  changeDetection: ChangeDetectionStrategy.Default,
})
export class FullCalendarComponent<T> implements OnInit, OnChanges {

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['eventsMain'] ) {
      // Handle changes in options or selected option
      console.log('eventsMain changed:', this.eventsMain);
    }
  }
  @Input()  eventsMain!: FullCalendarInterface<T>;
  @Output() eventSelected = new EventEmitter<EventsFullCalendar<T>>();
  public events:any[]=[];

  display: boolean = false;
  selectedEvent: any;



  ngOnInit(): void {
    this.events = this.eventsMain.events.map((item)=>{
      return{
        title: item.title,
        start: item.start,
        end: item.end,
        description: item.description
      }
    });
  }
  
  public options?: CalendarOptions =
  {
    plugins: [dayGridPlugin, timeGridPlugin,interactionPlugin],     
    initialView: 'dayGridMonth',
    locale: esLocale,
    headerToolbar:{  left:'prev,next',
      center: 'title',
      right:'dayGridMonth,timeGridWeek,timeGridDay,today'
      }
      , 
    weekends: true,
    editable: false,  
    eventClick: this.handleEventClick.bind(this),  
    height:'auto',    
  };
  

  handleEventClick(arg: any) {
    const selectedEvent = this.eventsMain.events.find(e => e.title === arg.event.title);
    if (selectedEvent) {
      this.eventSelected.emit(selectedEvent);
    }
  }
   
}
