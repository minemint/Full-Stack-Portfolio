import { Component } from '@angular/core'

// Ng2 Charts
import { BaseChartDirective } from 'ng2-charts'

// Chart JS
import { ChartConfiguration, ChartOptions, ChartType } from 'chart.js'
import { MatCard, MatCardContent, MatCardHeader, MatCardTitle } from '@angular/material/card'

import { Meta } from '@angular/platform-browser'

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    BaseChartDirective,
    MatCard,
    MatCardHeader,
    MatCardTitle,
    MatCardContent
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {

  title = 'Sales Dashboard'

  constructor(
    private meta: Meta
  ) {}

  ngOnInit() {
    this.meta.addTag({ name: 'description', content: 'Dashboard for Stock Management' })
  }

  // Bar Chart
  public barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
    datasets: [
      {
        label: 'Angular',
        data: [65, 59, 80, 81, 56, 55, 40],
        backgroundColor: 'rgba(255,0,0,0.5)',
        borderColor: 'black',
        borderWidth: 1,
      },
      {
        label: 'React',
        data: [28, 48, 40, 19, 86, 27, 90],
        backgroundColor: 'rgba(0,255,0,0.6)',
        borderColor: 'black',
        borderWidth: 1,
      },
    ],
  }

  public barChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    maintainAspectRatio: false,
  }

  public barChartType: ChartType = 'bar'

  // Line Chart
  public lineChartData: ChartConfiguration<'line'>['data'] = {
    labels: [
      'January',
      'February',
      'March',
      'April',
      'May',
      'June',
      'July',
      'August',
      'September',
      'October',
      'November',
      'December',
    ],

    datasets: [
      {
        data: [40, 45, 50, 55, 60, 65, 70, 75, 70, 60, 50, 45],
        label: 'Angular',
        fill: true,
        tension: 0.5,
        borderColor: 'black',
        backgroundColor: 'rgba(255,0,0,0.6)',
      },
      {
        data: [45, 50, 60, 70, 75, 65, 50, 60, 55, 50, 45, 45],
        label: 'React',
        fill: true,
        tension: 0.5,
        borderColor: 'black',
        backgroundColor: 'rgba(0,255,0,0.5)',
      },
    ],
  }
  public lineChartOptions: ChartOptions<'line'> = {
    responsive: true,
    maintainAspectRatio: false,
  }
  public lineChartLegend = true

  // Doughnut Chart
  public doughnutChartData: ChartConfiguration<'doughnut'>['data'] = {
    labels: ['Angular', 'React', 'Vue'],
    datasets: [
      {
        data: [300, 500, 100],
        backgroundColor: ['rgba(255,0,0,0.6)', 'rgba(0,255,0,0.6)', 'rgba(0,0,255,0.3)'],
        borderColor: 'black',
        borderWidth: 1,
      },
    ],
  }

  public doughnutChartOptions: ChartOptions<'doughnut'> = {
    responsive: true,
    maintainAspectRatio: false,
  }

  // Scatter Chart
  public scatterChartData: ChartConfiguration<'scatter'>['data'] = {
    datasets: [
      {
        label: 'Angular',
        data: [
          { x: 1, y: 2 },
          { x: 2, y: 3 },
          { x: 3, y: 4 },
          { x: 4, y: 5 },
          { x: 5, y: 6 },
        ],
        backgroundColor: 'rgba(255,0,0,0.6)',
        borderColor: 'black',
        borderWidth: 2,
      },
      {
        label: 'React',
        data: [
          { x: 1, y: 1 },
          { x: 2, y: 2 },
          { x: 3, y: 3 },
          { x: 4, y: 4 },
          { x: 5, y: 5 },
        ],
        backgroundColor: 'rgba(0,255,0,0.6)',
        borderColor: 'black',
        borderWidth: 2,
      },
    ],
  }

  public scatterChartOptions: ChartOptions<'scatter'> = {
    responsive: true,
    maintainAspectRatio: false,
  }

}