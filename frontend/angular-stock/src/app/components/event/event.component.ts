import { Component, OnInit, inject } from '@angular/core'
import { FullCalendarModule } from '@fullcalendar/angular'
import dayGridPlugin from '@fullcalendar/daygrid'
import interactionPlugin from '@fullcalendar/interaction'
import timeGridPlugin from '@fullcalendar/timegrid'
import { CalendarOptions } from '@fullcalendar/core'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { LOCALE_ID } from '@angular/core'
import localeTh from '@angular/common/locales/th'
import { registerLocaleData } from '@angular/common'
import { MatCard, MatCardContent, MatCardHeader, MatCardTitle } from '@angular/material/card'
import { MatDialog, MatDialogModule } from '@angular/material/dialog'
import { environment } from '../../../environments/environment'
import { EventDialogComponent } from '../event-dialog/event-dialog.component'

registerLocaleData(localeTh)

@Component({
  selector: 'app-event',
  standalone: true,
  imports: [
    FullCalendarModule,
    MatCard,
    MatCardHeader,
    MatCardTitle,
    MatCardContent,
    MatDialogModule
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'th-TH' }
  ],
  templateUrl: './event.component.html',
  styleUrl: './event.component.scss'
})
export class EventComponent implements OnInit {

  private http = inject(HttpClient)
  private apiURL = environment.dotnet_api_url
  private dialog = inject(MatDialog)

  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin, interactionPlugin, timeGridPlugin],
    locale: 'th',
    buttonText: {
      today: 'วันนี้',
      month: 'เดือน',
      week: 'สัปดาห์',
      day: 'วัน'
    },
    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek,timeGridDay'
    },
    events: [],
    eventTimeFormat: { // Add this to customize the time format
      hour: '2-digit',
      minute: '2-digit',
      meridiem: false,
      hour12: false
    },
    eventClick: this.handleEventClick.bind(this) // Bind event click handler
  }

  // Header
  httpOptions  = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    })
  }

  ngOnInit(): void {
    this.loadEvents()
  }

  loadEvents() {
    this.http.get(this.apiURL + 'Event', this.httpOptions).subscribe({
      next: (data: any) => {
        console.log(data)
        this.calendarOptions.events = data.map((event: any) => {
          return {
            title: event.title,
            start: event.start,
            end: event.end
          }
        })
      },
      error: error => {
        console.error('There was an error!', error)
      }
    })
  }

  handleEventClick(arg: any) {
    this.dialog.open(EventDialogComponent, {
      data: {
        title: arg.event.title,
        start: arg.event.start,
        end: arg.event.end
      }
    })
  }

}
