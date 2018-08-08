import { Component, OnInit } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { IDailyRate, DailyRate } from '../../../../_shared/api/dashboard/model/dailyrate';
import { IDailyRateResponse } from '../../../../_shared/api/dashboard/response/api-dailyrateresponse';
import { Countries } from '../../../../_shared/helpers/Country.enum';

@Component({
  selector: 'uk-dailyrate',
  templateUrl: './uk-dailyrate.component.html',
  styleUrls: ['./uk-dailyrate.component.css']
})
export class UkDailyRateComponent implements OnInit {
  public ukDailyRate: IDailyRate = new DailyRate();
  public isProcessing: boolean;
  public titleDate: Date;
  constructor(private apiService: ApiDashboardService) { }

  ngOnInit() {

    this.ukDailyRate.CountryId = Countries.UK;
    this.ukDailyRate.StartDate = new Date();
    this.ukDailyRate.StartDate.setDate(this.ukDailyRate.StartDate.getDate() - 1);

    this.ukDailyRate.EndDate = new Date();
    this.ukDailyRate.EndDate.setDate(this.ukDailyRate.EndDate.getDate());

    this.titleDate = this.ukDailyRate.EndDate;

    this.ukDailyRate.ErrorPercentage = 0;
    this.isProcessing = true;
    this.fillUKDailyRate();
  }

  fillUKDailyRate(){
    this.apiService.getDailyRate(this.ukDailyRate.CountryId, this.ukDailyRate.StartDate, this.ukDailyRate.EndDate).subscribe((response: IDailyRateResponse) => {
      this.ukDailyRate = response.records[0];
      this.isProcessing = false;
    });
  }
}
