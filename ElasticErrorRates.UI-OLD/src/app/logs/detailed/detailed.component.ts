import { Component, OnInit, SimpleChanges, SimpleChange, OnDestroy } from '@angular/core';
import { ApiLogService } from '../../_shared/api/log/api-log.service';
import { ILog } from '../../_shared/api/log/model/log';
import { ILogResponse } from '../../_shared/api/log/response/api-logresponse';
import { ActivatedRoute, Params } from '@angular/router';
import { DatePickerService } from '../../_shared/datepicker-material/datePicker.service';

@Component({
  selector: 'logs-detailed',
  styleUrls: ['./detailed.component.css'],
  templateUrl: './detailed.component.html'
})
export class DetailedComponent implements OnInit, OnDestroy {
  public logs: ILog[] = [];
  public totalRecords = 0;
  public isProcessing: boolean;
  public term: string;
  public displayModal = false;
  public selectedLog;
  public httpUrl : string;
  public currentPage = 0;
  public pageSize = 50;
  public columnField = "exception";
  public filterStartDate: Date = new Date();
  public filterEndDate: Date = new Date();

  public subscription : any;

  constructor(private apiService: ApiLogService, private activatedRoute: ActivatedRoute, private datePickerService : DatePickerService) {   }

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe((params: Params) => {
      this.httpUrl = params['httpUrl'];
    });

    this.getValues();
  }

  getValues() : void {
    this.subscription = this.datePickerService.getDateValue()
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

    this.apiService.getLogs(this.filterStartDate, this.filterEndDate, this.currentPage, this.pageSize, this.httpUrl)
      .then(async (logs: ILogResponse) => {
        this.logs = this.logs.concat(logs.records);
        this.totalRecords = logs.totalRecords;

        this.isProcessing = false;
    });
  }

  onInputText(term : string){
    if(term.length > 3){
      this.searchByText(term);
    }
    else{
      this.fillGrid();
    }
  }

  onSelectLog(logId : string){
    this.displayModal = true;
    this.selectedLog = logId;
  }

  fetchMoreData() {
    this.currentPage++;
    this.fillGrid();
  }

  modalEventChanges(event){
    this.selectedLog = event;
  }

  searchByText(term : string){
    this.term = (term != null && term != undefined && term != "") ? term : 'null'
      this.apiService.findLogs(this.columnField, this.httpUrl,this.term).subscribe((logs : ILogResponse) => {
        this.logs = logs.records;
        this.totalRecords = logs.totalRecords

        console.log(logs);
      },
      (err : any) => {
        console.log(err);
      },
      () => {
        this.isProcessing = false;
      });  
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
