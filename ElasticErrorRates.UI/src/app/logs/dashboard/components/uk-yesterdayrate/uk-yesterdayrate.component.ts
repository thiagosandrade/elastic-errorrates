import { Component, OnInit } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { IDailyRate, DailyRate } from '../../../../_shared/api/dashboard/model/dailyrate';
import { IDailyRateResponse } from '../../../../_shared/api/dashboard/response/api-dailyrateresponse';
import { Countries } from '../../../../_shared/helpers/Country.enum';

@Component({
  selector: 'uk-yesterdayrate',
  templateUrl: './uk-yesterdayrate.component.html',
  styleUrls: ['./uk-yesterdayrate.component.css']
})
export class UkYesterdayRateComponent implements OnInit {
  titleDate: any;
  public ukYesterdayRate: IDailyRate = new DailyRate();
  public isProcessing: boolean;
  constructor(private apiService: ApiDashboardService) { }

  ngOnInit() {

    this.ukYesterdayRate.CountryId = Countries.UK;
    this.ukYesterdayRate.StartDate = new Date();
    this.ukYesterdayRate.StartDate.setDate(this.ukYesterdayRate.StartDate.getDate() - 2);

    this.ukYesterdayRate.EndDate = new Date();
    this.ukYesterdayRate.EndDate.setDate(this.ukYesterdayRate.EndDate.getDate() - 1);

    this.titleDate = this.ukYesterdayRate.EndDate;

    this.ukYesterdayRate.ErrorPercentage = 0;
    this.isProcessing = true;
    this.fillUKDailyRate();
  }

  fillUKDailyRate(){
    this.apiService.getDailyRate(this.ukYesterdayRate.CountryId, this.ukYesterdayRate.StartDate, this.ukYesterdayRate.EndDate).subscribe((response: IDailyRateResponse) => {
      this.ukYesterdayRate = response.records[0];
      this.isProcessing = false;
    });
  }
}
