import { IGraphValues } from "../model/graphvalues";

export interface IGraphRequestResponse {
    totalRecords: number;
    records: IGraphValues[]
}

export class GraphRequestResponse implements IGraphRequestResponse {
    totalRecords: number;
    records: IGraphValues[]
}


