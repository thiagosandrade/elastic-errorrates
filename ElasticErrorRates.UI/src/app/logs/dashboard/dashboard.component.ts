import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { DatePickerService } from '../../_shared/datepicker-material/datePicker.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})



export class DashboardComponent implements OnInit {
  @Output() datePickerDate : Date;

  constructor(private datePickerService : DatePickerService) {
  
    this.datePickerService.getDateValue().subscribe(
      (res : Date) => {
          this.datePickerDate = res;
      }
    );
  }
  
  ngOnInit() {
    this.datePickerService.setDateValue(new Date());
  }
 
}
