export interface IChartModel {
    labels: string[]; 
    series: number[][]; 
}

export class ChartModel implements IChartModel {
    labels: string[]; 
    series: number[][]; 
}
