import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import * as Chartist from 'chartist';
import { ChartModel } from '../helpers/ChartModel';
import 'chartist-plugin-pointlabels';
import 'chartist-plugin-axistitle';
import 'chartist-plugin-threshold';
import 'chartist-plugin-tooltip'

@Component({
  selector: 'chart-generator',
  styleUrls: ['./chartgenerator.component.css'],
  templateUrl: './chartgenerator.component.html'
})
export class ChartGeneratorComponent implements OnInit, AfterViewInit {

  @Input() chartData: ChartModel;
  @Input() chartName: string;

  public optionsDailySalesChart: any;
  public optionswebsiteViewsChart: any;
  public responsiveOptions: any;

  ngOnInit(){

      this.optionsDailySalesChart = {
            lineSmooth: Chartist.Interpolation.cardinal({
                tension: 1
            }),
            low: 0,
            high: Math.max.apply(null, this.chartData.series[0]) + 5, // creative tim: we recommend you to set the high sa the biggest value + something for a better look
            plugins: [
              Chartist.plugins.tooltip({
                anchorToPoint: true
              })
            ]
        };

      this.optionswebsiteViewsChart = {
          low: 0,
          high: Math.max.apply(null, this.chartData.series[0]) + 5,
          plugins: [
            Chartist.plugins.tooltip({
              anchorToPoint: true
            })
          ]
      };

      this.responsiveOptions = [
        ['screen and (min-width: 641px) and (max-width: 1024px)', {
          axisX: {
            labelInterpolationFnc: function (value) {
              return value;
            }
          }
        }],
        ['screen and (max-width: 640px)', {
          axisX: {
            labelInterpolationFnc: function (value) {
              return value[0];
            }
          }
        }]
      ];

  }

  ngAfterViewInit(): void {

    if(this.chartName === 'UKWeekGraphTasksChart'){
      var ukWeekGraphTasksChart = new Chartist.Line(`#${this.chartName}`, this.chartData, this.optionsDailySalesChart);
      this.startAnimationForLineChart(ukWeekGraphTasksChart);
    }
    else if(this.chartName === 'ROIWeekGraphTasksChart'){
      var roiWeekGraphTasksChart = new Chartist.Line(`#${this.chartName}`, this.chartData, this.optionsDailySalesChart);
      this.startAnimationForLineChart(roiWeekGraphTasksChart);
    }
    else if(this.chartName === 'UKMonthRateChart'){
      var ukMonthRateChart = new Chartist.Bar(`#${this.chartName}`, this.chartData, this.optionswebsiteViewsChart, this.responsiveOptions);
      this.startAnimationForBarChart(ukMonthRateChart);
    }
    else if(this.chartName === 'ROIMonthRateChart'){
      var roiMonthRateChart = new Chartist.Bar(`#${this.chartName}`, this.chartData, this.optionswebsiteViewsChart, this.responsiveOptions);
      this.startAnimationForBarChart(roiMonthRateChart);
    }


  }

  startAnimationForLineChart(chart){
    let seq: any, delays: any, durations: any;
    seq = 0;
    delays = 80;
    durations = 500;

    chart.on('draw', function(data) {
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
