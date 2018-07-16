import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import * as Chartist from 'chartist';

@Component({
  selector: 'chart-generator',
  styleUrls: ['./chartgenerator.component.css'],
  templateUrl: './chartgenerator.component.html'
})
export class ChartGeneratorComponent implements OnInit, AfterViewInit {
  
  @Input() chartData: any;
  @Input() chartName: string;

  public optionsDailySalesChart: any;
  public optionsCompletedTasksChart: any;

  ngOnInit(){

      this.optionsDailySalesChart = {
            lineSmooth: Chartist.Interpolation.cardinal({
                tension: 0
            }),
            low: 0,
            high: 50, // creative tim: we recommend you to set the high sa the biggest value + something for a better look
            chartPadding: { top: 0, right: 0, bottom: 0, left: 0},
        }

      this.optionsCompletedTasksChart = {
          lineSmooth: Chartist.Interpolation.cardinal({
              tension: 0
          }),
          low: 0,
          high: 1000, // creative tim: we recommend you to set the high sa the biggest value + something for a better look
          chartPadding: { top: 0, right: 0, bottom: 0, left: 0}
      }
  }

  ngAfterViewInit(): void {

    if(this.chartName === 'dailySalesChart'){
      var dailySalesChart = new Chartist.Line(`#${this.chartName}`, this.chartData, this.optionsDailySalesChart);
      this.startAnimationForLineChart(dailySalesChart);
    }
    else if(this.chartName === 'completedTasksChart'){
      var completedTasksChart = new Chartist.Line(`#${this.chartName}`, this.chartData, this.optionsCompletedTasksChart);
      this.startAnimationForLineChart(completedTasksChart);
    }

  }

  startAnimationForLineChart(chart){
    let seq: any, delays: any, durations: any;
    seq = 0;
    delays = 80;
    durations = 500;

    chart.on('draw', function(data) {
      console.log(data);
      if(data.type === 'line' || data.type === 'area') {
        data.element.animate({
          d: {
            begin: 600,
            dur: 700,
            from: data.path.clone().scale(1, 0).translate(0, data.chartRect.height()).stringify(),
            to: data.path.clone().stringify(),
            easing: Chartist.Svg.Easing.easeOutQuint
          }
        });
      } else if(data.type === 'point') {
            seq++;
            data.element.animate({
              opacity: {
                begin: seq * delays,
                dur: durations,
                from: 0,
                to: 1,
                easing: 'ease'
              }
            });
        }
    });

    seq = 0;
  };

  startAnimationForBarChart(chart){
      let seq2: any, delays2: any, durations2: any;

      seq2 = 0;
      delays2 = 80;
      durations2 = 500;
      chart.on('draw', function(data) {
        if(data.type === 'bar'){
            seq2++;
            data.element.animate({
              opacity: {
                begin: seq2 * delays2,
                dur: durations2,
                from: 0,
                to: 1,
                easing: 'ease'
              }
            });
        }
      });

      seq2 = 0;
  };
}
