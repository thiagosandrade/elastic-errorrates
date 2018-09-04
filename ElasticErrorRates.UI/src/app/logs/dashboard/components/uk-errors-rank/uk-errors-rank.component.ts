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
  private countryId: number = Countries.UK;
  constructor(private apiService: ApiDashboardService) { }

  ngOnInit() {
    this.fetchErrorRank();
  }

  fetchErrorRank(){
    this.apiService.getErrorRank(this.countryId).then(async (response: IErrorsRankResponse) => {
      this.errors = response.records;
    });
  }

}
