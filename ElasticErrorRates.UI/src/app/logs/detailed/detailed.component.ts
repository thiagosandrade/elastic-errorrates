import { Component, OnInit } from '@angular/core';
import { ApiLogService } from '../../_shared/api/log/api.service';
import { ILog } from '../../_shared/api/log/model/log';
import { ILogResponse } from '../../_shared/api/log/response/api-logresponse';
import { ActivatedRoute, Params } from '@angular/router';
import { KeepHtmlPipe } from '../../_shared/keepHtmlPipe/keep-html.pipe';


@Component({
  selector: 'logs-detailed',
  styleUrls: ['./detailed.component.css'],
  templateUrl: './detailed.component.html'
})
export class DetailedComponent implements OnInit {
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

  constructor(private apiService: ApiLogService, private activatedRoute: ActivatedRoute) {   }

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe((params: Params) => {
      this.httpUrl = params['httpUrl'];
    });

    this.fillGrid();
  }

  fillGrid() {
    this.isProcessing = true;

    this.apiService.getLogs(this.currentPage, this.pageSize, this.httpUrl).subscribe((logs: ILogResponse) => {
      this.logs = this.logs.concat(logs.records);
      this.totalRecords = logs.totalRecords;
    },
    (err: any) => {
      console.log(err);
    },
    () => {
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
}
