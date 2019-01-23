import { Component, OnInit, AfterViewInit, Input } from '@angular/core';

@Component({
  selector: 'chart-footer',
  templateUrl: './chartFooter.component.html',
  styleUrls: ['./chartFooter.component.css']
})
export class ChartFooterComponent implements OnInit, AfterViewInit {
  
  @Input() comparison: any = {  };
     
  constructor() { }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
  }
}
