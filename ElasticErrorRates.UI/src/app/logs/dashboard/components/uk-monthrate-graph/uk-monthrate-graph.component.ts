import { Component, OnInit } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { ChartModel } from '../../../../_shared/helpers/ChartModel';
import { GraphTypeAggregation } from '../../../../_shared/helpers/GraphTypeAggregation.enum';
import { IGraphRequestResponse } from '../../../../_shared/api/dashboard/response/api-graphrequestresponse';
import { Countries } from '../../../../_shared/helpers/Country.enum';

@Component({
  selector: 'uk-monthrate-graph',
  templateUrl: './uk-monthrate-graph.component.html',
  styleUrls: ['./uk-monthrate-graph.component.css']
})
export class UkMonthRateGraphComponent implements OnInit {

  public datawebsiteViewsChart: ChartModel;
  public chartName : string;
  public isProcessing: boolean;
  public comparison: any = {  };

  constructor(private apiService: ApiDashboardService) { }

  ngOnInit() {

     /* ----------==========     UKMonthRateChart Chart initialization    ==========---------- */
    this.chartName = 'UKMonthRateChart';

    this.datawebsiteViewsChart = {
      labels: ['J', 'F', 'M', 'A', 'M', 'J', 'J', 'A', 'S', 'O', 'N', 'D'],
      series: [
      ]
    };

    this.isProcessing = true;
    this.fillRate();
  }

  fillRate(){
    
    this.apiService.getGraphValues(Countries.UK, GraphTypeAggregation.Month, `${((new Date()).getMonth() +1)}`).subscribe((response: IGraphRequestResponse) => {
     var self = this;
     var seriesArray: number[] = [];
      
      response.records.forEach(function(element){
        seriesArray.push(Number(element.errorPercentage))
      });

      self.datawebsiteViewsChart.series.push(seriesArray.slice().reverse());
      
      console.log(seriesArray);
      this.comparison.value = (seriesArray[0] / seriesArray[1] * 100) - 100;  
      this.comparison.valueAbsolute = Math.abs(this.comparison.value);

      this.isProcessing = false;
    });
  }
}
