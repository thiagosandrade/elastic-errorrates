import { Component, OnInit, Input, SimpleChanges, SimpleChange } from '@angular/core';
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
export class UkMonthRateGraphComponent {

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

    /* ----------==========     UKMonthRateChart Chart initialization    ==========---------- */
    this.chartName = 'UKMonthRateChart';

    this.datawebsiteViewsChart = {
      labels: ['J', 'F', 'M', 'A', 'M', 'J', 'J', 'A', 'S', 'O', 'N', 'D'],
      series: [
      ]
    };

    this.isProcessing = true;
    
    this.apiService.getGraphValues(Countries.UK, GraphTypeAggregation.Month, `${(newDate.getMonth() +1)}`, newDate)
      .then(async (response: IGraphRequestResponse) => {
        var self = this;
        var seriesArray: number[] = [];
          
        response.records.forEach(function(element){
          seriesArray.push(Number(element.errorPercentage))
        });

        self.datawebsiteViewsChart.series.push(seriesArray.slice().reverse());
        
        this.comparison.value = (seriesArray[0] / seriesArray[1] * 100) - 100;  
        this.comparison.valueAbsolute = Math.abs(this.comparison.value);

        this.isProcessing = false;
      }
    );
  }
}
