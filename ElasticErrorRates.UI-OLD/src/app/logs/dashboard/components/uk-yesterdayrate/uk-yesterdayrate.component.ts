import { Component, OnInit, Input, SimpleChanges, SimpleChange } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api-dashboard.service';
import { IDailyRate, DailyRate } from '../../../../_shared/api/dashboard/model/dailyrate';
import { IDailyRateResponse } from '../../../../_shared/api/dashboard/response/api-dailyrateresponse';
import { Countries } from '../../../../_shared/helpers/Country.enum';

@Component({
  selector: 'uk-yesterdayrate',
  templateUrl: './uk-yesterdayrate.component.html',
  styleUrls: ['./uk-yesterdayrate.component.css']
})
export class UkYesterdayRateComponent {
  public ukYesterdayRate: IDailyRate = new DailyRate();
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

    this.ukYesterdayRate.startDate.setDate(newDate.getDate() - 2);
    this.ukYesterdayRate.endDate.setDate(newDate.getDate() - 1);

    this.titleDate = this.ukYesterdayRate.endDate;

    this.fillUKDailyRate();
  }

  fillUKDailyRate(){

    this.ukYesterdayRate.countryId = Countries.UK;
    this.ukYesterdayRate.errorPercentage = 0;
    this.isProcessing = true;

    this.apiService.getDailyRate(this.ukYesterdayRate.countryId, this.ukYesterdayRate.startDate, this.ukYesterdayRate.endDate)
      .then(async (response: IDailyRateResponse) => {
        this.ukYesterdayRate = response.records[0];
        this.isProcessing = false;
      }
    );
  }
}
