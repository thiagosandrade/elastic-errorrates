import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../_shared/api/api.service';
import { ILog } from '../../_shared/api/model/log';
import { ILogResponse } from '../../_shared/api/response/api-logresponse';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'logs-detailed',
  templateUrl: './detailed.component.html',
  styleUrls: ['./detailed.component.css']
})
export class DetailedComponent implements OnInit {
  public logs: ILog[];
  public totalRecords: number;
  public isProcessing: boolean; 
  public term: string;
  public sort: string = "false";
  public match: string = "false";
  public displayModal = false;
  public selectedLog;
  public httpUrl : string;

  constructor(private apiService: ApiService, private activatedRoute: ActivatedRoute) {   }

  ngOnInit(){
    this.activatedRoute.queryParams.subscribe((params: Params) => {
      this.httpUrl = params['httpUrl'];
    });

    this.fillGrid();
  }

  fillGrid() {
    this.isProcessing = true;
    
    this.apiService.getLogs(this.httpUrl).subscribe((logs : ILogResponse) => {
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

  onSelectLog(logId : string){
    this.displayModal = true;
    this.selectedLog = logId;
  }

  onImport(){
    this.apiService.importLogs().subscribe(
      result => this.fillGrid(),
      error => console.log(error)
    );
  }
  
}
