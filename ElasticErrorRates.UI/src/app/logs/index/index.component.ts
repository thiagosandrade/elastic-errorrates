import { Component, OnInit } from '@angular/core';
import { ApiLogService } from '../../_shared/api/log/api.service';
import { ILog } from '../../_shared/api/log/model/log';
import { ILogResponse } from '../../_shared/api/log/response/api-logresponse';
import { ILogSummaryResponse } from '../../_shared/api/log/response/api-logsummaryresponse';
import { ILogSummary } from '../../_shared/api/log/model/logsummary';

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

  constructor(private apiService: ApiLogService) { 
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
    this.apiService.findLogsSummary(this.columnField,"null",this.term).subscribe((logs : ILogSummaryResponse) => {
        this.logs = logs.records;
        this.totalRecords = logs.totalRecords
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
