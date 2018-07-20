import { Component, OnInit } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { IDailyRate, DailyRate } from '../../../../_shared/api/dashboard/model/dailyrate';
import { IDailyRateResponse } from '../../../../_shared/api/dashboard/response/api-dailyrateresponse';

@Component({
  selector: 'roi-dailyrate',
  templateUrl: './roi-dailyrate.component.html',
  styleUrls: ['./roi-dailyrate.component.css']
})
export class ROIDailyRateComponent implements OnInit {
  titleDate: Date;
  public roiDailyRate: IDailyRate = new DailyRate();
  public isProcessing: boolean;

  constructor(private apiService: ApiDashboardService) { }

  ngOnInit() {

    this.roiDailyRate.CountryId = 2;
    this.roiDailyRate.StartDate = new Date();
    this.roiDailyRate.StartDate.setDate(this.roiDailyRate.StartDate.getDate() - 1);

    this.roiDailyRate.EndDate = new Date();
    this.roiDailyRate.EndDate.setDate(this.roiDailyRate.EndDate.getDate());

    this.titleDate = this.roiDailyRate.EndDate;

    this.roiDailyRate.ErrorPercentage = 0;
    this.isProcessing = true;

    this.fillUKDailyRate();
  }

  fillUKDailyRate(){
    this.apiService.getDailyRate(this.roiDailyRate.CountryId, this.roiDailyRate.StartDate, this.roiDailyRate.EndDate).subscribe((response: IDailyRateResponse) => {
      this.roiDailyRate = response.records[0];
      this.isProcessing = false;
    });
  }
}
