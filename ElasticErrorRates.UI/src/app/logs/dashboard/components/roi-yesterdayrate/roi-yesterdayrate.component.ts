import { Component, OnInit } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { IDailyRate, DailyRate } from '../../../../_shared/api/dashboard/model/dailyrate';
import { IDailyRateResponse } from '../../../../_shared/api/dashboard/response/api-dailyrateresponse';

@Component({
  selector: 'roi-yesterdayrate',
  templateUrl: './roi-yesterdayrate.component.html',
  styleUrls: ['./roi-yesterdayrate.component.css']
})
export class ROIYesterdayRateComponent implements OnInit {
  titleDate: Date;
  public roiYesterdayRate: IDailyRate = new DailyRate();
  public isProcessing: boolean;
  constructor(private apiService: ApiDashboardService) { }

  ngOnInit() {

    this.roiYesterdayRate.CountryId = 2;
    this.roiYesterdayRate.StartDate = new Date();
    this.roiYesterdayRate.StartDate.setDate(this.roiYesterdayRate.StartDate.getDate() - 3);

    this.roiYesterdayRate.EndDate = new Date();
    this.roiYesterdayRate.EndDate.setDate(this.roiYesterdayRate.EndDate.getDate() - 2);

    this.titleDate = this.roiYesterdayRate.EndDate;

    this.roiYesterdayRate.ErrorPercentage = 0;
    this.isProcessing = true;
    this.fillUKDailyRate();
  }

  fillUKDailyRate(){
    this.apiService.getDailyRate(this.roiYesterdayRate.CountryId, this.roiYesterdayRate.StartDate, this.roiYesterdayRate.EndDate).subscribe((response: IDailyRateResponse) => {
      this.roiYesterdayRate = response.records[0];
      this.isProcessing = false;
    });
  }
}
