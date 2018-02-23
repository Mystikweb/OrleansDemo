import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  navOpened = false;

  toggleNavMenu(opened: boolean) {
    this.navOpened = opened;
  }
}
