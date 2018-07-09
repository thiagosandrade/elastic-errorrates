import { IDailyRate } from "./../model/dailyrate";

export interface IDailyRateResponse {
    totalRecords: number;
    records: IDailyRate[]
}

export class DailyRateResponse implements IDailyRateResponse {
    totalRecords: number;
    records: IDailyRate[]
}


