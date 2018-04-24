import { ILog } from "./../model/log";

export interface ILogResponse {
    totalRecords: number;
    records: ILog[]
}

export class LogResponse implements ILogResponse {
    totalRecords: number;
    records: ILog[]
}


