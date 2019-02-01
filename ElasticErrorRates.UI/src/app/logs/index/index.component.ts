import { Component, OnInit, Input, SimpleChanges, SimpleChange } from '@angular/core';
import { ApiLogService } from '../../_shared/api/log/api.service';
import { ILogSummaryResponse } from '../../_shared/api/log/response/api-logsummaryresponse';
import { ILogSummary } from '../../_shared/api/log/model/logsummary';
import { DatePickerService } from '../../_shared/datepicker-material/datePicker.service';

@Component({
  selector: 'logs-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent {
  public logs: ILogSummary[] = [];
  public totalRecords = 0;
  public isProcessing: boolean;
  public isImporting: boolean;
  public term: string;
  public displayModal = false;
  public selectedLog;
  public columnField = "httpUrl";
  public filterStartDate: Date = new Date();
  public filterEndDate: Date = new Date();

  @Input() datePickerChanged: Date;

  constructor(private apiService: ApiLogService, private datePickerService : DatePickerService) {   }

  ngOnInit() {
    this.getValues();
  }

  getValues() : void {
    this.datePickerService.getDateValue()
    .subscribe(
      (res : Date) => {
        this.setDates(res);
      }
    );

  }

  ngOnChanges(changes: SimpleChanges) {
    const datePickerChanged: SimpleChange = changes.datePickerChanged;
    
    if((datePickerChanged.previousValue != datePickerChanged.currentValue) && datePickerChanged.currentValue != undefined ){
      this.setDates(datePickerChanged.currentValue)
    }
    
  }

  setDates(newDate : Date){

    this.filterStartDate.setDate(newDate.getDate() - 1);
    this.filterEndDate.setDate(newDate.getDate());

    this.fillGrid();
  }

  fillGrid() {
      this.isProcessing = true;
      this.logs = null;
      this.totalRecords = null;
      this.apiService.getLogsAggregate(this.filterStartDate, this.filterEndDate)
        .then(async (logs: ILogSummaryResponse) => {
        this.logs = logs.records;
        this.totalRecords = logs.totalRecords;
        this.isProcessing = false;
        }
      );
  }

  onInputText(term : string){
    this.term = (term != null && term != undefined) ? term : "null"
    if(term !== "null"){
      this.apiService.findLogsSummary(this.columnField,"null",this.term, this.filterStartDate, this.filterEndDate)
        .then(async (logs : ILogSummaryResponse) => {
          this.logs = logs.records;
          this.totalRecords = logs.totalRecords
          this.isProcessing = false;
        }
      );
      
    }
    else{
      this.fillGrid();
    }
    
  }

  onSelectLog(logId : string){
    this.displayModal = true;
    this.selectedLog = logId;
  }
}
