import { Component, OnInit, Input, OnChanges, SimpleChanges, SimpleChange } from '@angular/core';
import { ModalService } from "./modal.service";
import { ILog } from '../../_shared/api/model/log';



@Component({
   selector: 'info-modal',
   templateUrl: './modal.component.html',
   styleUrls: ['./modal.component.css']
})
export class ModalComponent implements OnChanges, OnInit {
    @Input() log: ILog;
    @Input() displayModal: boolean;

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
    }

}