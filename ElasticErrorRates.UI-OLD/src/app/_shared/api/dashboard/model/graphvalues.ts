export interface IGraphValues {
    date: string;
    orderCount: string;
    errorCount: string;
    orderValue: string;
    errorPercentage: string;
}

export class GraphValues implements IGraphValues {
    date: string;
    orderCount: string;
    errorCount: string;
    orderValue: string;
    errorPercentage: string;
}
