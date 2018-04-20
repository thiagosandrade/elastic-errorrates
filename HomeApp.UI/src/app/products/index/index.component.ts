import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../_shared/api/api.service';
import { IProduct } from '../../_shared/api/model/product';
import { IElasticResponse } from '../../_shared/api/response/api-elasticresponse';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent {
  public products: IProduct[];
  public totalRecords: number;
  public isProcessing: boolean; 
  public term: string;
  public sort: string = "false";
  public match: string = "false";

  constructor(private apiService: ApiService) { 
    this.fillGrid();
  }

  fillGrid() {
    this.isProcessing = true;
    
    this.apiService.getProducts().subscribe((products : IElasticResponse) => {
      this.products = products.records;
      this.totalRecords = products.totalRecords

    },
    (err : any) => {
      console.log(err);
    },
    () => {
      this.isProcessing = false;
    });
    
  }

  onInputText(term : string){
    this.term = (term != null && term != undefined && term != "") ? term : "null"
    this.apiService.findProducts(this.term, this.sort, this.match).subscribe((products : IElasticResponse) => {
       this.products = products.records;
       this.totalRecords = products.totalRecords
    },
    (err : any) => {
      console.log(err);
    },
    () => {
      this.isProcessing = false;
    });
  }
  
}
