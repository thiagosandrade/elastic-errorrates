export interface IUser {
    id: string;
    name: string;
    userName: string;
    password: string;
    token: string;
}

export class User implements IUser {
    id: string;
    name: string;
    userName: string;
    password: string;
    token: string;

    constructor(userName : string, password : string, token : string){
        this.userName = userName;
        this.password = password;
        this.token = token;
    }
}
