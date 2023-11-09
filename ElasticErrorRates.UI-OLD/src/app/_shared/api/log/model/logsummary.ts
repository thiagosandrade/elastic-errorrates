export interface ILogSummary {
    Id: number;
    HttpUrlCount: string;
    HttpUrl: string;
    FirstOccurrence: Date;
    LastOccurrence: Date;
}

export class LogSummary implements ILogSummary {
    Id: number;
    HttpUrlCount: string;
    HttpUrl: string;
    FirstOccurrence: Date;
    LastOccurrence: Date;
}
