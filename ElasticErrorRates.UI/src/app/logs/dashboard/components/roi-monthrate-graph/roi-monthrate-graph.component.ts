import { Component, OnInit, Input, SimpleChanges, SimpleChange } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api-dashboard.service';
import { ChartModel } from '../../../../_shared/helpers/ChartModel';
import { GraphTypeAggregation } from '../../../../_shared/helpers/GraphTypeAggregation.enum';
import { IGraphRequestResponse } from '../../../../_shared/api/dashboard/response/api-graphrequestresponse';
import { Countries } from '../../../../_shared/helpers/Country.enum';

@Component({
  selector: 'roi-monthrate-graph',
  templateUrl: './roi-monthrate-graph.component.html',
  styleUrls: ['./roi-monthrate-graph.component.css']
})
export class ROIMonthRateGraphComponent {

  public datawebsiteViewsChart: ChartModel;
  public chartName : string;
  public isProcessing: boolean;
  public comparison: any = {  };
  
  @Input() datePickerChanged: Date;
  
  constructor(private apiService: ApiDashboardService) { }

  ngOnChanges(changes: SimpleChanges) {
    const datePickerChanged: SimpleChange = changes.datePickerChanged;
    
    if((datePickerChanged.previousValue != datePickerChanged.currentValue) && datePickerChanged.currentValue != undefined ){
      this.fillRate(datePickerChanged.currentValue)
    }
    
  }

  fillRate(newDate : Date){
    
    /* ----------==========     ROIMonthRateChart Chart initialization    ==========---------- */
    this.chartName = 'ROIMonthRateChart';

    this.datawebsiteViewsChart = {
      labels: ['J', 'F', 'M', 'A', 'M', 'J', 'J', 'A', 'S', 'O', 'N', 'D'],
      series: [
      ]
    };

    this.isProcessing = true;

    this.apiService.getGraphValues(Countries.ROI, GraphTypeAggregation.Month, `${(newDate.getMonth() +1)}`, newDate)
      .then(async (response: IGraphRequestResponse) => {
        var self = this;
        var seriesArray: number[] = [];
          
        response.records.forEach(function(element){
          seriesArray.push(Number(element.errorPercentage))
        });

        self.datawebsiteViewsChart.series.push(seriesArray.slice().reverse());

        this.comparison.value = !Number.isNaN(seriesArray[0] / seriesArray[1]) ? (seriesArray[0] / seriesArray[1] * 100) - 100 : 0;  
        this.comparison.valueAbsolute = Math.abs(this.comparison.value);

        this.isProcessing = false;
      }
    );
  }
}
