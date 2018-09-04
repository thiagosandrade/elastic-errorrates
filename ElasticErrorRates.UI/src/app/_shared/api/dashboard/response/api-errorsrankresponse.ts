import { IError } from "../model/error";

export interface IErrorsRankResponse {
    records: IError[]
}

export class ErrorsRankResponse implements IErrorsRankResponse {
    records: IError[]
}


