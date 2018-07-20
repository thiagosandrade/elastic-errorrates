import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'uk-monthrate-graph',
  templateUrl: './uk-monthrate-graph.component.html',
  styleUrls: ['./uk-monthrate-graph.component.css']
})
export class UkMonthRateGraphComponent implements OnInit {

  public datawebsiteViewsChart: { labels: string[]; series: number[][]; };
  public chartName : string;

  ngOnInit() {

     /* ----------==========     UKMonthRateChart Chart initialization    ==========---------- */
    this.chartName = 'UKMonthRateChart';

    this.datawebsiteViewsChart = {
      labels: ['J', 'F', 'M', 'A', 'M', 'J', 'J', 'A', 'S', 'O', 'N', 'D'],
      series: [
        [542, 443, 320, 780, 553, 453, 326, 434, 568, 610, 756, 895]

      ]
    };

    //http://localhost:30539/api/dashboard/searchaggregate?typeAggregation=5&numberOfResults=monthActual
  }
}
