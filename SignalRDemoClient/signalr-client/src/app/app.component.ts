import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SignalRService } from './services/signal-r.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'signalr-client';

  constructor(public signalRService: SignalRService, private http: HttpClient) {}

  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.addGetAllApprenticesDataListener();
    this.getAllApprentices();
  }

  private getAllApprentices(){
    // Get apprentices on page load 
    this.http.get(environment.apprenticeApiBaseUrl).subscribe((apprentices: any) => 
      {
        this.signalRService.apprentices = apprentices;
        console.log(apprentices);
      });
  }
}
