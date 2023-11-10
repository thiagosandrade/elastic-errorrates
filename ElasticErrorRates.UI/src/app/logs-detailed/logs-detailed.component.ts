import { Component, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ChartModule } from 'primeng/chart';
import { AccordionModule } from 'primeng/accordion';
import { CalendarModule } from 'primeng/calendar';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LogService } from '@app/_services/log.service';
import { ActivatedRoute, Params, RouterModule } from '@angular/router';
import { KeepHtmlPipe } from '@app/_pipes/keep-html.pipe';

@Component({
    selector: 'app-logs-detailed',
    templateUrl: './logs-detailed.component.html',
    styleUrls: ['./logs-detailed.component.css'],
    imports: [ButtonModule, ChartModule, AccordionModule, CalendarModule, FormsModule, CommonModule, RouterModule, KeepHtmlPipe],
    standalone: true
})
export class LogsDetailedComponent implements OnInit {
    isProcessing: boolean = false;
    gridValues: any;
    totalRecords: Number = 0;

    httpUrl : string = '';
    startDate : Date | undefined;
    endDate : Date | undefined;

    urlBackdate: Date | undefined;

    constructor(private activatedRoute: ActivatedRoute, private logService: LogService ) {
    }

    ngOnInit() {
        this.activatedRoute.queryParams.subscribe((params: Params) => {
            this.httpUrl = params['httpUrl'];
            this.startDate = params['startDate'];
            this.endDate = params['endDate'];

            if(this.startDate !== undefined && this.endDate !== undefined){
                this.urlBackdate = new Date(this.startDate)
                this.fillGrid(this.startDate, this.endDate, this.httpUrl);
            }
          });
      
    }

    fillGrid(startDate : Date, endDate : Date, httpUrl : string) {
        this.logService.getLogsDetailed(startDate, endDate, httpUrl).subscribe((response : any) => {
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
                this.fillGrid(this.startDate, this.endDate, this.httpUrl);
            }
          }
    }

    searchByText(term : string){

        this.logService.findLogs(this.startDate, this.endDate, "exception", this.httpUrl, term).subscribe((logs : any) => {
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

