import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'loading-spinner',
  styleUrls: ['./loadingspinner.component.css'],
  templateUrl: './loadingspinner.component.html'
})
export class LoadingSpinnerComponent implements OnChanges, OnInit {
  
  @Input() isProcessing: string;
  showSpinner : string;
  constructor() { 
    
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(this.showSpinner != this.isProcessing){
      this.showSpinner = this.isProcessing;
    }
  }

  ngOnInit(){
     this.showSpinner = this.isProcessing;
  }
}
