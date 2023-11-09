import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'div-flexible',
  styleUrls: ['./divflexible.component.css'],
  templateUrl: './divflexible.component.html'
})
export class DivFlexibleComponent implements OnInit {
  @Input() textContent: string;
  message : string;
  constructor() { 
    
  }

  ngOnInit(){
     this.message = this.textContent;
  }
}
