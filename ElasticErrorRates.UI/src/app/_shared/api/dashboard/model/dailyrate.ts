export interface IDailyRate {
    Id: number;
    CountryId: number;
    StartDate: Date;
    EndDate: Date;
    ErrorCount: number;
    OrderCount: number;
    OrderValue: number;
    ErrorPercentage: number;
}

export class DailyRate implements IDailyRate {
    Id: number;
    CountryId: number;
    StartDate: Date;
    EndDate: Date;
    ErrorCount: number;
    OrderCount: number;
    OrderValue: number;
    ErrorPercentage: number;
}
