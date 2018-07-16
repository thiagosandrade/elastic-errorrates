import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'uk-weekrate-graph',
  templateUrl: './uk-weekrate-graph.component.html',
  styleUrls: ['./uk-weekrate-graph.component.css']
})
export class UkWeekrateGraphComponent implements OnInit {

  public dataDailySalesChart: { labels: string[]; series: number[][]; };
  public chartName : string;

  ngOnInit() {

     /* ----------==========     Daily Sales Chart initialization For Documentation    ==========---------- */
    this.chartName = 'dailySalesChart';

    this.dataDailySalesChart = {
        labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S'],
        series: [
            [12, 17, 7, 17, 23, 18, 38]
        ]
    };
  }
}
