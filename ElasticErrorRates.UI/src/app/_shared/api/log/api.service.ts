import { Injectable } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../../../environments/environment';
import 'rxjs/add/operator/catch';

import { ILogResponse } from './response/api-logresponse';
import { ILogSummaryResponse } from './response/api-logsummaryresponse';

@Injectable()
export class ApiLogService {
    constructor(private http: HttpClient) { }

    public importLogs(): Observable<any>{
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${environment.apiUrl}/log/import`, options)
            .catch((error: any) => { return Observable.throw(error) });
    }

    public getLogsAggregate(): Observable<ILogSummaryResponse> {
        return this.http.get<ILogSummaryResponse>(`${environment.apiUrl}/log/searchaggregate`);
    }

    public getLogs(page: number, pageSize: number, httpUrl: string): Observable<ILogResponse> {
        return this.http.get<ILogResponse>(`${environment.apiUrl}/log/search/${page}/${pageSize}?httpUrl=${httpUrl}`);
    }

    public findLogs(columnField: string, httpUrl: string, term : string): Observable<ILogResponse> {
        return this.http.get<ILogResponse>(`${environment.apiUrl}/log/find/?columnField=${columnField}&httpUrl=${httpUrl}&term=${term}`);
    }

    public findLogsSummary(columnField: string, httpUrl: string, term : string): Observable<ILogSummaryResponse> {
        return this.http.get<ILogSummaryResponse>(`${environment.apiUrl}/log/find/?columnField=${columnField}&httpUrl=${httpUrl}&term=${term}`);
    }
}
