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

    startDate : Date | undefined;
    endDate : Date | undefined;

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
        this.startDate = new Date(term.getFullYear(), term.getMonth(), term.getDate())
        this.endDate = new Date(term.getFullYear(), term.getMonth(), term.getDate(), 23, 59, 59)
        this.fillGrid(this.startDate, this.endDate)
    }

    fillGrid(startDate : Date, endDate : Date) {
        this.logService.getLogsAggregate(startDate, endDate).subscribe((response : any) => {
            this.gridValues = response.records;
            this.totalRecords = response.totalRecords;
        })
    }

    onInputText(event : Event){
        var term = (event.target as HTMLInputElement).value ;
        
        if(term.length > 3){
            this.searchByText(term);
          }
          else{
            if(this.startDate !== undefined && this.endDate !== undefined){
                this.fillGrid(this.startDate, this.endDate);
            }
          }
    }

    searchByText(term : string){

        this.logService.findLogs(this.startDate, this.endDate, "httpUrl", "", term).subscribe((logs : any) => {
            this.gridValues = logs.records;
            this.totalRecords = logs.totalRecords
          },
          (err : any) => {
            console.log(err);
          },
          () => {
            this.isProcessing = false;
          });  
      }
}

