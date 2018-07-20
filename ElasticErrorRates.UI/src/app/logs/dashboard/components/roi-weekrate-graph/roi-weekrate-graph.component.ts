import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'roi-weekrate-graph',
  templateUrl: './roi-weekrate-graph.component.html',
  styleUrls: ['./roi-weekrate-graph.component.css']
})
export class ROIWeekrateGraphComponent implements OnInit {

  public dataCompletedTasksChart: { labels: string[]; series: number[][]; };
  public chartName : string;

  ngOnInit() {

      /* ----------==========     ROIWeekGraphTasksChart Chart initialization    ==========---------- */
      this.chartName = 'ROIWeekGraphTasksChart';

      this.dataCompletedTasksChart = {
          labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S'],
          series: [
              [230, 750, 450, 300, 280, 240, 200, 190]
          ]
      };
  }
}
