export interface IDailyRate {
    id: number;
    countryId: number;
    startDate: Date;
    endDate: Date;
    errorCount: number;
    orderCount: number;
    orderValue: number;
    errorPercentage: number;
}

export class DailyRate implements IDailyRate {
    id: number;
    countryId: number;
    startDate: Date;
    endDate: Date;
    errorCount: number;
    orderCount: number;
    orderValue: number;
    errorPercentage: number;

    constructor(){
        this.startDate = new Date();
        this.endDate = new Date();
    }
}
