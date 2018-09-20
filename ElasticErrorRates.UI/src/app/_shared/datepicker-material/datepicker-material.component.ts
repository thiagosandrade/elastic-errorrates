import { Component, Output, EventEmitter, OnChanges, SimpleChanges, Injectable, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';

import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';

import * as _moment from 'moment';
import { DatePickerService } from './datePicker.service';
import { MatDatepickerInputEvent } from '@angular/material';

export const MY_FORMATS = {
  parse: {
    dateInput: 'DD-MM-YYYY',
  },
  display: {
    dateInput: 'DD-MM-YYYY',
    monthYearLabel: 'MM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'datepicker-material',
  templateUrl: './datepicker-material.component.html',
  styleUrls: ['./datepicker-material.component.css'],
  providers: [
      {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},

      {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS},
  ]
})

@Injectable()
export class DatepickerMaterialComponent {
  
  constructor(private datePickerService: DatePickerService){
    
  }
  
  
  date = new FormControl(_moment(new Date(), 'DD-MM-YYYY'));

  addEvent(type: string, event: MatDatepickerInputEvent<Date>) {

    this.datePickerService.setDateValue(_moment(event.value, 'DD-MM-YYYY').toDate());
    
  }

}



