import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Event, NavigationEnd, Router } from '@angular/router';

import { DetailsHostService } from './shared/details-host/details-host.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {
  navOpened = false;
  detailsOpened = false;

  constructor(private router: Router,
    private detailsHost: DetailsHostService) { }

  ngOnInit() {
    this.detailsHost.detailsOpen$.subscribe(opened => this.detailsOpened = opened);
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
