import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'roi-monthrate-graph',
  templateUrl: './roi-monthrate-graph.component.html',
  styleUrls: ['./roi-monthrate-graph.component.css']
})
export class ROIMonthRateGraphComponent implements OnInit {

  public dataROIwebsiteViewsChart: { labels: string[]; series: number[][]; };
  public chartName : string;

  ngOnInit() {

     /* ----------==========     ROIMonthRateChart Chart initialization    ==========---------- */
    this.chartName = 'ROIMonthRateChart';

    this.dataROIwebsiteViewsChart = {
      labels: ['J', 'F', 'M', 'A', 'M', 'J', 'J', 'A', 'S', 'O', 'N', 'D'],
      series: [
        [542, 443, 320, 780, 553, 453, 326, 434, 568, 610, 756, 895]

      ]
    };
  }
}
