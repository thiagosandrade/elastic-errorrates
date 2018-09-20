import { Component, Input, SimpleChanges, SimpleChange } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { IDailyRate, DailyRate } from '../../../../_shared/api/dashboard/model/dailyrate';
import { IDailyRateResponse } from '../../../../_shared/api/dashboard/response/api-dailyrateresponse';
import { Countries } from '../../../../_shared/helpers/Country.enum';

@Component({
  selector: 'uk-dailyrate',
  templateUrl: './uk-dailyrate.component.html',
  styleUrls: ['./uk-dailyrate.component.css']
})
export class UkDailyRateComponent {
  public ukDailyRate: IDailyRate = new DailyRate();
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

    this.ukDailyRate.startDate.setDate(newDate.getDate() - 1);
    this.ukDailyRate.endDate.setDate(newDate.getDate());

    this.titleDate = this.ukDailyRate.endDate;

    this.fillUKDailyRate();
  }

  fillUKDailyRate(){

    this.ukDailyRate.countryId = Countries.UK;
    this.ukDailyRate.errorPercentage = 0;
    this.isProcessing = true;

    this.apiService.getDailyRate(this.ukDailyRate.countryId, this.ukDailyRate.startDate, this.ukDailyRate.endDate)
      .then(async (response: IDailyRateResponse) => {
        this.ukDailyRate = response.records[0];
        this.isProcessing = false;
      }
    );
  }

  
}
