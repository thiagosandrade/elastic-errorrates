import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { DatePickerService } from '../../_shared/datepicker-material/datePicker.service';
import { ApiDashboardService } from '../../_shared/api/dashboard/api-dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})



export class DashboardComponent implements OnInit {
  
  @Output() datePickerDate : Date;
  public hasResults : Promise<boolean>;

  constructor(private datePickerService : DatePickerService, private apiService: ApiDashboardService ) {  }
  
  ngOnInit() {
    this.datePickerService.getDateValue().subscribe(
      (res : Date) => {
          var yesterday = new Date();
          yesterday.setDate(res.getDate() - 1);
          
          this.apiService.getLogsQuantity(yesterday, res).then(
            (qtt : number) => {
              if(qtt > 0){
                this.hasResults = Promise.resolve(true);
                this.datePickerDate = res;
              }
              else{
                this.hasResults = Promise.resolve(false);
              }
            }
          );
      }
    );
  }
 
}
