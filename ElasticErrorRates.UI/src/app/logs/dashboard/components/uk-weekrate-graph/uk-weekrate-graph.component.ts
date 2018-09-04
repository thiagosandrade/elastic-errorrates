import { Component, OnInit } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { IGraphRequestResponse } from '../../../../_shared/api/dashboard/response/api-graphrequestresponse';
import { WeekDays } from '../../../../_shared/helpers/WeekDays.enum';
import { ChartModel } from '../../../../_shared/helpers/ChartModel';
import { GraphTypeAggregation } from '../../../../_shared/helpers/GraphTypeAggregation.enum';
import { Countries } from '../../../../_shared/helpers/Country.enum';

@Component({
  selector: 'uk-weekrate-graph',
  templateUrl: './uk-weekrate-graph.component.html',
  styleUrls: ['./uk-weekrate-graph.component.css']
})
export class UkWeekRateGraphComponent implements OnInit {

  public dataDailySalesChart: ChartModel;
  public chartName : string;
  public isProcessing: boolean;
  public comparison: any = {  };

  constructor(private apiService: ApiDashboardService) { }

  ngOnInit() {

     /* ----------==========     UKWeekGraphTasksChart initialization    ==========---------- */
    this.chartName = 'UKWeekGraphTasksChart';

    this.dataDailySalesChart = { labels: [], series: []}
    this.isProcessing = true;
    this.fillRate();
  }

  fillRate(){
    
    this.apiService.getGraphValues(Countries.UK, GraphTypeAggregation.Day,"7")
      .then(async (response: IGraphRequestResponse) => {
        var self = this;
        var labelArray: string[] = [];
        var seriesArray: number[] = [];
          
        await response.records.forEach(function(element){
          seriesArray.push(Number(element.errorPercentage))
          labelArray.push(WeekDays[(new Date(element.date)).getDay()])
        });

        self.dataDailySalesChart.series.push(seriesArray.slice().reverse());
        self.dataDailySalesChart.labels = labelArray.reverse();
        
      
        this.comparison.value = (seriesArray[0] / seriesArray[1] * 100) - 100;  
        this.comparison.valueAbsolute = Math.abs(this.comparison.value);

        this.isProcessing = false;
      }
    );
  }
}
