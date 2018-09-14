import { Component, OnInit } from '@angular/core';
import { ApiDashboardService } from '../../../../_shared/api/dashboard/api.service';
import { IError } from '../../../../_shared/api/dashboard/model/error';
import { Countries } from '../../../../_shared/helpers/Country.enum';
import { IErrorsRankResponse } from '../../../../_shared/api/dashboard/response/api-errorsrankresponse';

@Component({
  selector: 'uk-errors-rank',
  templateUrl: './uk-errors-rank.component.html',
  styleUrls: ['./uk-errors-rank.component.css']
})
export class UkErrorsRankComponent implements OnInit {
  public errors: IError[];
  public errorRankStartDate: Date = new Date();
  public errorRankEndDate: Date = new Date();
  private countryId: number = Countries.UK;
  public isProcessing: boolean;

  constructor(private apiService: ApiDashboardService) { }

  ngOnInit() {
    this.errorRankStartDate.setDate((new Date()).getDate()-1);
    this.errorRankEndDate.setDate((new Date()).getDate());

    this.isProcessing = true;
    this.fetchErrorRank();
  }

  fetchErrorRank(){
    this.apiService.getErrorRank(this.countryId, this.errorRankStartDate, this.errorRankEndDate).then(async (response: IErrorsRankResponse) => {
      this.errors = response.records;
      this.isProcessing = false;
    });
  }

}
