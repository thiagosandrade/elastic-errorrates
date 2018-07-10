import { Injectable } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../../../environments/environment';
import 'rxjs/add/operator/catch';

import { IDailyRateResponse } from './response/api-dailyrateresponse';

@Injectable()
export class ApiDashboardService {
    constructor(private http: HttpClient) { }

    public importLogs(): Observable<any>{
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${environment.apiUrl}/dashboard/import`, options)
            .catch((error: any) => { return Observable.throw(error) });
    }

    public getDailyRate(countryId: number, startDate: Date = null, enddate: Date = null): Observable<IDailyRateResponse> {

        var url: string =  `${environment.apiUrl}/dashboard/search/${countryId}`;

        if(startDate !== null && enddate !== null){
            startDate.setHours(7,0,0,0);
            enddate.setHours(7,0,0,0);
            

            url = url.concat(`?startdate=${startDate.toISOString()}&enddate=${enddate.toISOString()}`);
        }
        
        return this.http.get<IDailyRateResponse>(url);
    }

}
