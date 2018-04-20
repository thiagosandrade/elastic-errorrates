export interface IProduct {
    DocumentId: number;
    Id: number;
    Name: string;
    SpeechNumber: string;
    LineNumber: string;
    Speaker: string;
    TextEntry: string;
    Highlight: string;
}

export class Product implements IProduct {
    DocumentId: number;
    Id: number;
    Name: string;
    SpeechNumber: string;
    LineNumber: string;
    Speaker: string;
    TextEntry: string;
    Highlight: string;
}
