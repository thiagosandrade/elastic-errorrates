import { Component, OnInit } from '@angular/core';
import * as Chartist from 'chartist';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  public dataCompletedTasksChart: { labels: string[]; series: number[][]; };
  public chartName : string;
  constructor() { }
  
  ngOnInit() {

     


     
     
  }

}
