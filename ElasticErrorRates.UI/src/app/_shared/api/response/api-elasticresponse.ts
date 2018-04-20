import { IProduct } from "./../model/product";

export interface IElasticResponse {
    totalRecords: number;
    records: IProduct[]
}

export class ElasticResponse implements IElasticResponse {
    totalRecords: number;
    records: IProduct[]
}
