import { Component, OnInit, Input, SimpleChanges, SimpleChange } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api-dashboard.service';
import { IDailyRate, DailyRate } from '../../../../_shared/api/dashboard/model/dailyrate';
import { IDailyRateResponse } from '../../../../_shared/api/dashboard/response/api-dailyrateresponse';
import { Countries } from '../../../../_shared/helpers/Country.enum';

@Component({
  selector: 'roi-yesterdayrate',
  templateUrl: './roi-yesterdayrate.component.html',
  styleUrls: ['./roi-yesterdayrate.component.css']
})
export class ROIYesterdayRateComponent {
  public roiYesterdayRate: IDailyRate = new DailyRate();
  public isProcessing: boolean;
  public   titleDate: Date;

  @Input() datePickerChanged: Date;

  constructor(private apiService: ApiDashboardService) { }

  ngOnChanges(changes: SimpleChanges) {
    const datePickerChanged: SimpleChange = changes.datePickerChanged;
    
    if((datePickerChanged.previousValue != datePickerChanged.currentValue) && datePickerChanged.currentValue != undefined ){
      this.setDates(datePickerChanged.currentValue)
    }
    
  }

  setDates(newDate : Date){

    this.roiYesterdayRate.startDate.setDate(newDate.getDate() - 2);
    this.roiYesterdayRate.endDate.setDate(newDate.getDate() - 1);

    this.titleDate = this.roiYesterdayRate.endDate;

    this.fillROIYesterdayDailyRate();
  }

  fillROIYesterdayDailyRate(){

    this.roiYesterdayRate.countryId = Countries.ROI;
    this.roiYesterdayRate.errorPercentage = 0;
    this.isProcessing = true;
    
    this.apiService.getDailyRate(this.roiYesterdayRate.countryId, this.roiYesterdayRate.startDate, this.roiYesterdayRate.endDate)
      .then(async (response: IDailyRateResponse) => {
        this.roiYesterdayRate = response.records[0];
        this.isProcessing = false;
      }
    );
  }
}
