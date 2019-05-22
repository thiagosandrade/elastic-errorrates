import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { map } from 'rxjs/operators';
import { User } from './model/user';
import { BehaviorSubject, Observable } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class ApiUserService {
    
    private subject = new BehaviorSubject<User>(null);
    static singletonInstance: ApiUserService;
    
    constructor(private http: HttpClient, private router: Router) {
        if(!ApiUserService.singletonInstance){
            ApiUserService.singletonInstance = this;
        }

        return ApiUserService.singletonInstance;
     }

    public async login(username: string, password: string) : Promise<User> {
        return await this.http.post<any>(`${environment.apiUrl}/user/authenticate`, 
                { username: username, password: password })
            .pipe(
                map(user => {
                    // login successful if there's a jwt token in the response
                    if (user && user.token) {
                        // store user details and jwt token in local storage to keep user logged in between page refreshes
                        localStorage.setItem('currentUser', JSON.stringify(user));
                        this.setUser(user);
                    }
                    return user;
                })
            )
            .toPromise();
    }

    public async logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        this.setUser(null);
        this.router.navigate(['']);
    }

    public async register(user: User) : Promise<User> {
        return await this.http.post<User>(`${environment.apiUrl}/user/register`, user)
        .toPromise();
    }

    public getUser() : Observable<User>{
        let loggedUser = this.checkIfUserIsAlreadyLoggedIn();

        if(loggedUser != null)
            this.setUser(loggedUser);
        
        return this.subject.asObservable();
    }

    private checkIfUserIsAlreadyLoggedIn() : User {
        let loggedUser = JSON.parse(localStorage.getItem('currentUser'));
        if(loggedUser != null){
            return new User(loggedUser.username, loggedUser.password, loggedUser.token);
        }
        return null;
    }

    private setUser(user : User){
        this.subject.next(user);
    }
}
