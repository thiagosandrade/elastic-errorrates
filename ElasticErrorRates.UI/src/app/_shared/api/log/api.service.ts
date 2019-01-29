import { Injectable } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../../../environments/environment';

import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { map } from 'rxjs/operators';

import { ILogResponse } from './response/api-logresponse';
import { ILogSummaryResponse } from './response/api-logsummaryresponse';
import { DatePipe } from '@angular/common';

@Injectable()
export class ApiLogService {
    constructor(private http: HttpClient, public datepipe: DatePipe) { }

    public async getLogsAggregate(startDate: Date = null, endDate: Date = null): Promise<ILogSummaryResponse> {
        
        var url: string =  `${environment.apiUrl}/log/searchaggregate`;

        if(startDate !== null && endDate !== null){
            startDate.setHours(6,0,0,0);
            endDate.setHours(6,0,0,0);
            
            url = url.concat(`?startdate=${this.datepipe
                .transform(startDate, 'yyyy-MM-ddTHH:mm:ss.SSS')}&enddate=${this.datepipe.transform(endDate, 'yyyy-MM-ddTHH:mm:ss.SSS')}`);
        }
        
        return await this.http.get<ILogSummaryResponse>(url)
            .pipe(
                map( response => {
                    return response;
                })
            )
            .toPromise();
    }

    public getLogs(page: number, pageSize: number, httpUrl: string): Observable<ILogResponse> {
        return this.http.get<ILogResponse>(`${environment.apiUrl}/log/search/${page}/${pageSize}?httpUrl=${httpUrl}`);
    }

    public findLogs(columnField: string, httpUrl: string, term : string): Observable<ILogResponse> {
        return this.http.get<ILogResponse>(`${environment.apiUrl}/log/find/?columnField=${columnField}&httpUrl=${httpUrl}&term=${term}`);
    }

    public async findLogsSummary(columnField: string, httpUrl: string, term : string, startDate: Date = null, endDate: Date = null): Promise<ILogSummaryResponse> {
        
        var url: string =  `${environment.apiUrl}/log/find?columnField=${columnField}&httpUrl=${httpUrl}&term=${term}`;

        if(startDate !== null && endDate !== null){
            startDate.setHours(6,0,0,0);
            endDate.setHours(6,0,0,0);

            url = url.concat(`&startdate=${this.datepipe
                .transform(startDate, 'yyyy-MM-ddTHH:mm:ss.SSS')}&enddate=${this.datepipe.transform(endDate, 'yyyy-MM-ddTHH:mm:ss.SSS')}`);
        }

        return await this.http.get<ILogSummaryResponse>(url)
            .pipe(
                map( response => {
                    return response;
                })
            )
            .toPromise();
    }
}
