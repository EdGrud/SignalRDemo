import { Injectable, OnDestroy } from '@angular/core';
import { Apprentice } from '../models/apprentice';
import * as signalR from "@microsoft/signalr";
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRService implements OnDestroy {
  public apprentices: Apprentice[] = [];

  private apprenticeHubConnection: signalR.HubConnection | undefined

  public startConnection() {
    // Create a connection to the apis signalR hub route
    this.apprenticeHubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.apprenticeHubUrl)
      .build();
    
    // Start Connection
    this.apprenticeHubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  public addGetAllApprenticesDataListener() {
    // Subscribe to the getallapprentices call
    this.apprenticeHubConnection?.on('getallapprentices', (apprentices) => {
      this.apprentices = apprentices;
      console.log(apprentices);
    });
  }

  ngOnDestroy(): void {
    // Clean up, closing the connection
    this.apprenticeHubConnection?.stop().then(() => {
      console.log('Connection closed');
    });
  }
}
