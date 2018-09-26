import { Injectable } from '@angular/core'; 
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';

@Injectable() 
export class ModalService {
     private subject = new Subject<any>();
     
     constructor(){}
     
    showModal(title : string, message: string, OK : ()=> void) {
        let that = this;
        this.subject.next({ 
            type: "info", 
            title: title,
            info: message,
            OK:
            function(){
                that.subject.next();
                OK();
            }
         });
     }

     getMessage(): Observable<any> {
        return this.subject.asObservable();
      }
}