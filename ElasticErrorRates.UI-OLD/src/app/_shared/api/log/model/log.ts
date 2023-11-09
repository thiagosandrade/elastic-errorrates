export interface ILog {
    Id: number;
    Level: string;
    Message: string;
    Source: string;
    Exception: string;
    HttpUrl: string;
    DateTimeLogged: Date;
    DateTimeLoggedAsString: string;
}

export class Log implements ILog {
    Id: number;
    Level: string;
    Message: string;
    Source: string;
    Exception: string;
    HttpUrl: string;
    DateTimeLogged: Date;
    DateTimeLoggedAsString: string;
}
