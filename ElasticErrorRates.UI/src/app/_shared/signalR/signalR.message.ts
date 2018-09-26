export interface ISignalRMessage {
    type: string;
    payload: string;
}

export class SignalRMessage implements ISignalRMessage {
    
    type: string;
    payload: string;
    
    constructor(type: string, payload: string) {
        this.type = type;
        this.payload = payload;
    }

    
}
