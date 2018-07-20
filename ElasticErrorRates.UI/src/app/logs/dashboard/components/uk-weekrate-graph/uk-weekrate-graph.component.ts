import { Component, OnInit } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { IGraphRequestResponse } from '../../../../_shared/api/dashboard/response/api-graphrequestresponse';
import { WeekDays } from '../../../../_shared/helpers/WeekDays.enum';
import { ChartModel } from '../../../../_shared/helpers/ChartModel';
import { GraphTypeAggregation } from '../../../../_shared/helpers/GraphTypeAggregation.enum';

@Component({
  selector: 'uk-weekrate-graph',
  templateUrl: './uk-weekrate-graph.component.html',
  styleUrls: ['./uk-weekrate-graph.component.css']
})
export class UkWeekRateGraphComponent implements OnInit {

  public dataDailySalesChart: ChartModel;
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
    
    this.apiService.getGraphValues(GraphTypeAggregation.Day,"7").subscribe((response: IGraphRequestResponse) => {
     var self = this;
     var labelArray: string[] = [];
     var seriesArray: number[] = [];
      
      response.records.forEach(function(element){
        seriesArray.push(Number(element.errorPercentage))
        labelArray.push(WeekDays[(new Date(element.date)).getDay()])
      });

      self.dataDailySalesChart.series.push(seriesArray.reverse());
      self.dataDailySalesChart.labels = labelArray.reverse();

      this.isProcessing = false;
    });
  }
}
