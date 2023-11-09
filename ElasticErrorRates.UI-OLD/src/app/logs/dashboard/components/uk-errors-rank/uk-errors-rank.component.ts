import { Component, OnInit, Input, SimpleChanges, SimpleChange } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api-dashboard.service';
import { IError } from '../../../../_shared/api/dashboard/model/error';
import { Countries } from '../../../../_shared/helpers/Country.enum';
import { IErrorsRankResponse } from '../../../../_shared/api/dashboard/response/api-errorsrankresponse';

@Component({
  selector: 'uk-errors-rank',
  templateUrl: './uk-errors-rank.component.html',
  styleUrls: ['./uk-errors-rank.component.css']
})
export class UkErrorsRankComponent {
  public errors: IError[];
  public errorRankStartDate: Date;
  public errorRankEndDate: Date;
  private countryId: number = Countries.UK;
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
