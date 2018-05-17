import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../_shared/api/api.service';
import { ILog } from '../../_shared/api/model/log';
import { ILogResponse } from '../../_shared/api/response/api-logresponse';
import { ILogSummaryResponse } from '../../_shared/api/response/api-logsummaryresponse';
import { ILogSummary } from '../../_shared/api/model/logsummary';

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
  public sort: string = "false";
  public match: string = "false";
  public displayModal = false;
  public selectedLog;

  constructor(private apiService: ApiService) { 
    this.fillGrid();
  }

  fillGrid() {
    this.isProcessing = true;
    
    this.apiService.getLogsAggregate().subscribe((logs: ILogSummaryResponse) => {
      this.logs = logs.records;
      this.totalRecords = logs.totalRecords;
    },
    (err : any) => {
      console.log(err);
    },
    () => {
      this.isProcessing = false;
    });
    
  }

  onInputText(term : string){
    this.term = (term != null && term != undefined && term != "") ? term : "null"
    this.apiService.findLogs("null",this.term).subscribe((logs : ILogResponse) => {
      //  this.logs = logs.records;
      //  this.totalRecords = logs.totalRecords
    },
    (err : any) => {
      console.log(err);
    },
    () => {
      this.isProcessing = false;
    });
  }

  onSelectLog(logId : string){
    this.displayModal = true;
    this.selectedLog = logId;
  }

  onImport() {
    this.isImporting = true;
    this.apiService.importLogs().subscribe(
      result => this.fillGrid(),
      error => console.log(error),
      () => this.isImporting = false
    );
  }
}
