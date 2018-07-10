import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'loading-spinner',
  styleUrls: ['./loadingSpinner.component.css'],
  templateUrl: './loadingSpinner.component.html'
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
