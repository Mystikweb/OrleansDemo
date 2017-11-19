import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Event, NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {
  navOpened = false;

  constructor(private router: Router) { }

  ngOnInit() {
    this.router.events.subscribe((event: Event) => {
      if (event instanceof NavigationEnd) {
        this.navOpened = false;
      }
    });
  }

  onToggled(opened: boolean) {
    this.navOpened = opened;
  }
}
