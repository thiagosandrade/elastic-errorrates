import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class DatePickerService {
    static singletonInstance: DatePickerService;

    private subject = new BehaviorSubject<Date>(new Date());

    constructor() {
        if(!DatePickerService.singletonInstance){
            DatePickerService.singletonInstance = this;
        }

        return DatePickerService.singletonInstance;
    }

    getDateValue() : Observable<Date>{
        return this.subject.asObservable();
    }

    setDateValue(dateValue : Date){
        this.subject.next(dateValue);
    }
}
