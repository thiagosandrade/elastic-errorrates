import { Component, OnInit } from '@angular/core';
import { SignalRService } from './_shared/signalR/signalR.service';
import { SignalRMessage } from './_shared/signalR/signalR.message';
import { Message } from 'primeng/api';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  
  title = 'app';

  public msgs: Message[] = [];

  constructor(private signalRService : SignalRService) {  }

   ngOnInit(): void {
    this.signalRService.notificationReceived.subscribe((signalRMessage : SignalRMessage[]) => {
      this.msgs = [];
      signalRMessage.forEach(message => {
        this.msgs.push({ 
          life: 3000, 
          sticky: false,
          severity: message.type, 
          summary: message.payload,
          closable: true
        });  
      });
    });
  }
}
