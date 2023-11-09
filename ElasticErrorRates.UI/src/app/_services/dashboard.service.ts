import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { DatePipe } from '@angular/common';
import { map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class DashboardService {

    constructor(
        private http: HttpClient,
        public datepipe: DatePipe
    ) {}
    
    import() {
        return this.http.post(`${environment.apiUrl}/dashboard/import`, null);
    }

    getLogsQuantity(startDate: Date | null = null, enddate: Date | null = null) {
        var url: string =  `${environment.apiUrl}/dashboard/getlogsquantity`;

        if(startDate !== null && enddate !== null){
            url = url.concat(`?startdate=${this.datepipe.transform(startDate, 'yyyy-MM-ddTHH:mm:ss.SSS')}&enddate=${this.datepipe.transform(enddate, 'yyyy-MM-ddTHH:mm:ss.SSS')}`);
        }

        return this.http.get<Number>(`${url}`)
            .pipe(map((response: any) => {
                return response;
            }));
    }

    getDailyRate(countryId: number, startDate: Date | null = null, enddate: Date | null = null) {
        var url: string =  `${environment.apiUrl}/dashboard/search?countryId=${countryId}`;

        if(startDate !== null && enddate !== null){
            url = url.concat(`&startdate=${this.datepipe.transform(startDate, 'yyyy-MM-ddTHH:mm:ss.SSS')}&enddate=${this.datepipe.transform(enddate, 'yyyy-MM-ddTHH:mm:ss.SSS')}`);
        }

        return this.http.get<Number>(`${url}`)
            .pipe(map((response: any) => {
                return response.totalRecords;
            }));
    }
}