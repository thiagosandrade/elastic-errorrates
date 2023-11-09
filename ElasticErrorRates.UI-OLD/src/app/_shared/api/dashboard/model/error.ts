export interface IError {
    httpUrlCount: number;
    httpUrl: string;
    firstOccurrence: string;
    lastOccurrence: Date;
    StartDate: Date;
    EndDate: Date;
}

export class Error implements IError {
    httpUrlCount: number;
    httpUrl: string;
    firstOccurrence: string;
    lastOccurrence: Date;
    StartDate: Date;
    EndDate: Date;
}
