import { Injectable, Input } from '@angular/core';
import { Headers, RequestOptions } from '@angular/http';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../../../environments/environment';

import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { map } from 'rxjs/operators';

import { IDailyRateResponse } from './response/api-dailyrateresponse';
import { IGraphRequestResponse } from './response/api-graphrequestresponse';
import { IErrorsRankResponse } from './response/api-errorsrankresponse';
import { DatePipe } from '@angular/common';

@Injectable()
export class ApiDashboardService {
    
    constructor(private http: HttpClient, public datepipe: DatePipe) { }

    public importLogs(): Observable<any>{
        const headers = new Headers(
            { 
                'Content-Type': 'application/json'
            });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${environment.apiUrl}/dashboard/import`, options)
            .catch((error: any) => { return Observable.throw(error) });
    }

    public async getDailyRate(countryId: number, startDate: Date = null, enddate: Date = null): Promise<IDailyRateResponse> {

        var url: string =  `${environment.apiUrl}/dashboard/search/${countryId}`;

        if(startDate !== null && enddate !== null){
            startDate.setHours(6,0,0,0);
            enddate.setHours(6,0,0,0);
            

            url = url.concat(`?startdate=${this.datepipe.transform(startDate, 'yyyy-MM-ddTHH:mm:ss.SSS')}&enddate=${this.datepipe.transform(enddate, 'yyyy-MM-ddTHH:mm:ss.SSS')}`);
        }
        
        return await this.http.get<IDailyRateResponse>(url)
            .pipe(
                map( response => {
                    response.records.forEach(x => {
                        x.startDate = new Date(x.startDate);
                        x.endDate = new Date(x.endDate)
                        return x;
                    })
                    return response;
                })
            )
            .toPromise();
    }

    public async getGraphValues(countryId: number, frequencyType: string, numberOfResults: string, enddate: Date = null): Promise<IGraphRequestResponse> {

        var url: string =  `${environment.apiUrl}/dashboard/searchaggregate?countryId=${countryId}&typeAggregation=${frequencyType}&numberOfResults=${numberOfResults}`;

        if(enddate !== null){
            enddate.setHours(6,0,0,0);

            url = url.concat(`&enddate=${this.datepipe.transform(enddate, 'yyyy-MM-ddTHH:mm:ss.SSS')}`);
        }
        return await this.http.get<IGraphRequestResponse>(url)
            .map( response => response as IGraphRequestResponse)    
            .toPromise();
    }

    public async getErrorRank(countryId: number, startDate: Date = null, enddate: Date = null): Promise<IErrorsRankResponse> {

        var url: string =  `${environment.apiUrl}/dashboard/errorsrank/${countryId}`;

        if(startDate !== null && enddate !== null){
            startDate.setHours(6,0,0,0);
            enddate.setHours(6,0,0,0);
            
            url = url.concat(`?startdate=${this.datepipe.transform(startDate, 'yyyy-MM-ddTHH:mm:ss.SSS')}&enddate=${this.datepipe.transform(enddate, 'yyyy-MM-ddTHH:mm:ss.SSS')}`);
        }
        
        return await this.http.get<IErrorsRankResponse>(url)
            .map( response => response as IErrorsRankResponse)
            .toPromise();
    }

    public async getLogsQuantity(startDate: Date = null, enddate: Date = null) : Promise<Number>{
        var url: string =  `${environment.apiUrl}/dashboard/getlogsquantity`;

        if(startDate !== null && enddate !== null){
            startDate.setHours(6,0,0,0);
            enddate.setHours(6,0,0,0);
            
            url = url.concat(`?startdate=${this.datepipe.transform(startDate, 'yyyy-MM-ddTHH:mm:ss.SSS')}&enddate=${this.datepipe.transform(enddate, 'yyyy-MM-ddTHH:mm:ss.SSS')}`);
        }

        return await this.http.get<Number>(url)
            .map( response => response as Number)
            .toPromise();
    }

}
