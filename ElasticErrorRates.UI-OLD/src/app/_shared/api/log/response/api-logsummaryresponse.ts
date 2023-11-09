import { ILogSummary } from "../model/logsummary";

export interface ILogSummaryResponse {
    totalRecords: number;
    records: ILogSummary[]
}

export class LogSummaryResponse implements ILogSummaryResponse {
    totalRecords: number;
    records: ILogSummary[]
}