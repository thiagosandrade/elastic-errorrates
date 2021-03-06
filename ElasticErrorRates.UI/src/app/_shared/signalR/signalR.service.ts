import { Injectable, EventEmitter, Output } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@aspnet/signalr';
import { SignalRMessage } from './signalR.message';
import { environment } from '../../../environments/environment';
import { Message } from 'primeng/api';

@Injectable()
export class SignalRService {
    @Output() notificationReceived: EventEmitter<SignalRMessage> = new EventEmitter();
    @Output() connectionEstablished: EventEmitter<any> = new EventEmitter();
 
    private _hubConnection: HubConnection;
    private baseUrl = `${environment.apiUrl}/notify`;
    public msgs: Message[] = [];

    constructor() {
        this.createConnection();
        this.registerOnServerEvents();
        this.startConnection();
    }
 
    private createConnection() {
        this._hubConnection = new HubConnectionBuilder()
            .withUrl(this.baseUrl,
                {
                    transport: HttpTransportType.LongPolling
                }
            )
            .build();
    }
 
    private registerOnServerEvents() {
        this._hubConnection
            .on('BroadcastMessage', (data) => {
                // console.log(data);
                 this.notificationReceived.emit(data);
                }
            );
    }
 
    private startConnection(): void {
        this._hubConnection
            .start()
            .then(() => {
                // console.log('Hub started');
                this.connectionEstablished.emit({ severity: 'true', summary: 'empty' });
            }).catch(err => {
                console.log(`Error on connecting to signalR, retrying..`);
                // setTimeout(this.startConnection(), 10000);
            });
    }
}