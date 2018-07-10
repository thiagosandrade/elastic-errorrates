import { Component, OnInit, Input, OnChanges, SimpleChanges, SimpleChange, Output, EventEmitter } from '@angular/core';
import { ModalService } from "./modal.service";
import { ILog } from '../../_shared/api/log/model/log';
import { Observable } from 'rxjs/Observable';

@Component({
   selector: 'info-modal',
   templateUrl: './modal.component.html',
   styleUrls: ['./modal.component.css']
})
export class ModalComponent implements OnChanges, OnInit {
    @Input() log: ILog;
    @Input() displayModal: boolean;

    @Output() change: EventEmitter<ILog> = new EventEmitter<ILog>();

    message: any;
    constructor(private alertService : ModalService) { }

    ngOnInit() {
        this.alertService.getMessage().subscribe(message => {
            this.message = message;
        });   
    }

    ngOnChanges(changes: SimpleChanges){
        if(this.log != null || this.log != undefined){
            this.displayModal = true;
            console.log(this);
        }
        
    }

    onCloseModal(){
        this.displayModal = false;
        this.log = null;
        this.change.emit(this.log);
    }
}