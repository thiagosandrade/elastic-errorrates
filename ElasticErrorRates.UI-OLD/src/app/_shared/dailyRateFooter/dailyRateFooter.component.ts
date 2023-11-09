import { Component, OnInit, Input, AfterViewInit } from '@angular/core';

@Component({
  selector: 'dailyrate-footer',
  templateUrl: './dailyRateFooter.component.html',
  styleUrls: ['./dailyRateFooter.component.css']
})
export class DailyRateFooterComponent implements OnInit, AfterViewInit {

  @Input() rate: any = {  };
     
  constructor() { }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
  }

}
