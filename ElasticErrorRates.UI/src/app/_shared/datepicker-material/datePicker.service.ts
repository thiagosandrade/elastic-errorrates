import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

@Injectable()
export class DatePickerService {
    private subject = new BehaviorSubject<Date>(new Date());

    constructor() {
    }

    getDateValue() : Observable<Date>{
        return this.subject.asObservable();
    }

    setDateValue(dateValue : Date){
        this.subject.next(dateValue);
    }
}
