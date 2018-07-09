import { Component, OnInit } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { IDailyRate, DailyRate } from '../../../../_shared/api/dashboard/model/dailyrate';
import { IDailyRateResponse } from '../../../../_shared/api/dashboard/response/api-dailyrateresponse';

@Component({
  selector: 'uk-dailyrate',
  templateUrl: './uk-dailyrate.component.html',
  styleUrls: ['./uk-dailyrate.component.css']
})
export class UkDailyRateComponent implements OnInit {
  public ukDailyRate: IDailyRate = new DailyRate();

  constructor(private apiService: ApiDashboardService) { }

  ngOnInit() {

    this.ukDailyRate.CountryId = 1;
    this.ukDailyRate.StartDate = new Date();
    this.ukDailyRate.StartDate.setDate(this.ukDailyRate.StartDate.getDate() - 2);
    this.ukDailyRate.EndDate = new Date();
    this.ukDailyRate.ErrorPercentage = 0;

    this.fillUKDailyRate();
  }

  fillUKDailyRate(){
    this.apiService.getDailyRate(this.ukDailyRate.CountryId, this.ukDailyRate.StartDate, this.ukDailyRate.EndDate).subscribe((response: IDailyRateResponse) => {
      this.ukDailyRate = response.records[0];
    });
  }
}
