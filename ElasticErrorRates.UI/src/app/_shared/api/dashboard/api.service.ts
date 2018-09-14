import { Injectable } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../../../environments/environment';
import 'rxjs/add/operator/catch';

import { IDailyRateResponse } from './response/api-dailyrateresponse';
import { IGraphRequestResponse } from './response/api-graphrequestresponse';
import { IErrorsRankResponse } from './response/api-errorsrankresponse';

@Injectable()
export class ApiDashboardService {
    constructor(private http: HttpClient) { }

    public importLogs(): Observable<any>{
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${environment.apiUrl}/dashboard/import`, options)
            .catch((error: any) => { return Observable.throw(error) });
    }

    public async getDailyRate(countryId: number, startDate: Date = null, enddate: Date = null): Promise<IDailyRateResponse> {

        var url: string =  `${environment.apiUrl}/dashboard/search/${countryId}`;

        if(startDate !== null && enddate !== null){
            startDate.setHours(7,0,0,0);
            enddate.setHours(7,0,0,0);
            

            url = url.concat(`?startdate=${startDate.toISOString()}&enddate=${enddate.toISOString()}`);
        }
        
        return await this.http.get<IDailyRateResponse>(url).toPromise();
    }

    public async getGraphValues(countryId: number, frequencyType: string, numberOfResults: string): Promise<IGraphRequestResponse> {

        var url: string =  `${environment.apiUrl}/dashboard/searchaggregate`;

        url = url.concat(`?countryId=${countryId}&typeAggregation=${frequencyType}&numberOfResults=${numberOfResults}`);
        
        return await this.http.get<IGraphRequestResponse>(url).toPromise();
    }

    public async getErrorRank(countryId: number, startDate: Date = null, enddate: Date = null): Promise<IErrorsRankResponse> {

        var url: string =  `${environment.apiUrl}/dashboard/errorsrank/${countryId}`;

        if(startDate !== null && enddate !== null){
            startDate.setHours(0,0,0,0);
            enddate.setHours(23,59,59,59);
            

            url = url.concat(`?startdate=${startDate.toISOString()}&enddate=${enddate.toISOString()}`);
        }
        
        return await this.http.get<IErrorsRankResponse>(url).toPromise();
    }

}
