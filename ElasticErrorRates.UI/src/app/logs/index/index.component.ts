import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../_shared/api/api.service';
import { ILog } from '../../_shared/api/model/log';
import { ILogResponse } from '../../_shared/api/response/api-logresponse';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent {
  public logs: ILog[];
  public totalRecords: number;
  public isProcessing: boolean; 
  public term: string;
  public sort: string = "false";
  public match: string = "false";

  constructor(private apiService: ApiService) { 
    this.fillGrid();
  }

  fillGrid() {
    this.isProcessing = true;
    
    this.apiService.getLogs().subscribe((logs : ILogResponse) => {
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

  onInputText(term : string){
    this.term = (term != null && term != undefined && term != "") ? term : "null"
    this.apiService.findLogs(this.term, this.sort, this.match).subscribe((logs : ILogResponse) => {
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
  
}
