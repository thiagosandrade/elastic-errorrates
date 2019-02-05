import { Component, OnInit, Input, SimpleChanges, SimpleChange } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api-dashboard.service';
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
export class UkWeekRateGraphComponent {

  public dataDailySalesChart: ChartModel;
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

    /* ----------==========     UKWeekGraphTasksChart initialization    ==========---------- */
    this.chartName = 'UKWeekGraphTasksChart';

    this.dataDailySalesChart = { labels: [], series: []}
    this.isProcessing = true;
    
    this.apiService.getGraphValues(Countries.UK, GraphTypeAggregation.Day,"7", newDate)
      .then(async (response: IGraphRequestResponse) => {
        var self = this;
        var labelArray: string[] = [];
        var seriesArray: number[] = [];
          
        await response.records.forEach(function(element)
        {
          seriesArray.push(Number(element.errorPercentage))
          labelArray.push(WeekDays[(new Date(element.date)).getDay()])
        });

        self.dataDailySalesChart.series.push(seriesArray.slice().reverse());
        self.dataDailySalesChart.labels = labelArray.reverse();
      
        this.comparison.value = !Number.isNaN(seriesArray[0] / seriesArray[1]) ? (seriesArray[0] / seriesArray[1] * 100) - 100 : 0;  
        this.comparison.valueAbsolute = Math.abs(this.comparison.value);

        this.isProcessing = false;
      }
    );
  }
}
