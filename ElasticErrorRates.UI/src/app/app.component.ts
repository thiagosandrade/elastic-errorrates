import { Component, OnInit } from '@angular/core';
import { SignalRService } from './_shared/signalR/signalR.service';
import { SignalRMessage } from './_shared/signalR/signalR.message';
import { ApiUserService } from './_shared/api/user/api-user.service';
import { User } from './_shared/api/user/model/user';
import { MessageNotifierService } from './_shared/messageNotifier/messageNotifier.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {

  title = 'app';
  public isLoggedIn: boolean = false;

  constructor(private signalRService: SignalRService,
    private apiUserService: ApiUserService,
    private messageNotifierService: MessageNotifierService) { }

  ngOnInit(): void {
    this.signalRService.notificationReceived.subscribe((signalRMessage: SignalRMessage[]) => {
      signalRMessage.forEach(message => {
        this.messageNotifierService.messageNotify(message.type, message.payload);
      });
    });

    this.apiUserService.getUser().subscribe((user: User) => {
      if (user != null && user.token != "") {
        this.isLoggedIn = true;
      }
    });
  }

  onLogout(): void {
    this.apiUserService.logout();
    this.messageNotifierService.messageNotify('success', 'User logged out!');
    this.isLoggedIn = false;
  }

}
