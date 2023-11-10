import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { DatePipe } from '@angular/common';
import { map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LogService {

    constructor(
        private http: HttpClient,
        public datepipe: DatePipe
    ) {}
    
    import() {
        return this.http.post(`${environment.apiUrl}/log/import`, null);
    }

    getLogsAggregate(startDate: Date | null = null, enddate: Date | null = null) : any {
        var url: string =  `${environment.apiUrl}/log/searchlogsaggregate`;

        if(startDate !== null && enddate !== null){
            url = url.concat(`?startdate=${this.datepipe.transform(startDate, 'yyyy-MM-ddTHH:mm:ss.SSS')}&enddate=${this.datepipe.transform(enddate, 'yyyy-MM-ddTHH:mm:ss.SSS')}`);
        }

        return this.http.get<any>(`${url}`)
            .pipe(map((response: any) => {
                return response;
            }));
    }

    getLogsDetailed(startDate: Date | null = null, enddate: Date | null = null, httpUrl: string) : any {
        var url: string =  `${environment.apiUrl}/log/searchlogsdetailed/${0}/${50}?httpUrl=${httpUrl}`;

        if(startDate !== null && enddate !== null){
            url = url.concat(`&startdate=${this.datepipe.transform(startDate, 'yyyy-MM-ddTHH:mm:ss.SSS')}&enddate=${this.datepipe.transform(enddate, 'yyyy-MM-ddTHH:mm:ss.SSS')}`);
        }

        return this.http.get<any>(`${url}`)
            .pipe(map((response: any) => {
                return response;
            }));
    }

    findLogs(startDate: Date | null = null, enddate: Date | null = null, columnField: string, httpUrl: string, term : string) : any {
        var url = `${environment.apiUrl}/log/find/?columnField=${columnField}&httpUrl=${httpUrl}&term=${term}`

        if(startDate !== null && enddate !== null){
            url = url.concat(`&startdate=${this.datepipe.transform(startDate, 'yyyy-MM-ddTHH:mm:ss.SSS')}&enddate=${this.datepipe.transform(enddate, 'yyyy-MM-ddTHH:mm:ss.SSS')}`);
        }

        return this.http.get<any>(`${url}`)
            .pipe(map((response: any) => {
                return response;
            }));
    }
}