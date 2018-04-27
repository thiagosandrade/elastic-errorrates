import { Injectable } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../../environments/environment';
import 'rxjs/add/operator/catch';

import { IProduct } from './model/product';
import { IElasticResponse } from './response/api-elasticresponse';
import { ILogResponse } from './response/api-logresponse';
import { ILogSummaryResponse } from './response/api-logsummaryresponse';

@Injectable()
export class ApiService {
    constructor(private http: HttpClient) { }

    public getProducts(): Observable<IElasticResponse> {
        return this.http.get<IElasticResponse>(`${environment.apiUrl}/elastic/search`);
    }

    public findProducts(term : string, sort : string, match : string ): Observable<IElasticResponse> {
        return this.http.get<IElasticResponse>(`${environment.apiUrl}/elastic/find/${term}/${sort}/${match}`);
    }

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

    public findLogs(term : string, sort : string, match : string ): Observable<ILogResponse> {
        return this.http.get<ILogResponse>(`${environment.apiUrl}/log/find/${term}/${sort}/${match}`);
    }
}
