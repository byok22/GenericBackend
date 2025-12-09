export interface FullCalendarInterface<T>{    
    events:EventsFullCalendar<T>[];
}

export interface EventsFullCalendar<T>{
    title:string,
    start: Date,
    end?: Date,
    description: string ,
    color: string,  
    data?:T 
}