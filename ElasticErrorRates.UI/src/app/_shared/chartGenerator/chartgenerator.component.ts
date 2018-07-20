import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import * as Chartist from 'chartist';
import { ChartModel } from '../helpers/ChartModel';
import 'chartist-plugin-pointlabels';

@Component({
  selector: 'chart-generator',
  styleUrls: ['./chartgenerator.component.css'],
  templateUrl: './chartgenerator.component.html'
})
export class ChartGeneratorComponent implements OnInit, AfterViewInit {
  
  @Input() chartData: ChartModel;
  @Input() chartName: string;

  public optionsDailySalesChart: any;
  public optionsCompletedTasksChart: any;
  public optionswebsiteViewsChart: any;
  public responsiveOptions: any;

  ngOnInit(){

      this.optionsDailySalesChart = {
            lineSmooth: Chartist.Interpolation.cardinal({
                tension: 1
            }),
            low: 0,
            high: Math.max.apply(null, this.chartData.series[0]) + 20, // creative tim: we recommend you to set the high sa the biggest value + something for a better look
            chartPadding: { top: 0, right: 0, bottom: 0, left: 0}
            // plugins: [
            //   Chartist.plugins.ctPointLabels({
            //     textAnchor: 'middle',
                
            //   })
            // ]
        }

      this.optionsCompletedTasksChart = {
          lineSmooth: Chartist.Interpolation.cardinal({
              tension: 1
          }),
          low: 0,
          high: 1000, // creative tim: we recommend you to set the high sa the biggest value + something for a better look
          chartPadding: { top: 0, right: 0, bottom: 0, left: 0}
      }

      this.optionswebsiteViewsChart = {
          axisX: {
              showGrid: true
          },
          low: 0,
          high: Math.max.apply(null, this.chartData.series[0]) + 5,
          chartPadding: { top: 0, right: 5, bottom: 0, left: 0}
      };
      
      this.responsiveOptions = [
        ['screen and (max-width: 640px)', {
          seriesBarDistance: 5,
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
      var dailySalesChart = new Chartist.Line(`#${this.chartName}`, this.chartData, this.optionsDailySalesChart);
      this.startAnimationForLineChart(dailySalesChart);
    }
    else if(this.chartName === 'ROIWeekGraphTasksChart'){
      var completedTasksChart = new Chartist.Line(`#${this.chartName}`, this.chartData, this.optionsCompletedTasksChart);
      this.startAnimationForLineChart(completedTasksChart);
    }
    else if(this.chartName === 'UKMonthRateChart'){
      var websiteViewsChart = new Chartist.Bar(`#${this.chartName}`, this.chartData, this.optionswebsiteViewsChart, this.responsiveOptions);
      this.startAnimationForBarChart(websiteViewsChart);
    }
    else if(this.chartName === 'ROIMonthRateChart'){
      var websiteViewsChart = new Chartist.Bar(`#${this.chartName}`, this.chartData, this.optionswebsiteViewsChart, this.responsiveOptions);
      this.startAnimationForBarChart(websiteViewsChart);
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
