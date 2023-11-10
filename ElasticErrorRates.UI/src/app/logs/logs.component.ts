import { Component, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ChartModule } from 'primeng/chart';
import { AccordionModule } from 'primeng/accordion';
import { AlertService } from '@app/_services';
import { first } from 'rxjs';
import { CalendarModule } from 'primeng/calendar';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LogService } from '@app/_services/log.service';
import { ActivatedRoute, Params, RouterModule } from '@angular/router';

@Component({
    selector: 'app-logs',
    templateUrl: './logs.component.html',
    styleUrls: ['./logs.component.css'],
    imports: [ButtonModule, ChartModule, AccordionModule, CalendarModule, FormsModule, CommonModule, RouterModule ],
    standalone: true
})
export class LogsComponent implements OnInit {
    calendarDateDetails: Date;

    isProcessing: boolean = false;
    gridValues: any;
    totalRecords: Number = 0;

    constructor(private activatedRoute: ActivatedRoute, private logService: LogService, private alertService: AlertService) {
        this.calendarDateDetails = new Date();
     }

    ngOnInit() {
        this.activatedRoute.queryParams.subscribe((params: Params) => {
            if(params['urlBackDate'] != undefined)
                this.dateChangedDetails(new Date(params['urlBackDate']));
        })

    }

    onImport() {
        this.isProcessing = true;
        this.alertService.clear();
        this.alertService.success('Import triggered', true);
        this.logService.import()
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

    dateChangedDetails(term : Date){
        var startDate = new Date(term.getFullYear(), term.getMonth(), term.getDate())
        var endDate = new Date(term.getFullYear(), term.getMonth(), term.getDate(), 23, 59, 59)
        this.fillGrid(startDate, endDate)
    }

    fillGrid(startDate : Date, endDate : Date) {
        this.logService.getLogsAggregate(startDate, endDate).subscribe((response : any) => {
            this.gridValues = response.records;
            this.totalRecords = response.totalRecords;
        })
    }
}

