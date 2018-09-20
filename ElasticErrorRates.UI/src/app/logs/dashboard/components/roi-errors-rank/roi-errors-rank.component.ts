import { Component, OnInit, Input, SimpleChanges, SimpleChange } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { IError } from '../../../../_shared/api/dashboard/model/error';
import { Countries } from '../../../../_shared/helpers/Country.enum';
import { IErrorsRankResponse } from '../../../../_shared/api/dashboard/response/api-errorsrankresponse';

@Component({
  selector: 'roi-errors-rank',
  templateUrl: './roi-errors-rank.component.html',
  styleUrls: ['./roi-errors-rank.component.css']
})
export class ROIErrorsRankComponent {
  public errors: IError[];
  public errorRankStartDate: Date;
  public errorRankEndDate: Date;
  private countryId: number = Countries.ROI;
  public isProcessing: boolean;

  @Input() datePickerChanged: Date;
  public titleDate: Date;
  
  constructor(private apiService: ApiDashboardService) { }

  ngOnChanges(changes: SimpleChanges) {
    const datePickerChanged: SimpleChange = changes.datePickerChanged;
    
    if((datePickerChanged.previousValue != datePickerChanged.currentValue) && datePickerChanged.currentValue != undefined ){
      this.setDates(datePickerChanged.currentValue)
    }
    
  }

  setDates(newDate : Date){

    this.errorRankStartDate = new Date();
    this.errorRankEndDate = new Date();
    this.errorRankStartDate.setDate(newDate.getDate() - 1);
    this.errorRankEndDate.setDate(newDate.getDate());
    this.titleDate = this.errorRankEndDate;

    this.fetchErrorRank();
  }

  fetchErrorRank(){

    this.isProcessing = true;

    this.apiService.getErrorRank(this.countryId, this.errorRankStartDate, this.errorRankEndDate)
      .then(async (response: IErrorsRankResponse) => {
        this.errors = response.records;
        this.isProcessing = false;
    });
  }

}
