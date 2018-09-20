import { Component, OnInit, Input, SimpleChange, SimpleChanges } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { IDailyRate, DailyRate } from '../../../../_shared/api/dashboard/model/dailyrate';
import { IDailyRateResponse } from '../../../../_shared/api/dashboard/response/api-dailyrateresponse';

@Component({
  selector: 'roi-dailyrate',
  templateUrl: './roi-dailyrate.component.html',
  styleUrls: ['./roi-dailyrate.component.css']
})
export class ROIDailyRateComponent {
  public roiDailyRate: IDailyRate = new DailyRate();
  public isProcessing: boolean;
  public titleDate: Date;

  @Input() datePickerChanged: Date;
  
  constructor(private apiService: ApiDashboardService) { }

  ngOnChanges(changes: SimpleChanges) {
    const datePickerChanged: SimpleChange = changes.datePickerChanged;
    
    if((datePickerChanged.previousValue != datePickerChanged.currentValue) && datePickerChanged.currentValue != undefined ){
      this.setDates(datePickerChanged.currentValue)
    }
    
  }

  setDates(newDate : Date){

    this.roiDailyRate.startDate.setDate(newDate.getDate() - 1);
    this.roiDailyRate.endDate.setDate(newDate.getDate());

    this.titleDate = this.roiDailyRate.endDate;

    this.fillROIDailyRate();
  }

  fillROIDailyRate(){

    this.roiDailyRate.countryId = 2;
    this.roiDailyRate.errorPercentage = 0;
    this.isProcessing = true;

    this.apiService.getDailyRate(this.roiDailyRate.countryId, this.roiDailyRate.startDate, this.roiDailyRate.endDate)
      .then(async (response: IDailyRateResponse) => {
        this.roiDailyRate = response.records[0];
        this.isProcessing = false;
      }
    );
  }
}
