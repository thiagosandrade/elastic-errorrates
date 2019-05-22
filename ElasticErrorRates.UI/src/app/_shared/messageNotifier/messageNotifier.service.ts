import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { Message } from 'primeng/api';

export type Severities = 'success' | 'info' | 'warn' | 'error';

@Injectable()
export class MessageNotifierService {

  messageNotificationChange: Subject<Object> = new Subject<Object>();
  static singletonInstance: MessageNotifierService;

  constructor() {
    if(!MessageNotifierService.singletonInstance){
      MessageNotifierService.singletonInstance = this;
    }

    return MessageNotifierService.singletonInstance;
 }


  messageNotify(severity: Severities, summary: string) {
    this.messageNotificationChange.next({ severity, summary });
  }

}