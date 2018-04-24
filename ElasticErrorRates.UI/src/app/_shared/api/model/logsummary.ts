export interface ILogSummary {
    Id: number;
    HttpUrlCount: string;
    HttpUrl: string;
    DateTimeLogged: Date;
}

export class LogSummary implements ILogSummary {
    Id: number;
    HttpUrlCount: string;
    HttpUrl: string;
    DateTimeLogged: Date;
}
