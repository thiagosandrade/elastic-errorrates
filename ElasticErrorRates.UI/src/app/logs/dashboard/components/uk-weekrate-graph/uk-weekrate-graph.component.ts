import { Component, OnInit } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { IGraphRequestResponse } from '../../../../_shared/api/dashboard/response/api-graphrequestresponse';

@Component({
  selector: 'uk-weekrate-graph',
  templateUrl: './uk-weekrate-graph.component.html',
  styleUrls: ['./uk-weekrate-graph.component.css']
})
export class UkWeekRateGraphComponent implements OnInit {

  public dataDailySalesChart: { labels: string[]; series: number[][]; };
  public chartName : string;
  public isProcessing: boolean;

  constructor(private apiService: ApiDashboardService) { }

  ngOnInit() {

     /* ----------==========     UKWeekGraphTasksChart initialization    ==========---------- */
    this.chartName = 'UKWeekGraphTasksChart';

    this.dataDailySalesChart = { labels: [], series: []}
    this.isProcessing = true;
    this.fillRate();

  }

  fillRate(){
    
    var weekdays = new Array(7);
      weekdays[0] = "M";
      weekdays[1] = "T";
      weekdays[2] = "W";
      weekdays[3] = "T";
      weekdays[4] = "F";
      weekdays[5] = "S";
      weekdays[6] = "S";


    this.apiService.getGraphValues("3","7").subscribe((response: IGraphRequestResponse) => {
     var self = this;
     var labelArray: string[] = [];
     var seriesArray: number[] = [];

      
      response.records.forEach(function(element){
        seriesArray.push(Number(element.errorPercentage))
        labelArray.push(weekdays[(new Date(element.date)).getDay()])
      });

      self.dataDailySalesChart.series.push(seriesArray.reverse());
      self.dataDailySalesChart.labels = labelArray.reverse();

      this.isProcessing = false;
    });
  }
}
