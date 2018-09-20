import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable()
export class DatePickerService {
    private subject = new Subject<Date>();

constructor() { }
    getDateValue() : Observable<Date>{
        return this.subject.asObservable();
    }

    setDateValue(dateValue : Date){
        this.subject.next(dateValue);
    }
}
