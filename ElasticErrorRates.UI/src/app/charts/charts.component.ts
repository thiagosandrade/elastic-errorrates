import { Component, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ChartModule } from 'primeng/chart';
import { AccordionModule } from 'primeng/accordion';
import { AlertService } from '@app/_services';
import { first } from 'rxjs';
import { DashboardService } from '@app/_services/dashboard.service';
import { CalendarModule } from 'primeng/calendar';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LogService } from '@app/_services/log.service';

@Component({
    selector: 'app-charts',
    templateUrl: './charts.component.html',
    styleUrls: ['./charts.component.css'],
    imports: [ButtonModule, ChartModule, AccordionModule, CalendarModule, FormsModule, CommonModule],
    standalone: true
})
export class ChartsComponent implements OnInit {
    calendarDate: Date;

    data: any;
    options: any;
    
    basicData: any;
    basicOptions: any;

    dailyRateUKData: any
    dailyRateROIData: any
    basicOptionsDailyRateUKData: any
    basicOptionsDailyRateROIData: any
    UkNumbers : Number = 0;
    ROINumbers : Number = 0;
    UkNumbersAggregate : Number = 0;
    ROINumbersAggregate : Number = 0;
    
    agreggateData: any;

    isProcessing: boolean = false;
    gridValues: any;
    totalRecords: Number = 0;

    constructor(private dashboardService: DashboardService, private alertService: AlertService) {
        this.calendarDate = new Date();
     }

    ngOnInit() {
        this.verticalBarChart();
        this.getTotalData();
        this.dateChanged(this.calendarDate)
    }

    onImport() {
        this.isProcessing = true;
        this.alertService.clear();
        this.alertService.success('Import triggered', true);
        this.dashboardService.import()
            .pipe(first())
            .subscribe({
                next: () => {
                    this.isProcessing = false;
                    this.alertService.clear();
                },
                error: (error: string) => {
                    this.alertService.error(error);
                    this.isProcessing = false;
                }
            });
    }

    dateChanged(term : Date){
        var startDate = new Date(term.getFullYear(), term.getMonth(), term.getDate())
        var endDate = new Date(term.getFullYear(), term.getMonth(), term.getDate(), 23, 59, 59)
        this.getDailyRate(1, startDate, endDate, false)
        this.getDailyRate(2, startDate, endDate, false)
    }

    getLogsQuantity(startDate : Date, endDate : Date) {
        this.dashboardService.getLogsQuantity(startDate, endDate)
            .pipe(first())
            .subscribe({
                next: (response : Number) => {
                    this.alertService.clear();
                    this.basicData = this.basicChart('Quantity of Logs', ['General'], [response])
                    this.basicOptions = this.generateBasicOptions();
                },
                error: (error: string) => {
                    this.alertService.error(error);
                    this.isProcessing = false;
                }
            })
    }

    getDailyRate(countryId : number, startDate : Date, endDate : Date, total : boolean){
        this.dashboardService.getDailyRate(countryId, startDate, endDate)
            .pipe(first())
            .subscribe({
                next: (response : Number) => {
                    this.alertService.clear();
                    if(countryId == 1){
                        if(total){
                            this.basicOptionsDailyRateUKData = this.generateBasicOptions();
                            this.dailyRateUKData = this.basicChart('Quantity of Logs', ['UK'], [response])
                        }
                        else
                        {
                            this.UkNumbers = response;
                        }
                    }
                    if(countryId == 2){
                        if(total){
                            this.basicOptionsDailyRateROIData = this.generateBasicOptions();
                            this.dailyRateROIData = this.basicChart('Quantity of Logs', ['IRL'], [response])
                        }
                        else
                        {
                            this.ROINumbers = response;
                            this.agreggateData = this.basicChart('Quantity of Logs', ['UK', 'IRL'], [this.UkNumbers, this.ROINumbers ])
                        }
                    }
                },
                error: (error: string) => {
                    this.alertService.error(error);
                    this.isProcessing = false;
                }
            })
    }

    getTotalData(){
        var startDate = new Date(1500, this.calendarDate.getMonth(), this.calendarDate.getDate())
        var endDate = new Date(2030, this.calendarDate.getMonth(), this.calendarDate.getDate(), 23, 59, 59)

        this.getLogsQuantity(startDate, endDate);
        this.getDailyRate(1, startDate, endDate, true)
        this.getDailyRate(2, startDate, endDate, true)
    }

    basicChart(label: string = 'label', labels : string[] = ['Q1', 'Q2', 'Q3', 'Q4'], data: Number[] = [540, 325, 702, 620]) : any {
        return {
            labels: labels,
            datasets: [
                {
                    label: label,
                    data: data,
                    backgroundColor: ['rgba(255, 159, 64, 0.2)', 'rgba(75, 192, 192, 0.2)', 'rgba(54, 162, 235, 0.2)', 'rgba(153, 102, 255, 0.2)'],
                    borderColor: ['rgb(255, 159, 64)', 'rgb(75, 192, 192)', 'rgb(54, 162, 235)', 'rgb(153, 102, 255)'],
                    borderWidth: 1
                }
            ]
        };

    }

    private generateBasicOptions() {
        const documentStyle = getComputedStyle(document.documentElement);
        const textColor = documentStyle.getPropertyValue('--text-color');
        const textColorSecondary = documentStyle.getPropertyValue('--text-color-secondary');
        const surfaceBorder = documentStyle.getPropertyValue('--surface-border');

        return {
            plugins: {
                legend: {
                    labels: {
                        color: textColor
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        color: textColorSecondary
                    },
                    grid: {
                        color: surfaceBorder,
                        drawBorder: false
                    }
                },
                x: {
                    ticks: {
                        color: textColorSecondary
                    },
                    grid: {
                        color: surfaceBorder,
                        drawBorder: false
                    }
                }
            }
        };
    }

    verticalBarChart() {
        const documentStyle = getComputedStyle(document.documentElement);
        const textColor = documentStyle.getPropertyValue('--text-color');
        const textColorSecondary = documentStyle.getPropertyValue('--text-color-secondary');
        const surfaceBorder = documentStyle.getPropertyValue('--surface-border');

        this.data = {
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
            datasets: [
                {
                    label: 'My First dataset',
                    backgroundColor: documentStyle.getPropertyValue('--blue-500'),
                    borderColor: documentStyle.getPropertyValue('--blue-500'),
                    data: [65, 59, 80, 81, 56, 55, 40]
                },
                {
                    label: 'My Second dataset',
                    backgroundColor: documentStyle.getPropertyValue('--pink-500'),
                    borderColor: documentStyle.getPropertyValue('--pink-500'),
                    data: [28, 48, 40, 19, 86, 27, 90]
                }
            ]
        };

        this.options = {
            maintainAspectRatio: false,
            aspectRatio: 0.8,
            plugins: {
                legend: {
                    labels: {
                        color: textColor
                    }
                }
            },
            scales: {
                x: {
                    ticks: {
                        color: textColorSecondary,
                        font: {
                            weight: 500
                        }
                    },
                    grid: {
                        color: surfaceBorder,
                        drawBorder: false
                    }
                },
                y: {
                    ticks: {
                        color: textColorSecondary
                    },
                    grid: {
                        color: surfaceBorder,
                        drawBorder: false
                    }
                }

            }
        };
    }
}

