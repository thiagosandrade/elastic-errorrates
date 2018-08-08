import { Component, OnInit } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { ChartModel } from '../../../../_shared/helpers/ChartModel';
import { GraphTypeAggregation } from '../../../../_shared/helpers/GraphTypeAggregation.enum';
import { IGraphRequestResponse } from '../../../../_shared/api/dashboard/response/api-graphrequestresponse';
import { Countries } from '../../../../_shared/helpers/Country.enum';

@Component({
  selector: 'roi-monthrate-graph',
  templateUrl: './roi-monthrate-graph.component.html',
  styleUrls: ['./roi-monthrate-graph.component.css']
})
export class ROIMonthRateGraphComponent implements OnInit {

  public datawebsiteViewsChart: ChartModel;
  public chartName : string;
  public isProcessing: boolean;

  constructor(private apiService: ApiDashboardService) { }

  ngOnInit() {

     /* ----------==========     ROIMonthRateChart Chart initialization    ==========---------- */
    this.chartName = 'ROIMonthRateChart';

    this.datawebsiteViewsChart = {
      labels: ['J', 'F', 'M', 'A', 'M', 'J', 'J', 'A', 'S', 'O', 'N', 'D'],
      series: [
      ]
    };

    this.isProcessing = true;
    this.fillRate();
  }

  fillRate(){
    
    this.apiService.getGraphValues(Countries.ROI, GraphTypeAggregation.Month, `${((new Date()).getMonth() +1)}`).subscribe((response: IGraphRequestResponse) => {
     var self = this;
     var seriesArray: number[] = [];
      
      response.records.forEach(function(element){
        seriesArray.push(Number(element.errorPercentage))
      });

      self.datawebsiteViewsChart.series.push(seriesArray.reverse());

      this.isProcessing = false;
    });
  }
}
