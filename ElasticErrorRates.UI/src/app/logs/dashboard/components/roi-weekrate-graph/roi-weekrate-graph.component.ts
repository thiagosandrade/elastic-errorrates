import { Component, OnInit } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { IGraphRequestResponse } from '../../../../_shared/api/dashboard/response/api-graphrequestresponse';
import { WeekDays } from '../../../../_shared/helpers/WeekDays.enum';
import { ChartModel } from '../../../../_shared/helpers/ChartModel';
import { GraphTypeAggregation } from '../../../../_shared/helpers/GraphTypeAggregation.enum';
import { Countries } from '../../../../_shared/helpers/Country.enum';

@Component({
  selector: 'roi-weekrate-graph',
  templateUrl: './roi-weekrate-graph.component.html',
  styleUrls: ['./roi-weekrate-graph.component.css']
})
export class ROIWeekrateGraphComponent implements OnInit {

  public dataDailySalesChart: ChartModel;
  public chartName : string;
  public isProcessing: boolean;
  public comparison: any = {  };
  
  constructor(private apiService: ApiDashboardService) { }

  ngOnInit() {

      /* ----------==========     ROIWeekGraphTasksChart Chart initialization    ==========---------- */
      this.chartName = 'ROIWeekGraphTasksChart';

      this.dataDailySalesChart = { labels: [], series: []}
        this.isProcessing = true;
        this.fillRate();
  }

  fillRate(){
    this.apiService.getGraphValues(Countries.ROI, GraphTypeAggregation.Day,"7")
      .then(async (response: IGraphRequestResponse) => {
        var self = this;
        var labelArray: string[] = [];
        var seriesArray: number[] = [];

        response.records.forEach(function(element)
        {
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
